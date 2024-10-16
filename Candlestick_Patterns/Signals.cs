namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        IPatterns _patterns;
        IFormations _formations;


        public int GetPatternsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);

            var bullishMethodNames = _patterns.GetAllMethodNames().Where(x => x.StartsWith("Bullish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_patterns.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetPatternsBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);

            var bullishMethodNames = _patterns.GetAllMethodNames().Where(x => x.StartsWith("Bearish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_patterns.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public List<OhlcvObject> GetPatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetPatternsSignalsQuantities(patternName);
        }

        public List<List<OhlcvObject>> GetPatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            _patterns = new Patterns(dataOhlcv);

            List<List<OhlcvObject>> list = new List<List<OhlcvObject>>();

            foreach (var methodName in patternNames)
            {
                list.Add(_patterns.GetPatternsSignalsQuantities(methodName));
            }

            return list;
        }

        public int GetPatternsSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            _patterns = new Patterns(dataOhlcv);

            List<int> count = new List<int>();

            foreach (var methodName in patternNames)
            {
                count.Add(_patterns.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetPatternsSignalsCount(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetSignalsCount(patternName);
        }

        public decimal GetPatternsSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight)
        {
            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetSignalsCount(patternName) * weight;
        }

        public decimal GetPatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights)
        {
            _patterns = new Patterns(dataOhlcv);

            List<decimal> count = new List<decimal>();

            foreach (var methodName in patternNamesWithWeights.Keys)
            {
                count.Add(_patterns.GetSignalsCount(methodName) * patternNamesWithWeights[methodName]);
            }

            return count.Sum(x => x);
        }

        public int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _formations = new Formations(dataOhlcv);
            return _formations.GetFormationsSignalsCount(patternName);
        }
    }
}
