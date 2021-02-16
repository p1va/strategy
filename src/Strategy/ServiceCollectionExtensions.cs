using Strategy;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStrategyResolver<TStrategy>(this IServiceCollection services)
            where TStrategy : class
        {
            services.AddScoped<IStrategyResolver<TStrategy>, AttributeStrategyResolver<TStrategy>>();

            return services;
        }
    }
}
