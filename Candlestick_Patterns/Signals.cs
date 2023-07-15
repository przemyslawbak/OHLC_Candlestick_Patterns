namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        IPatterns _patterns;

        public int GetBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);

            var bullishMethodNames = _patterns.GetAllMethodNames().Where(x => x.StartsWith("Bullish")).ToList();

            List<int> bullishQty = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                bullishQty.Add(_patterns.GetSignalsCount(methodName));
            }

            return bullishQty.Sum(x => x);
        }

        public int GetBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);

            var bullishMethodNames = _patterns.GetAllMethodNames().Where(x => x.StartsWith("Bearish")).ToList();

            List<int> bullishQty = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                bullishQty.Add(_patterns.GetSignalsCount(methodName));
            }

            return bullishQty.Sum(x => x);
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
            return _patterns.GetSignalsCount(patternName);
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
