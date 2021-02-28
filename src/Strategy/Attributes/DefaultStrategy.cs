using System;

namespace Strategy.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DefaultStrategyAttribute : StrategyAttribute
    {
        private const string DefaultKey = "default";

        public DefaultStrategyAttribute() : base(DefaultKey, true)
        {
        }

        public DefaultStrategyAttribute(string key) : base(key, true)
        {
        }
    }
}
