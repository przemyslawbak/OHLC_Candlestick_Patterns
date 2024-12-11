using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public class AccuracyTrials : IAccuracyTrials
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;

        public AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResults(signalsList, patternName);
        }

        public AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName, int candlesAheadQty)
        {
            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResultsForNCandlesAhead(signalsList, patternName, candlesAheadQty);
        }

        public AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(fiboName);

            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResults(signalsListOhlcv, fiboName);
        }

        public AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string formation, int candlesAheadQty)
        {
            _fibonacci = new Fibonacci(dataOhlcv);

            var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(formation);

            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResultsForNCandlesAhead(signalsListOhlcv, formation, candlesAheadQty);
        }

        public AccuracyObject GetAverPercentFormationAccuracy(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _formations = new Formations(dataOhlcv);

            var signalsListZigZag = _formations.GetFormationsSignalsList(formationName);

            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResults(signalsListOhlcv, formationName);
        }

        private List<OhlcvObject> ConvertZigZagToOhlcv(List<ZigZagObject> signalsListZigZag, List<OhlcvObject> dataOhlcv)
        {
            foreach (var zigZagItem in signalsListZigZag)
            {
                dataOhlcv[zigZagItem.IndexOHLCV].Signal = zigZagItem.Signal;
            }

            return dataOhlcv;
        }

        //todo: DRY
        private AccuracyObject GetAccuracyResultsForNCandlesAhead(List<OhlcvObject> signalsList, string name, int candlesAheadQty)
        {
            var multiplier = name.Contains("Bullish") ? 1M : name.Contains("Bearish") ? -1M : 0M;

            if (signalsList.Where(x => x.Signal == true).Count() == 0 || multiplier == 0M)
            {
                return new AccuracyObject()
                {
                    AccuracyToAverageClose = 0,
                    AccuracyToEndClose = 0,
                };
            }

            List<decimal> accuracyEndResuts = new List<decimal>();
            List<decimal> accuracyAverResuts = new List<decimal>();

            for (int i = 0; i < signalsList.Count; i++)
            {
                var lastIndex = signalsList.Count() - 1;
                var lastClose = signalsList.Last().Close;

                if (i + candlesAheadQty < lastIndex)
                {
                    lastClose = signalsList[i + candlesAheadQty].Close;
                    lastIndex = i + candlesAheadQty;
                }

                if (signalsList[i].Signal)
                {
                    accuracyEndResuts.Add((lastClose - signalsList[i].Close) * multiplier / signalsList[i].Close * 100);

                    var averCloseAfterSignal = signalsList.Skip(i).Take(candlesAheadQty).Average(x => x.Close);

                    if (signalsList[lastIndex].Close != 0)
                    {
                        accuracyAverResuts.Add((averCloseAfterSignal - signalsList[i].Close) * multiplier / signalsList[i].Close * 100);
                    }
                }
            }

            return new AccuracyObject()
            {
                AccuracyToAverageClose = accuracyAverResuts.Average(),
                AccuracyToEndClose = accuracyEndResuts.Average(),
            };
        }

        //todo: DRY
        private AccuracyObject GetAccuracyResults(List<OhlcvObject> signalsList, string name)
        {
            var multiplier = name.Contains("Bullish") ? 1M : name.Contains("Bearish") ? -1M : 0M;

            if (signalsList.Where(x => x.Signal == true).Count() == 0 || multiplier == 0M)
            {
                return new AccuracyObject()
                {
                    AccuracyToAverageClose = 0,
                    AccuracyToEndClose = 0,
                };
            }

            List<decimal> accuracyEndResuts = new List<decimal>();
            List<decimal> accuracyAverResuts = new List<decimal>();

            for (int i = 0; i < signalsList.Count; i++)
            {
                var lastClose = signalsList.Last().Close;

                if (signalsList[i].Signal)
                {
                    accuracyEndResuts.Add((lastClose - signalsList[i].Close) * multiplier / signalsList[i].Close * 100);

                    var averCloseAfterSignal = signalsList.Skip(i).Average(x => x.Close);

                    if (signalsList[i].Close != 0)
                    {
                        accuracyAverResuts.Add((averCloseAfterSignal - signalsList[i].Close) * multiplier / signalsList[i].Close * 100);
                    }
                }
            }

            return new AccuracyObject()
            {
                AccuracyToAverageClose = accuracyAverResuts.Average(),
                AccuracyToEndClose = accuracyEndResuts.Average(),
            };
        }

        public string[] GetPositiveAccuracyToAverPatterns(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);
            var allPatterns = _patterns.GetAllMethodNames();

            var res = new List<string>();

            foreach (var pattern in allPatterns)
            {
                var acc = GetAverPercentPatternAccuracy(dataOhlcv, pattern);

                if (acc.AccuracyToAverageClose > 0)
                {
                    res.Add(pattern);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToAverFibos(List<OhlcvObject> dataOhlcv)
        {
            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();

            var res = new List<string>();

            foreach (var formation in allFormations)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, formation);

                if (acc.AccuracyToAverageClose > 0)
                {
                    res.Add(formation);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToAverFibo(List<OhlcvObject> dataOhlcv)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var allFibo = _fibonacci.GetAllMethodNames();

            var res = new List<string>();

            foreach (var fibo in allFibo)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, fibo);

                if (acc.AccuracyToAverageClose > 0)
                {
                    res.Add(fibo);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToEndPatterns(List<OhlcvObject> dataOhlcv)
        {
            _patterns = new Patterns(dataOhlcv);
            var allPatterns = _patterns.GetAllMethodNames();

            var res = new List<string>();

            foreach (var pattern in allPatterns)
            {
                var acc = GetAverPercentPatternAccuracy(dataOhlcv, pattern);

                if (acc.AccuracyToEndClose > 0)
                {
                    res.Add(pattern);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToEndFormations(List<OhlcvObject> dataOhlcv)
        {
            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();

            var res = new List<string>();

            foreach (var formation in allFormations)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, formation);

                if (acc.AccuracyToEndClose > 0)
                {
                    res.Add(formation);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToEndFibos(List<OhlcvObject> dataOhlcv)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var allFibo = _fibonacci.GetAllMethodNames();

            var res = new List<string>();

            foreach (var fibo in allFibo)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, fibo);

                if (acc.AccuracyToEndClose > 0)
                {
                    res.Add(fibo);
                }
            }

            return res.ToArray();
        }

        public string[] GetBestAccuracyPatterns(List<OhlcvObject> dataOhlcv, int topPercentage)
        {
            _patterns = new Patterns(dataOhlcv);
            var allPatterns = _patterns.GetAllMethodNames();
            int qty = allPatterns.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var pattern in allPatterns)
            {
                var acc = GetAverPercentPatternAccuracy(dataOhlcv, pattern);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(pattern, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        public string[] GetBestAccuracyFormations(List<OhlcvObject> dataOhlcv, int topPercentage)
        {
            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();
            int qty = allFormations.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var formation in allFormations)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, formation);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(formation, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        public string[] GetBestAccuracyFibos(List<OhlcvObject> dataOhlcv, int topPercentage)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var allFibo = _fibonacci.GetAllMethodNames();
            int qty = allFibo.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var fibo in allFibo)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, fibo);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(fibo, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        public string[] GetBestAccuracyPatterns(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead)
        {
            _patterns = new Patterns(dataOhlcv);
            var allPatterns = _patterns.GetAllMethodNames();
            int qty = allPatterns.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var pattern in allPatterns)
            {
                var acc = GetAverPercentPatternAccuracy(dataOhlcv, pattern, candlesAhead);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(pattern, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        public string[] GetBestAccuracyFormations(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead)
        {
            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();
            int qty = allFormations.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var formation in allFormations)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, formation, candlesAhead);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(formation, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        public string[] GetBestAccuracyFibos(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var allFibo = _fibonacci.GetAllMethodNames();
            int qty = allFibo.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var fibo in allFibo)
            {
                var acc = GetAverPercentAccuracy(dataOhlcv, fibo, candlesAhead);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values.Add(fibo, (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) / 2);
                }
            }

            if (values.Count == 0)
            {
                return new string[0];
            }

            if (values.Count < qty)
            {
                qty = values.Count;
            }

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }
    }
}
