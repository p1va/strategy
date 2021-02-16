using System;
using System.Linq;

namespace Strategy
{
    public static class ObjectExtensions
    {
        public static bool AnyAttribute<TAttribute>(this object @object, Func<TAttribute, bool> func)
            where TAttribute : Attribute => @object
            .GetType()
            .GetCustomAttributes(inherit: false)
            .OfType<TAttribute>()
            .Any(func);

        public static bool AnyAttribute<TAttribute>(this object @object)
            where TAttribute : Attribute => @object
            .GetType()
            .GetCustomAttributes(inherit: false)
            .OfType<TAttribute>()
            .Any();
    }
}
