
#if !SILVERLIGHT
using System.Collections.Specialized;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;

namespace Reflex
{
    public static class Transform
    {
        /// <summary>
        /// Transform an object into a dictionary using property name as key and
        /// property value as dictionary value.
        /// </summary>
        public static IDictionary<string, object> ToDictionary(object src)
        {
            if (src == null) throw new ArgumentNullException("src");

            var dict = new Dictionary<string, object>();
            var props = Get.Properties(src);

            foreach (var prop in props)
            {
                var value = prop.GetValue(src, Consts.Dud);
                dict.Add(prop.Name, value);
            }

            return dict;
        }

        /// <summary>
        /// Transform an object into a dictionary with property names as key and
        /// property values converted to TValue as dictionary value.
        /// </summary>
        public static IDictionary<string, TValue> ToDictionary<TValue>(object src)
        {
            if (src == null) throw new ArgumentNullException("src");

            var dict = new Dictionary<string, TValue>();
            var props = Get.Properties(src);

            foreach (var prop in props)
            {

                try
                {
#if !SILVERLIGHT
                    var value = (TValue)Convert.ChangeType(
                        prop.GetValue(src, Consts.Dud),
                        typeof(TValue));
#else
                    var value = (TValue)prop.GetValue(src, Consts.Dud);
#endif

                    dict.Add(prop.Name, value);

                }
                catch (FormatException)
                {
                    dict.Add(prop.Name, default(TValue));

                }
            }

            return dict;
        }

        /// <summary>
        /// Transform an object into a dictionary with property names as key and
        /// property values converted to string as dictionary value.
        /// </summary>
        public static IDictionary<string, string> ToStringDict(object src)
        {
            if (src == null) throw new ArgumentNullException("src");

            var dict = ToDictionary(src);
            var result = new Dictionary<string, string>();

            foreach (var item in dict)
                result.Add(item.Key,
                    item.Value == null ? null : Convert.ToString(item.Value));

            return result;
        }

#if !SILVERLIGHT
        /// <summary>
        /// Converts a NameValueCollection to an equivalent string dictionary.
        /// </summary>
        public static IDictionary<string, string> ToStringDict(NameValueCollection col)
        {
            if (col == null) throw new ArgumentNullException("col");

            var result = new Dictionary<string, string>();

            foreach (var key in col.AllKeys)
                result.Add(key, col[key]);

            return result;
        }

        /// <summary>
        /// Transform an object into a NameValueCollection using property name as key and
        /// property value converted to string as dictionary value.
        /// </summary>
        public static NameValueCollection ToNameValue(object src)
        {
            if (src == null) throw new ArgumentNullException("src");

            var props = Get.Properties(src);
            var result = new NameValueCollection();

            foreach (var prop in props)
            {
                var value = prop.GetValue(src, Consts.Dud);
                result.Add(prop.Name, value == null ? null : Convert.ToString(value));
            }

            return result;
        }

        /// <summary>
        /// Converts a string dictionary to an equivalent NameValueCollection.
        /// </summary>
        public static NameValueCollection ToNameValue(IDictionary<string, string> dict)
        {
            if (dict == null) throw new ArgumentNullException("dict");

            var result = new NameValueCollection();

            foreach (var item in dict)
                result.Add(item.Key, item.Value);

            return result;
        }
#endif

        /// <summary>
        /// Creates an object (and a new type) from the dictionary
        /// using dictionary key as property name and dictionary value as property value.
        /// </summary>
        [Obsolete("Until further optimization is done, this method " +
            "must be used with care as it can become an instant bottleneck.")]
        public static object ToObject(IDictionary<string, object> dict)
        {
            if (dict == null) throw new ArgumentNullException("dict");

            // TODO: Generalize this into a proper setup routine
            //       and maybe find a way to reuse asm instances
            //       and/or cache it to disk.

            // create a new type name
            var guid = Guid.NewGuid().ToString().Substring(0, 4);
            var name = "EmittedAsm" + guid;
            var moduleName = "EmittedModule" + guid;
            var className = "EmittedClass" + guid;

            // create an assembly to host the new type
            var domain = AppDomain.CurrentDomain;
            var asmName = new AssemblyName(name);

            var asmBuilder = domain.DefineDynamicAssembly(asmName,
                AssemblyBuilderAccess.Run);

            // build a public class
            var moduleBuilder = asmBuilder.DefineDynamicModule(moduleName);
            var typeBuilder = moduleBuilder.DefineType(className,
                TypeAttributes.Public | TypeAttributes.Class);

            // build type's properties
            foreach (var item in dict)
            {
                var returnType = item.Value.GetType();

                // define a backing field for the property
                var field = typeBuilder.DefineField("_" + item.Key,
                    returnType, FieldAttributes.Private);

                // define the property
                var prop = typeBuilder.DefineProperty(item.Key,
                    PropertyAttributes.None,
                    returnType, new[] { returnType });

                // make a "getter" method that reads the backing field
                var getter = typeBuilder.DefineMethod("Get" + item.Key,
                    MethodAttributes.Public, returnType, Consts.DudTypes);

                var emitter = getter.GetILGenerator();
                emitter.Emit(OpCodes.Ldarg_0);
                emitter.Emit(OpCodes.Ldfld, field);
                emitter.Emit(OpCodes.Ret);

                // make a "setter" method that sets the backing field's value
                var setter = typeBuilder.DefineMethod("Set" + item.Key,
                    MethodAttributes.Public, null,
                    new[] { returnType });

                emitter = setter.GetILGenerator();
                emitter.Emit(OpCodes.Ldarg_0);
                emitter.Emit(OpCodes.Ldarg_1);
                emitter.Emit(OpCodes.Stfld, field);
                emitter.Emit(OpCodes.Ret);

                // attach getter and setter to the property
                prop.SetGetMethod(getter);
                prop.SetSetMethod(setter);

            }


            // materialize the type
            var type = typeBuilder.CreateType();

            // create the object and load values from the dictionary
            var obj = Activator.CreateInstance(type);

            foreach (var item in dict)
            {
                var prop = type.GetProperty(item.Key);
                prop.SetValue(obj, item.Value, Consts.Dud);
            }

            return obj;
        }


        /// <summary>
        /// Builds a Delegate instance from the supplied MethodInfo object and a target to invoke against.
        /// </summary>
        public static Delegate ToDelegate(MethodInfo mi, object target)
        {
            if (mi == null) throw new ArgumentNullException("mi");

            Type delegateType;

            var typeArgs = mi.GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            // builds a delegate type
            if (mi.ReturnType == typeof(void))
            {
                delegateType = Expression.GetActionType(typeArgs.ToArray());

            }
            else
            {
                typeArgs.Add(mi.ReturnType);
                delegateType = Expression.GetFuncType(typeArgs.ToArray());
            }

            // creates a binded delegate if target is supplied
            var result = (target == null)
                ? Delegate.CreateDelegate(delegateType, mi)
                : Delegate.CreateDelegate(delegateType, target, mi);

            return result;
        }

        public static Delegate ToDelegate(MethodInfo mi)
        {
            if (mi == null) throw new ArgumentNullException("mi");

            return ToDelegate(mi, null);
        }
    }
}
