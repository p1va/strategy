using System;

namespace Strategy
{
    public class StrategyException : Exception
    {
        public StrategyException(string message) : base(message)
        {
        }
    }
}
