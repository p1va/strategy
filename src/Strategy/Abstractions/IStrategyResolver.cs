namespace Strategy.Abstractions
{
    public interface IStrategyResolver<out TStrategy> where TStrategy : class
    {
        TStrategy Resolve(string key);
    }
}
