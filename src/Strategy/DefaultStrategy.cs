using System;

namespace Strategy
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DefaultStrategyAttribute : StrategyAttribute
    {
        public DefaultStrategyAttribute() : base(string.Empty, true)
        {
        }

        public DefaultStrategyAttribute(string key) : base(key, true)
        {
        }
    }
}
