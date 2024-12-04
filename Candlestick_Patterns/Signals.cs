namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;

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

            return _patterns.GetPatternsSignalsList(patternName);
        }

        public List<List<OhlcvObject>> GetMultiplePatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            _patterns = new Patterns(dataOhlcv);

            List<List<OhlcvObject>> list = new List<List<OhlcvObject>>();

            foreach (var methodName in patternNames)
            {
                list.Add(_patterns.GetPatternsSignalsList(methodName));
            }

            return list;
        }

        public int GetMultiplePatternsSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames)
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

        public decimal GetPatternSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight)
        {
            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetSignalsCount(patternName) * weight;
        }

        public decimal GetMultiplePatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights)
        {
            _patterns = new Patterns(dataOhlcv);

            List<decimal> count = new List<decimal>();

            foreach (var methodName in patternNamesWithWeights.Keys)
            {
                count.Add(_patterns.GetSignalsCount(methodName) * patternNamesWithWeights[methodName]);
            }

            return count.Sum(x => x);
        }

        public int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _formations = new Formations(dataOhlcv);
            return _formations.GetFormationsSignalsCount(formationName);
        }

        public int GetFormationsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _formations = new Formations(dataOhlcv);

            var bullishMethodNames = _formations.GetAllMethodNames().Where(x => x.StartsWith("Bullish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_formations.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetFormationsBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _formations = new Formations(dataOhlcv);

            var bullishMethodNames = _formations.GetAllMethodNames().Where(x => x.StartsWith("Bearish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_formations.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetMultipleFormationsSignalsCount(List<OhlcvObject> dataOhlcv, string[] formationNames)
        {
            _formations = new Formations(dataOhlcv);

            List<int> count = new List<int>();

            foreach (var methodName in formationNames)
            {
                count.Add(_formations.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public decimal GetFormationSignalsIndex(List<OhlcvObject> dataOhlcv, string formationName, decimal weight)
        {
            _formations = new Formations(dataOhlcv);

            return _formations.GetSignalsCount(formationName) * weight;
        }

        public decimal GetMultipleFormationsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> formationsNamesWithWeights)
        {
            _formations = new Formations(dataOhlcv);

            List<decimal> count = new List<decimal>();

            foreach (var methodName in formationsNamesWithWeights.Keys)
            {
                count.Add(_formations.GetSignalsCount(methodName) * formationsNamesWithWeights[methodName]);
            }

            return count.Sum(x => x);
        }

        public List<ZigZagObject> GetFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _formations = new Formations(dataOhlcv);

            return _formations.GetFormationsSignalsList(formationName);
        }

        public List<List<ZigZagObject>> GetMultipleFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] formationsNames)
        {
            _formations = new Formations(dataOhlcv);

            List<List<ZigZagObject>> list = new List<List<ZigZagObject>>();

            foreach (var methodName in formationsNames)
            {
                list.Add(_formations.GetFormationsSignalsList(methodName));
            }

            return list;
        }

        public int GetFibonacciSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            return _fibonacci.GetFibonacciSignalsCount(formationName);
        }

        public int GetFiboBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            var bullishMethodNames = _fibonacci.GetAllMethodNames().Where(x => x.StartsWith("Bullish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_fibonacci.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetFiboBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            var bullishMethodNames = _fibonacci.GetAllMethodNames().Where(x => x.StartsWith("Bearish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_fibonacci.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetMultipleFiboSignalsCount(List<OhlcvObject> dataOhlcv, string[] fiboNames)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            List<int> count = new List<int>();

            foreach (var methodName in fiboNames)
            {
                count.Add(_fibonacci.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetFiboSignalsCount(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            return _fibonacci.GetFibonacciSignalsCount(fiboName);
        }

        public decimal GetFiboSignalsIndex(List<OhlcvObject> dataOhlcv, string fiboName, decimal weight)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            return _fibonacci.GetSignalsCount(fiboName) * weight;
        }

        public decimal GetMultipleFiboSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> fibosNamesWithWeights)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            List<decimal> count = new List<decimal>();

            foreach (var methodName in fibosNamesWithWeights.Keys)
            {
                count.Add(_fibonacci.GetSignalsCount(methodName) * fibosNamesWithWeights[methodName]);
            }

            return count.Sum(x => x);
        }

        public List<ZigZagObject> GetFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            return _fibonacci.GetFibonacciSignalsList(fiboName);
        }

        public List<List<ZigZagObject>> GetMultipleFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] fiboNames)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            List<List<ZigZagObject>> list = new List<List<ZigZagObject>>();

            foreach (var methodName in fiboNames)
            {
                list.Add(_fibonacci.GetFibonacciSignalsList(methodName));
            }

            return list;
        }
    }
}
