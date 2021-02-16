using System;

namespace Strategy
{
    /// <summary>
    /// The strategy attribute class
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class StrategyAttribute : Attribute
    {
        public StrategyAttribute(string key) =>
            (Key, IsDefault) = (key, false);

        public StrategyAttribute(string key, bool isDefault) =>
            (Key, IsDefault) = (key, isDefault);

        /// <summary>
        /// Gets the value of the key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets a flag describing if this strategy is the default one
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
