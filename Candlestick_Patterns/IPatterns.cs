namespace Candlestick_Patterns
{
    public interface IPatterns
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetPatternsSignalsList(string patternName);
        int GetSignalsCount(string patternName);
    }
}
