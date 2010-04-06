
using System;
using System.Linq;
using System.Reflection;

namespace Reflex
{
    /// <summary>
    /// Provides utility for getting various values and information from an object.
    /// </summary>
    public static class Get
    {
        /// <summary>
        /// Gets a PropertyInfo with the supplied name from the referenced object.
        /// </summary>
        public static PropertyInfo Property(object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(propName)) throw new ArgumentNullException("propName");

            var type = obj.GetType();
            var prop = type.GetProperty(propName);

            return prop;
        }

        /// <summary>
        /// Gets the value of the requested property from the object.
        /// </summary>
        public static object PropertyValue(object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(propName)) throw new ArgumentNullException("propName");

            var type = obj.GetType();
            var prop = type.GetProperty(propName);

            if (prop == null) throw new ArgumentException(
                "Could not find property '" + propName +
                "' on object of type " + type.Name, "propName");

            return prop.GetValue(obj, Consts.Dud);
        }

        /// <summary>
        /// Gets a list of PropertyInfos from the referenced object.
        /// </summary>
        public static PropertyInfo[] Properties(object o)
        {
            return Get.Properties(o, Consts.DefaultFlags);
        }

        /// <summary>
        /// Gets a list of PropertyInfos with the supplied binding from the referenced object
        /// </summary>
        public static PropertyInfo[] Properties(object o, BindingFlags flags)
        {
            if (o == null)
                throw new ArgumentNullException("o",
                    "Invoking ObjectExtensions.GetProperties on a null reference.");

            var type = o.GetType();
            return type.GetProperties(flags);
        }


        /// <summary>
        /// Gets a method with the specified name from the referenced object.
        /// </summary>
        public static MethodInfo Method(object o, string name)
        {
            if (o == null) throw new ArgumentNullException("o");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            var type = o.GetType();
            var method = type.GetMethod(name);

            return method;
        }

        /// <summary>
        /// Gets a list of MethodInfos from the referenced object.
        /// </summary>
        public static MethodInfo[] Methods(object o)
        {
            if (o == null) throw new ArgumentNullException("o");

            return Methods(o, Consts.DefaultFlags);
        }

        /// <summary>
        /// Gets a list of MethodInfos with the supplied binding from the referenced object
        /// </summary>
        public static MethodInfo[] Methods(object o, BindingFlags flags)
        {
            if (o == null) throw new ArgumentNullException("o");

            var type = o.GetType();
            var methods = type.GetMethods(flags);

            methods = methods
                .Where(method => !Consts.IgnoredMethodPrefixes
                    .Any(prefix => method.Name.StartsWith(prefix)))
                .ToArray();

            return methods;
        }

    }
}
