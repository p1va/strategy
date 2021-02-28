using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using Strategy.Abstractions;
using Strategy.Attributes;
using Strategy.Extension;
using Xunit;

namespace Strategy.IntegrationTests
{
    public class IntegrationTests
    {
        [Theory(DisplayName = "Should return desired strategy or default")]
        [InlineData("fedex", "FedexShippingService", "FedexShippingService")]
        [InlineData("ups", "UpsShippingService", "UpsShippingService")]
        [InlineData("not-exists", "DefaultShippingService", "DefaultShippingService")]
        public void ShouldReturnStrategy(string key, string expectedTypeName, string expectedResult)
        {
            // Arrange
            var services = new ServiceCollection()
                .AddScoped<IShippingService, FedexShippingService>()
                .AddScoped<IShippingService, UpsShippingService>()
                .AddScoped<IShippingService, DefaultShippingService>()
                .AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            // Act
            var strategy = resolver?.Resolve(key);

            // Assert
            strategy.ShouldNotBeNull();
            strategy.GetType().Name.ShouldBe(expectedTypeName);
            strategy.Ship("the address").ShouldBe(expectedResult);
        }

        [Fact(DisplayName = "Should throw when no marked strategy registered")]
        public void ShouldThrowWhenNoStrategyRegistered()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddScoped<IShippingService, NotRegisteredShippingService>()
                .AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            var key = Guid.NewGuid().ToString();

            // Act
            var exception = Record.Exception(() => resolver!.Resolve(key));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<StrategyException>();
            exception.Message.ShouldBe("No strategy registration found for service IShippingService");
        }

        [Fact(DisplayName = "Should throw when no default and matching strategy found")]
        public void ShouldThrowWhenNoDefaultAndMatchingStrategyFound()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddScoped<IShippingService, UpsShippingService>()
                .AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            var key = Guid.NewGuid().ToString();

            // Act
            var exception = Record.Exception(() => resolver!.Resolve(key));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<StrategyException>();
            exception.Message.ShouldBe(
                $"No strategy registration found having key '{key}' for service IShippingService");
        }

        [Fact(Skip = "Skip")]
        public void Test1()
        {
            // Arrange
            var strategyAMock = new Mock<IShippingService>(MockBehavior.Strict);
            var strategyBMock = new Mock<IShippingService>(MockBehavior.Strict);
            var strategyCMock = new Mock<IShippingService>(MockBehavior.Strict);

            var strategyTypeA = strategyAMock.Object.GetType();
            var strategyTypeB = strategyBMock.Object.GetType();
            var strategyTypeC = strategyCMock.Object.GetType();

            var strategyInstanceA = strategyAMock.Object;
            var strategyInstanceB = strategyBMock.Object;
            var strategyInstanceC = strategyCMock.Object;

            var strategyAttributeA = new StrategyAttribute("strategy_a");
            var strategyAttributeB = new StrategyAttribute("strategy_b");
            var strategyAttributeC = new StrategyAttribute("strategy_c");

            TypeDescriptor.AddAttributes(strategyInstanceA, strategyAttributeA);
            TypeDescriptor.AddAttributes(strategyInstanceB, strategyAttributeB);
            TypeDescriptor.AddAttributes(strategyInstanceC, strategyAttributeC);

            var services = new ServiceCollection
            {
                new ServiceDescriptor(typeof(IShippingService), _ => strategyAMock.Object, ServiceLifetime.Scoped),
                new ServiceDescriptor(typeof(IShippingService), _ => strategyBMock.Object, ServiceLifetime.Scoped),
                new ServiceDescriptor(typeof(IShippingService), _ => strategyCMock.Object, ServiceLifetime.Scoped)
            };

            services.AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            // Act
            var strategy = resolver!.Resolve("strategy_c");

            // Assert
            strategy.ShouldNotBeNull();

            // Retrieve associated attribute
            var strategyAttribute = strategy.GetType()
                .GetRuntimeAttribute<StrategyAttribute>()
                .FirstOrDefault();

            strategyAttribute.ShouldNotBeNull();
            strategyAttribute.Key.ShouldBe("strategy_c");
        }
    }
}
