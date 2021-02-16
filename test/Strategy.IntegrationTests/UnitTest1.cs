using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit;

namespace Strategy.IntegrationTests
{
    public interface IShippingService
    {
        string Ship(string address);
    }

    [Strategy("fedex")]
    public class FedexShippingService : IShippingService
    {
        public string Ship(string address) => nameof(FedexShippingService);
    }

    [Strategy("ups")]
    public class UpsShippingService : IShippingService
    {
        public string Ship(string address) => nameof(UpsShippingService);
    }

    [DefaultStrategy]
    public class DefaultShippingService : IShippingService
    {
        public string Ship(string address) => nameof(DefaultShippingService);
    }

    public class UnitTest1
    {
        [Fact(Skip = "Skip this one")]
        public void Test1()
        {
            var strategyAMock = new Mock<IShippingService>(MockBehavior.Strict);
            var strategyBMock = new Mock<IShippingService>(MockBehavior.Strict);

            strategyBMock
                .Setup(x => x.Ship(It.IsAny<string>()))
                .Verifiable();

            var strategyAttributeA = new StrategyAttribute("strategy_a");
            var strategyAttributeB = new StrategyAttribute("strategy_b");

            TypeDescriptor.AddAttributes(strategyAMock.Object.GetType(), strategyAttributeA);
            TypeDescriptor.AddAttributes(strategyBMock.Object.GetType(), strategyAttributeB);

            var services = new ServiceCollection();

            services.AddScoped<IShippingService>(sp => strategyAMock.Object);
            services.AddScoped<IShippingService>(sp => strategyBMock.Object);
            services.AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            resolver?
                .Resolve("strategy_b")
                ?
                .Ship("the address");

            strategyAMock.Verify();
            strategyBMock.Verify();
            strategyBMock.Verify(x => x.Ship(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("fedex", "FedexShippingService")]
        [InlineData("ups", "UpsShippingService")]
        [InlineData("not-exists", "DefaultShippingService")]
        public void Test2(string key, string expectedImplementation)
        {
            // Arrange
            var services = new ServiceCollection();

            services
                .AddScoped<IShippingService, FedexShippingService>()
                .AddScoped<IShippingService, UpsShippingService>()
                .AddScoped<IShippingService, DefaultShippingService>()
                .AddStrategyResolver<IShippingService>();

            var provider = services.BuildServiceProvider();

            var resolver = provider.GetService<IStrategyResolver<IShippingService>>();

            // Act
            var implementationName = resolver?
                .Resolve(key)?
                .Ship("the address");

            // Assert
            implementationName.ShouldBe(expectedImplementation);
        }
    }
}
