namespace Candlestick_Patterns
{
    public interface IPatterns
    {
        IEnumerable<string> GetAllMethodNames();
        int GetSignalsCount(object methodName);
    }
}
