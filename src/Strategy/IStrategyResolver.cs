namespace Strategy
{
    public interface IStrategyResolver<out TStrategy> where TStrategy : class
    {
        TStrategy? Resolve(string key);
    }
}
