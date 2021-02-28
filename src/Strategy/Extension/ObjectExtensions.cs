using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Strategy.Extension
{
    public static class ObjectExtensions
    {
        public static bool AnyAttribute<TAttribute>(this object @object, Func<TAttribute, bool> func)
            where TAttribute : Attribute => @object
            .GetType()
            .GetRuntimeAttribute<TAttribute>()
            .Any(func);

        public static bool AnyAttribute<TAttribute>(this object @object)
            where TAttribute : Attribute => @object
            .GetType()
            .GetRuntimeAttribute<TAttribute>()
            .Any();

        /// <summary>
        /// Gets a runtime added attribute to a type.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>The first attribute or null if none is found.</returns>
        public static IEnumerable<TAttribute> GetRuntimeAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute => TypeDescriptor
            .GetAttributes(type)
            .OfType<TAttribute>();
    }
}
