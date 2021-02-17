# Strategy
A lightweight implementation of the strategy patter where the creation of instances is handled by the dependency injection framework.

### Example
Declare the strategy interface
```csharp
public interface IShippingService
{
    void Ship(string address);
}
```
Mark the different strategy implementations with the `[Strategy("key")]` attribute with the associated key.
```csharp
[Strategy("fedex")]
public class FedexShippingService : IShippingService
{
    public void Ship(string address) => ...;
}

[Strategy("ups")]
public class UpsShippingService : IShippingService
{
    public void Ship(string address) => ...;
}
```

Eventually add a default implementation to fallback to in case of no matches
```csharp
[DefaultStrategy]
public class DefaultShippingService : IShippingService
{
    public void Ship(string address) => ...;
}
```

After adding the strategy implementation to the `IServiceCollection` register the strategy resolver

```csharp
services
    .AddScoped<IShippingService, FedexShippingService>()
    .AddScoped<IShippingService, UpsShippingService>()
    .AddScoped<IShippingService, DefaultShippingService>()
    .AddStrategyResolver<IShippingService>();
```
In your code let the DI inject `IStrategyResolver<T>`

```csharp
public OrderService(IStrategyResolver<IShippingService> resolver) =>
    (_strategyResolver) = (resolver);
```

And use it to resolve the implementation matching the request key

```csharp
foreach (var order in orders)
{
    _strategyResolver
        .Resolve(order.ShippingMethod)?
        .Ship(order.Address);
}
```