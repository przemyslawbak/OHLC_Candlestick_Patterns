namespace Candlestick_Patterns
{
    public interface IPatterns
    {
        IEnumerable<string> GetAllMethodNames();
        List<OhlcvObject> GetSignals(string patternName);
        int GetSignalsCount(string methodName);
    }
}
