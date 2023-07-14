namespace Candlestick_Patterns
{
    public interface ISignals
    {
        int GetBullishSignalsCount(List<OhlcvObject> dataOhlcv);
        int GetBearishSignalsCount(List<OhlcvObject> dataOhlcv);
        int GetSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames);
        int GetSignalsCount(List<OhlcvObject> dataOhlcv, string patternName);
        decimal GetSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight);
        decimal GetSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights);
        List<OhlcvObject> GetOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string patternName);
    }
}
