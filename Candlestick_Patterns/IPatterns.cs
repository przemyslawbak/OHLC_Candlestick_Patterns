namespace Candlestick_Patterns
{
    public interface IPatterns
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetSignals(string patternName);
        int GetSignalsCount(string patternName);
    }
}
