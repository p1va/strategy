using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Strategy
{
    public class FuncStrategyResolver<TStrategy> : IStrategyResolver<TStrategy> where TStrategy : class
    {
        private readonly IServiceProvider _provider;

        public FuncStrategyResolver(IServiceProvider provider) =>
            (_provider) = (provider);

        /// <summary>
        /// Resolves the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The service</returns>
        public TStrategy? Resolve(string key)
        {
            throw new NotImplementedException();
        }
    }
}
