
#if !SILVERLIGHT
using System.Collections.Specialized;
#endif

using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflex
{
    public static class Map
    {
        /// <summary>
        /// Copy values from source object's properties to target object properties
        /// where both property has the same name.
        /// </summary>
        public static void Properties(object src, object target)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (target == null) throw new ArgumentNullException("target");

            var srcProps = Get.Properties(src, Consts.DefaultFlags);
            var targetType = target.GetType();

            foreach (var prop in srcProps)
            {

                var targetProp = targetType.GetProperty(prop.Name, Consts.DefaultFlags);
                if (targetProp == null) continue;

                var srcValue = prop.GetValue(src, Consts.Dud);
                targetProp.SetValue(target, srcValue, Consts.Dud);

            }
        }

        /// <summary>
        /// Copy values from the dictionary to the target object's properties
        /// where the dictionary key matched a property name.
        /// </summary>
        public static void Dictionary(IDictionary<string, object> dict, object target)
        {
            if (dict == null) throw new ArgumentNullException("dict");
            if (target == null) throw new ArgumentNullException("target");

            var type = target.GetType();

            foreach (var item in dict)
            {

                var prop = type.GetProperty(item.Key, Consts.DefaultFlags);
                prop.SetValue(target, item.Value, Consts.Dud);

            }
        }

        /// <summary>
        /// Copy values from the dictionary to the target object's properties
        /// and attempt to parse/convert it to the appropriate type
        /// where the dictionary key matched a property name.
        /// </summary>
        public static void StringDict(IDictionary<string, string> dict, object target)
        {
            if (dict == null) throw new ArgumentNullException("dict");
            if (target == null) throw new ArgumentNullException("target");

            var type = target.GetType();

            foreach (var item in dict)
            {
                var prop = type.GetProperty(item.Key, Consts.DefaultFlags);

                // TODO: Make a version of this that doesn't throw exception
                if (prop == null) throw new ArgumentException(
                    "Target object doesn't have property '" + item.Key + "', please remove it from the dictionary.", "dict");

                var value = Convert.To(prop.PropertyType, item.Value);
                prop.SetValue(target, value, Consts.Dud);
            }
        }


#if !SILVERLIGHT
        /// <summary>
        /// Copy values from the name value collection to the target object's properties
        /// and attempt to parse/convert it to the appropriate type
        /// where the name of the item matched a property name.
        /// </summary>
        public static void NameValue(NameValueCollection col, object target)
        {
            if (col == null) throw new ArgumentNullException("col");
            if (target == null) throw new ArgumentNullException("target");

            // convert to string dictionary
            var dict = col.AllKeys.ToDictionary(x => x, x => col[x]);

            StringDict(dict, target);
        }
#endif
    }
}
