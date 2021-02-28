using Strategy.Attributes;

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

    public class NotRegisteredShippingService : IShippingService
    {
        public string Ship(string address) => nameof(NotRegisteredShippingService);
    }
}
