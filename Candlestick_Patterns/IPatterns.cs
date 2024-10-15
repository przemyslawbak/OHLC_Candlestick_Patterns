namespace Candlestick_Patterns
{
    internal interface IPatterns
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetPatternsSignalsQuantities(string patternName);
        int GetSignalsCount(string patternName);
    }
}
