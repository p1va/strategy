using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Strategy
{
    public class AttributeStrategyResolver<TStrategy> : IStrategyResolver<TStrategy> where TStrategy : class
    {
        private readonly IServiceProvider _provider;

        public AttributeStrategyResolver(IServiceProvider provider) =>
            (_provider) = (provider);

        /// <summary>
        /// Resolves the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The service</returns>
        public TStrategy? Resolve(string key)
        {
            // Retrieve implementations of TStrategy having the Strategy attribute
            var strategies = _provider
                .GetServices<TStrategy>()
                .Where(s => s!.AnyAttribute<StrategyAttribute>())
                .ToList();

            // Try to find the strategy by key
            var strategy = strategies.FirstOrDefault(s => s
                .AnyAttribute<StrategyAttribute>(attr => attr.Key == key));

            if (strategy is not null)
            {
                return strategy;
            }

            // No strategy found
            // Try to search the default one as a fallback
            var defaultService = strategies.FirstOrDefault(s => s
                .AnyAttribute<StrategyAttribute>(attr => attr.IsDefault));

            if (defaultService is not null)
            {
                return defaultService;
            }

            throw new Exception($"No strategy registration found for for service {typeof(TStrategy).Name}.");
        }
    }
}
