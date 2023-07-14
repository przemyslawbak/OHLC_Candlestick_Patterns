namespace Candlestick_Patterns
{
    public interface ISignals
    {
        int GetBullishSignalsCount();
        int GetBearishSignalsCount();
        int GetSignalsCount(string[] patternNames);
        int GetSignalsCount(string patternName);
        decimal GetSignalsIndex(string patternName, decimal weight);
        decimal GetSignalsIndex(Dictionary<string, decimal> patternNamesWithWeights);
        List<OhlcvObject> GetOhlcvWithSignals(string patternName);
    }
}
