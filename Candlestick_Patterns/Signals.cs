namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;
        List<OhlcvObject> _lastData;

        private void EnsureInstances(List<OhlcvObject> dataOhlcv)
        {
            if (ReferenceEquals(_lastData, dataOhlcv)) return;

            _lastData = dataOhlcv;
            _patterns = new Patterns(dataOhlcv);
            _formations = new Formations(dataOhlcv);
            _fibonacci = new Fibonacci(dataOhlcv);
        }

        private int GetDirectionalCount(ISignalEngine engine, SignalDirection direction)
        {
            var prefix = direction.ToString();
            return engine.GetAllMethodNames().Where(n => n.StartsWith(prefix)).Sum(engine.GetSignalsCount);
        }

        private int GetSingleCount(ISignalEngine engine, string name)
        {
            return engine.GetSignalsCount(name);
        }

        private int GetMultipleCount(ISignalEngine engine, string[] names)
        {
            return names.Sum(engine.GetSignalsCount);
        }

        private decimal GetSingleIndex(ISignalEngine engine, string name, decimal weight)
        {
            return engine.GetSignalsCount(name) * weight;
        }

        private static List<T> CollectLists<T>(string[] names, Func<string, T> getList)
        {
            return names.Select(getList).ToList();
        }

        private static int SumCounts(IEnumerable<string> names, Func<string, int> getCount)
        {
            return names.Sum(getCount);
        }
        private decimal GetMultipleIndex(ISignalEngine engine, Dictionary<string, decimal> namesWithWeights)
        {
            return namesWithWeights.Sum(kv => engine.GetSignalsCount(kv.Key) * kv.Value);
        }

        private static decimal WeightedIndex(Dictionary<string, decimal> namesWithWeights, Func<string, int> getCount)
        {
            return namesWithWeights.Sum(kv => getCount(kv.Key) * kv.Value);
        }

        private T GetSingleList<T>(Func<string, T> listFetcher, string name)
        {
            return listFetcher(name);
        }

        private List<T> GetMultipleLists<T>(Func<string, T> listFetcher, string[] names)
        {
            return names.Select(listFetcher).ToList();
        }


        public int GetPatternsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            EnsureInstances(dataOhlcv);
            return GetDirectionalCount(_patterns, SignalDirection.Bearish);

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
            EnsureInstances(dataOhlcv); 
            return GetDirectionalCount(_patterns, SignalDirection.Bullish);

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
            EnsureInstances(dataOhlcv);
            return GetSingleList(_patterns.GetPatternsSignalsList, patternName);
            
            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetPatternsSignalsList(patternName);
            
        }

        public List<List<OhlcvObject>> GetMultiplePatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleLists(_patterns.GetPatternsSignalsList, patternNames);


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
            EnsureInstances(dataOhlcv);
            return GetMultipleCount(_patterns, patternNames);

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
            EnsureInstances(dataOhlcv);
            return GetSingleCount(_patterns, patternName);

            _patterns = new Patterns(dataOhlcv);

            return _patterns.GetSignalsCount(patternName);
        }

        public decimal GetPatternSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight)
        {
            //_patterns = new Patterns(dataOhlcv);
            EnsureInstances(dataOhlcv);
            return GetSingleIndex(_patterns, patternName, weight);
        }

        public decimal GetMultiplePatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleIndex(_patterns, patternNamesWithWeights);
            
            _patterns = new Patterns(dataOhlcv);

            List<decimal> count = new List<decimal>();

            foreach (var methodName in patternNamesWithWeights.Keys)
            {
                count.Add(_patterns.GetSignalsCount(methodName) * patternNamesWithWeights[methodName]);
            }

            return count.Sum(x => x);
            
        }

        /// <summary>
        ///  fromations
        /// </summary>
        /// <param name="dataOhlcv"></param>
        /// <param name="formationName"></param>
        /// <returns></returns>
        public int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            EnsureInstances(dataOhlcv);
            return GetSingleCount(_formations, formationName);

            _formations = new Formations(dataOhlcv);
            return _formations.GetFormationsSignalsCount(formationName);
        }

        public int GetFormationsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            EnsureInstances(dataOhlcv);
            return GetDirectionalCount(_formations, SignalDirection.Bearish);

            _formations = new Formations(dataOhlcv);

            var bearishMethodNames = _formations.GetAllMethodNames().Where(x => x.StartsWith("Bearish")).ToList(); 

            List<int> count = new List<int>();

            foreach (var methodName in bearishMethodNames)
            {
                count.Add(_formations.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetFormationsBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            EnsureInstances(dataOhlcv);
            return GetDirectionalCount(_formations, SignalDirection.Bullish);

            _formations = new Formations(dataOhlcv);

            var bullishMethodNames = _formations.GetAllMethodNames().Where(x => x.StartsWith("Bullish")).ToList();

            List<int> count = new List<int>();

            foreach (var methodName in bullishMethodNames)
            {
                count.Add(_formations.GetSignalsCount(methodName));
            }

            return count.Sum(x => x);
        }

        public int GetMultipleFormationsSignalsCount(List<OhlcvObject> dataOhlcv, string[] formationNames)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleCount(_formations, formationNames);

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
            EnsureInstances(dataOhlcv);
            return GetSingleIndex(_formations, formationName, weight);

            _formations = new Formations(dataOhlcv);

            return _formations.GetSignalsCount(formationName) * weight;
        }

        public decimal GetMultipleFormationsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> formationsNamesWithWeights)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleIndex(_formations, formationsNamesWithWeights);

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
            EnsureInstances(dataOhlcv);
            return GetSingleList(_formations.GetFormationsSignalsList, formationName);


            _formations = new Formations(dataOhlcv);

            return _formations.GetFormationsSignalsList(formationName);
        }

        public List<List<ZigZagObject>> GetMultipleFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] formationsNames)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleLists(_formations.GetFormationsSignalsList, formationsNames);

            _formations = new Formations(dataOhlcv);

            List<List<ZigZagObject>> list = new List<List<ZigZagObject>>();

            foreach (var methodName in formationsNames)
            {
                list.Add(_formations.GetFormationsSignalsList(methodName));
            }

            return list;
        }
        /// <summary>
        /// Fibonacci
        /// </summary>
        /// <param name="dataOhlcv"></param>
        /// <param name="fibonacciName"></param>
        /// <returns></returns>

        public int GetFibonacciSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            return _fibonacci.GetFibonacciSignalsCount(formationName);
        }

        public int GetFiboBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            EnsureInstances(dataOhlcv);
            return GetDirectionalCount(_fibonacci, SignalDirection.Bullish);

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
            EnsureInstances(dataOhlcv);
            return GetDirectionalCount(_fibonacci, SignalDirection.Bearish);

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
            EnsureInstances(dataOhlcv);
            return GetMultipleCount(_fibonacci, fiboNames);

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
            return GetFibonacciSignalsCount(dataOhlcv, fiboName);

            _fibonacci = new Fibonacci(dataOhlcv);
            return _fibonacci.GetFibonacciSignalsCount(fiboName);
        }

        public decimal GetFiboSignalsIndex(List<OhlcvObject> dataOhlcv, string fiboName, decimal weight)
        {
            EnsureInstances(dataOhlcv);
            return GetSingleIndex(_fibonacci, fiboName, weight);

            _fibonacci = new Fibonacci(dataOhlcv);

            return _fibonacci.GetSignalsCount(fiboName) * weight;
        }

        public decimal GetMultipleFiboSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> fibosNamesWithWeights)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleIndex(_fibonacci, fibosNamesWithWeights);

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
            EnsureInstances(dataOhlcv);
            return GetSingleList(_fibonacci.GetFibonacciSignalsList, fiboName);

            _fibonacci = new Fibonacci(dataOhlcv);

            return _fibonacci.GetFibonacciSignalsList(fiboName);
        }

        public List<List<ZigZagObject>> GetMultipleFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] fiboNames)
        {
            EnsureInstances(dataOhlcv);
            return GetMultipleLists(_fibonacci.GetFibonacciSignalsList, fiboNames);

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
