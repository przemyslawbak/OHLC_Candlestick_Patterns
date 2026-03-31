namespace Candlestick_Patterns
{
    public class Signals : ISignals
    {
        private class AnalysisSnapshot
        {
            public List<OhlcvObject> Data { get; }
            public IFormations Formations { get; }
            public IPatterns Patterns { get; }
            public IFibonacci Fibonacci { get; }

            public AnalysisSnapshot(List<OhlcvObject> data)
            {
                Data = data;
                Formations = new Formations(data);
                Patterns = new Patterns(data);
                Fibonacci = new Fibonacci(data);
            }
        }

        private volatile AnalysisSnapshot _currentSnapshot;
        private readonly object _lock = new object();

        IFormations _formations;
        IPatterns _patterns;
        IFibonacci _fibonacci;
        List<OhlcvObject> _lastData;

        private AnalysisSnapshot EnsureSnapshot(List<OhlcvObject> dataOhlcv)
        {
            var current = _currentSnapshot;
            if (current != null && ReferenceEquals(current.Data, dataOhlcv))
                return current;

            lock (_lock)
            {
                current = _currentSnapshot;
                if (current != null && ReferenceEquals(current.Data, dataOhlcv))
                    return current;
                var newSnapshot = new AnalysisSnapshot(dataOhlcv);
                _currentSnapshot = newSnapshot;
                return newSnapshot;
            }
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

        private decimal GetMultipleIndex(ISignalEngine engine, Dictionary<string, decimal> namesWithWeights)
        {
            return namesWithWeights.Sum(kv => engine.GetSignalsCount(kv.Key) * kv.Value);
        }

        private T GetSingleList<T>(Func<string, T> listFetcher, string name)
        {
            return listFetcher(name);
        }

        private List<List<T>> GetMultipleLists<T>(Func<string, List<T>> listFetcher, string[] names)
        {
            var results = new List<List<T>>(names.Length);

            foreach (var name in names)
            {
                var list = listFetcher(name);
                results.Add(list != null ? new List<T>(list) : new List<T>());
            }

            return results;
        }

        public int GetPatternsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Patterns, SignalDirection.Bearish);
        }

        public int GetPatternsBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Patterns, SignalDirection.Bullish);
        }

        public List<OhlcvObject> GetPatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string patternName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleList(snapshot.Patterns.GetPatternsSignalsList, patternName);

        }

        public List<List<OhlcvObject>> GetMultiplePatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleLists(snapshot.Patterns.GetPatternsSignalsList, patternNames);
        }

        public int GetMultiplePatternsSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleCount(snapshot.Patterns, patternNames);
        }

        public int GetPatternsSignalsCount(List<OhlcvObject> dataOhlcv, string patternName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleCount(snapshot.Patterns, patternName);
        }

        public decimal GetPatternSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleIndex(snapshot.Patterns, patternName, weight);
        }

        public decimal GetMultiplePatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleIndex(snapshot.Patterns, patternNamesWithWeights);

        }

        /// <summary>
        ///  fromations
        /// </summary>
        /// <param name="dataOhlcv"></param>
        /// <param name="formationName"></param>
        /// <returns></returns>
        /// 

        public int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleCount(snapshot.Formations, formationName);
        }

        public int GetFormationsBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Formations, SignalDirection.Bearish);
        }

        public int GetFormationsBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Formations, SignalDirection.Bullish);
        }

        public int GetMultipleFormationsSignalsCount(List<OhlcvObject> dataOhlcv, string[] formationNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleCount(snapshot.Formations, formationNames);
        }

        public decimal GetFormationSignalsIndex(List<OhlcvObject> dataOhlcv, string formationName, decimal weight)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleIndex(snapshot.Formations, formationName, weight);
        }

        public decimal GetMultipleFormationsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> formationsNamesWithWeights)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleIndex(snapshot.Formations, formationsNamesWithWeights);
        }

        public List<ZigZagObject> GetFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string formationName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleList(snapshot.Formations.GetFormationsSignalsList, formationName);
        }

        public List<List<ZigZagObject>> GetMultipleFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] formationsNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleLists(snapshot.Formations.GetFormationsSignalsList, formationsNames);
        }

        /// <summary>
        /// Fibonacci
        /// </summary>
        /// <param name="dataOhlcv"></param>
        /// <param name="fibonacciName"></param>
        /// <returns></returns>

        public int GetFibonacciSignalsCount(List<OhlcvObject> dataOhlcv, string formationName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleCount(snapshot.Fibonacci, formationName);
        }

        public int GetFiboBullishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Fibonacci, SignalDirection.Bullish);
        }

        public int GetFiboBearishSignalsCount(List<OhlcvObject> dataOhlcv)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetDirectionalCount(snapshot.Fibonacci, SignalDirection.Bearish);
        }

        public int GetMultipleFiboSignalsCount(List<OhlcvObject> dataOhlcv, string[] fiboNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleCount(snapshot.Fibonacci, fiboNames);
        }

        public int GetFiboSignalsCount(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            return GetFibonacciSignalsCount(dataOhlcv, fiboName);
        }

        public decimal GetFiboSignalsIndex(List<OhlcvObject> dataOhlcv, string fiboName, decimal weight)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleIndex(snapshot.Fibonacci, fiboName, weight);
        }

        public decimal GetMultipleFiboSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> fibosNamesWithWeights)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleIndex(snapshot.Fibonacci, fibosNamesWithWeights);
        }

        public List<ZigZagObject> GetFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetSingleList(snapshot.Fibonacci.GetFibonacciSignalsList, fiboName);
        }

        public List<List<ZigZagObject>> GetMultipleFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] fiboNames)
        {
            var snapshot = EnsureSnapshot(dataOhlcv);

            return GetMultipleLists(snapshot.Fibonacci.GetFibonacciSignalsList, fiboNames);
        }
    }
}
