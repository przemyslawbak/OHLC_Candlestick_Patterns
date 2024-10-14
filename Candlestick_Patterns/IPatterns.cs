namespace Candlestick_Patterns
{
    public interface IPatterns
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetPatternsSignalsQuantities(string patternName);
        int GetSignalsCount(string patternName);
    }
}
