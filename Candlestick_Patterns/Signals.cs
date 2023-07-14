namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        public int GetBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            throw new NotImplementedException();
        }

        public int GetBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            throw new NotImplementedException();
        }

        public List<OhlcvObject> GetOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string patternName)
        {
            throw new NotImplementedException();
        }

        public int GetSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            throw new NotImplementedException();
        }

        public int GetSignalsCount(List<OhlcvObject> dataOhlcv, string patternName)
        {
            throw new NotImplementedException();
        }

        public decimal GetSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight)
        {
            throw new NotImplementedException();
        }

        public decimal GetSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights)
        {
            throw new NotImplementedException();
        }
    }
}
