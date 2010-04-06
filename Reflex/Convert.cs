
using System;

using SysConvert = System.Convert;

namespace Reflex
{
    public static class Convert
    {
        public static T To<T>(object value)
        {
            if (value == null)
                return default(T);

            try
            {
                return (T)SysConvert.ChangeType(value, typeof(T));
            }
            catch (FormatException) { return default(T); }
            catch (InvalidCastException) { return default(T); }
        }

        public static object To(Type type, object value)
        {
            // helper for getting defaults for value types
            Func<object> defaultValue;
            if (type.IsValueType)
                defaultValue = () => Activator.CreateInstance(type);
            else
                defaultValue = () => null;


            if (value == null) return defaultValue();

            try
            {
                return SysConvert.ChangeType(value, type);
            }
            catch (FormatException) { return defaultValue(); }
            catch (InvalidCastException) { return defaultValue(); }
        }

        // TODO: temporary plug to make it compiles,
        //       should be removed and new implementation used in-place afterwards
        public static string ToString(object o)
        {
            return SysConvert.ToString(o);
        }

        public static object ChangeType(object value, Type conversionType)
        {
            return SysConvert.ChangeType(value, conversionType);
        }

    }
}
