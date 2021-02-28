using Strategy;
using Strategy.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StrategyServiceCollectionExtensions
    {
        public static IServiceCollection AddStrategyResolver<TStrategy>(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TStrategy : class
        {
            // Declare a new service descriptor
            var descriptor = new ServiceDescriptor(typeof(IStrategyResolver<TStrategy>),
                typeof(AttributeStrategyResolver<TStrategy>), lifetime);

            // Add the strategy resolver service to the collection
            services.Add(descriptor);

            return services;
        }
    }
}
