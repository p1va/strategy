using System;
using static EnsureThat.EnsureArg;

namespace Strategy.Attributes
{
    /// <summary>
    /// The strategy attribute class
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class StrategyAttribute : Attribute
    {
        public StrategyAttribute(string key) =>
            (Key, IsDefault) = (IsNotEmptyOrWhiteSpace(key, nameof(key)), false);

        public StrategyAttribute(string key, bool isDefault) =>
            (Key, IsDefault) = (IsNotEmptyOrWhiteSpace(key, nameof(key)), isDefault);

        /// <summary>
        /// Gets the value of the key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets a flag describing if this strategy is the default one
        /// </summary>
        public bool IsDefault { get; }
    }
}
