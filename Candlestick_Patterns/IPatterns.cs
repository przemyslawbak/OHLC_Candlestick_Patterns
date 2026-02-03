namespace Candlestick_Patterns
{
    public interface IPatterns : ISignalEngine
    {
        List<string> GetAllMethodNames();
        List<OhlcvObject> GetPatternsSignalsList(string patternName);
        int GetSignalsCount(string patternName);
    }
}
