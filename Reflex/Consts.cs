
using System;
using System.Reflection;

namespace Reflex
{
    /// <summary>
    /// Provides commonly used constants.
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// An empty object array to indicate a non-indexed property.
        /// </summary>
        public static readonly object[] Dud = new object[] { };

        /// <summary>
        /// An empty type array to indicate that there is no parameters.
        /// </summary>
        public static readonly Type[] DudTypes = new Type[] { };

        /// <summary>
        /// Commonly used BindingFlags value. (public instance except inherited members)
        /// </summary>
        public static readonly BindingFlags DefaultFlags =
            BindingFlags.Public |
            BindingFlags.Instance |
            BindingFlags.DeclaredOnly;


        /// <summary>
        /// Common prefixes of auto-generated code that should be filtered out.
        /// (i.e. property getters/setters)
        /// </summary>
        public static readonly string[] IgnoredMethodPrefixes = new[] { "get_", "set_" };
    }
}
