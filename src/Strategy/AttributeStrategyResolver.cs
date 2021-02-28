using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Strategy.Abstractions;
using Strategy.Attributes;
using Strategy.Extension;
using static EnsureThat.EnsureArg;

namespace Strategy
{
    public class AttributeStrategyResolver<TStrategy> : IStrategyResolver<TStrategy> where TStrategy : class
    {
        private readonly IServiceProvider _provider;

        public AttributeStrategyResolver(IServiceProvider provider) =>
            (_provider) = (IsNotNull(provider, nameof(IServiceProvider)));

        /// <summary>
        /// Resolves the strategy associated to the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The service</returns>
        public TStrategy Resolve(string key)
        {
            // Ensure the key is valid
            IsNotNullOrWhiteSpace(key, nameof(key));

            var services = _provider
                .GetServices<TStrategy>();

            // Retrieve all the implementations of TStrategy that have the Strategy attribute
            var strategies = _provider
                .GetServices<TStrategy>()
                .Where(s => s.AnyAttribute<StrategyAttribute>())
                .ToList();

            // If none was found throw an exception to inform to register at least one strategy
            if (strategies.Any() is not true)
            {
                throw new StrategyException($"No strategy registration found for service {typeof(TStrategy).Name}");
            }

            // Find the strategy that matches the key
            var strategy = strategies.FirstOrDefault(s => s
                .AnyAttribute<StrategyAttribute>(attr => attr.Key == key));

            // Strategy found
            if (strategy is not null)
            {
                return strategy;
            }

            // No strategy found
            // Try to fallback on the default strategy
            var defaultStrategy = strategies.FirstOrDefault(s => s
                .AnyAttribute<StrategyAttribute>(attr => attr.IsDefault));

            // Default strategy found
            if (defaultStrategy is not null)
            {
                return defaultStrategy;
            }

            // No strategy found matching the provided key
            throw new StrategyException(
                $"No strategy registration found having key '{key}' for service {typeof(TStrategy).Name}");
        }
    }
}
