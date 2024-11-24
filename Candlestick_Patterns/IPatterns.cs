namespace Candlestick_Patterns
{
    internal interface IPatterns
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetPatternsSignalsList(string patternName);
        int GetSignalsCount(string patternName);
    }
}
