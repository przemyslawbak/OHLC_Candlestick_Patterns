using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public class AccuracyTrials : IAccuracyTrials
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;

        private List<OhlcvObject> _dataOhlcv;

        private List<string> _cachedPatternNames;
        private List<string> _cachedFormationNames;
        private List<string> _cachedFibonacciNames;

        private enum AnalysisType
        {
            Pattern,
            Formation,
            Fibonacci
        }

        private void InitializePatterns(List<OhlcvObject> dataOhlcv)
        {
            if (_patterns == null || _dataOhlcv != dataOhlcv)
            {
                _patterns = new Patterns(dataOhlcv);
                _dataOhlcv = dataOhlcv;
                _cachedPatternNames = null;
            }
        }

        private void InitializeFormations(List<OhlcvObject> dataOhlcv)
        {
            if (_formations == null || _dataOhlcv != dataOhlcv)
            {
                _formations = new Formations(dataOhlcv);
                _dataOhlcv = dataOhlcv;
                _cachedFormationNames = null;
            }
        }

        private void InitializeFibonacci(List<OhlcvObject> dataOhlcv)
        {
            if (_fibonacci == null || _dataOhlcv != dataOhlcv)
            {
                _fibonacci = new Fibonacci(dataOhlcv);
                _dataOhlcv = dataOhlcv;
                _cachedFibonacciNames = null;
            }
        }

        public AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName)
        {
            InitializePatterns(dataOhlcv);
            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResults(signalsList, patternName);

            //_patterns = new Patterns(dataOhlcv);

            //var signalsList = _patterns.GetPatternsSignalsList(patternName);

            //return GetAccuracyResults(signalsList, patternName);
        }

        public AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName, int candlesAheadQty)
        {
            InitializePatterns(dataOhlcv);
            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResultsForNCandlesAhead(signalsList, patternName, candlesAheadQty);

            //_patterns = new Patterns(dataOhlcv);

            //var signalsList = _patterns.GetPatternsSignalsList(patternName);

            //return GetAccuracyResultsForNCandlesAhead(signalsList, patternName, candlesAheadQty);
        }

        public AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string fiboName)
        {
            InitializeFibonacci(dataOhlcv);
            var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(fiboName);
            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResults(signalsListOhlcv, fiboName);

            /*_fibonacci = new Fibonacci(dataOhlcv);

            var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(fiboName);

            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResults(signalsListOhlcv, fiboName);*/
        }

        public AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string formation, int candlesAheadQty)
        {
            InitializeFibonacci(dataOhlcv);
            var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(formation);
            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResultsForNCandlesAhead(signalsListOhlcv, formation, candlesAheadQty);

            //_fibonacci = new Fibonacci(dataOhlcv);

            //var signalsListZigZag = _fibonacci.GetFibonacciSignalsList(formation);

            //var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            //return GetAccuracyResultsForNCandlesAhead(signalsListOhlcv, formation, candlesAheadQty);
        }

        public AccuracyObject GetAverPercentFormationAccuracy(List<OhlcvObject> dataOhlcv, string formationName)
        {
            InitializeFormations(dataOhlcv);
            var signalsListZigZag = _formations.GetFormationsSignalsList(formationName);
            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResults(signalsListOhlcv, formationName);

            _formations = new Formations(dataOhlcv);
            //var signalsListZigZag = _formations.GetFormationsSignalsList(formationName);
            //var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            //return GetAccuracyResults(signalsListOhlcv, formationName);
        }

        public AccuracyObject GetAverPercentFormationAccuracy(List<OhlcvObject> dataOhlcv, string formationName, int candlesAheadQty)
        {
            InitializeFormations(dataOhlcv);
            var signalsListZigZag = _formations.GetFormationsSignalsList(formationName);
            var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            return GetAccuracyResultsForNCandlesAhead(signalsListOhlcv, formationName, candlesAheadQty);

            //_formations = new Formations(dataOhlcv);

            //var signalsListZigZag = _formations.GetFormationsSignalsList(formationName);

            //var signalsListOhlcv = ConvertZigZagToOhlcv(signalsListZigZag, dataOhlcv);

            //return GetAccuracyResultsForNCandlesAhead(signalsListOhlcv, formationName, candlesAheadQty);
        }

        private List<OhlcvObject> ConvertZigZagToOhlcv(List<ZigZagObject> signalsListZigZag, List<OhlcvObject> dataOhlcv)
        {
            var result = dataOhlcv.Select(x => new OhlcvObject
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
                Signal = false
            }).ToList();

            foreach (var zigZagItem in signalsListZigZag)
            {
                result[zigZagItem.IndexOHLCV].Signal = zigZagItem.Signal;
            }

            return result;

            foreach (var zigZagItem in signalsListZigZag)
            {
                dataOhlcv[zigZagItem.IndexOHLCV].Signal = zigZagItem.Signal;
            }

            return dataOhlcv;
        }

        //todo: DRY
        private AccuracyObject GetAccuracyResultsForNCandlesAhead(List<OhlcvObject> signalsList, string name, int candlesAheadQty)
        {
            var multiplier = GetMultiplier(name);

            // Early exit if no valid signals
            var signalCount = 0;
            for (int i = 0; i < signalsList.Count; i++)
            {
                if (signalsList[i].Signal)
                {
                    signalCount++;
                }
            }

            if (signalCount == 0 || multiplier == 0M)
            {
                return new AccuracyObject
                {
                    AccuracyToAverageClose = 0,
                    AccuracyToEndClose = 0,
                };
            }

            // Pre-allocate lists
            var accuracyEndResults = new List<decimal>(signalCount);
            var accuracyAverResults = new List<decimal>(signalCount);

            var lastIndex = signalsList.Count - 1;
            var lastClose = signalsList[lastIndex].Close;

            // Pre-calculate cumulative sums for windowed averages
            var count = signalsList.Count;
            var cumulativeSums = new decimal[count + 1];
            for (int i = 0; i < count; i++)
            {
                cumulativeSums[i + 1] = cumulativeSums[i] + signalsList[i].Close;
            }

            for (int i = 0; i < count; i++)
            {
                if (!signalsList[i].Signal) continue;

                var currentClose = signalsList[i].Close;
                if (currentClose == 0) continue;

                var endIndex = Math.Min(i + candlesAheadQty, lastIndex);
                var effectiveLastClose = signalsList[endIndex].Close;

                // Calculate end accuracy
                accuracyEndResults.Add((effectiveLastClose - currentClose) * multiplier / currentClose * 100);

                // Calculate average accuracy using cumulative sums
                var windowSize = Math.Min(candlesAheadQty, count - i);
                var windowSum = cumulativeSums[i + windowSize] - cumulativeSums[i];
                var averCloseAfterSignal = windowSum / windowSize;

                if (effectiveLastClose != 0)
                {
                    accuracyAverResults.Add((averCloseAfterSignal - currentClose) * multiplier / currentClose * 100);
                }
            }

            return new AccuracyObject
            {
                AccuracyToAverageClose = accuracyAverResults.Count > 0 ? accuracyAverResults.Average() : 0,
                AccuracyToEndClose = accuracyEndResults.Count > 0 ? accuracyEndResults.Average() : 0,
            };


            /*var multiplier = name.Contains("Bullish") ? 1M : name.Contains("Bearish") ? -1M : 0M;

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
            };*/
        }

        private decimal GetMultiplier(string name)
        {
            if (name.Contains("Bullish")) return 1M;
            if (name.Contains("Bearish")) return -1M;
            return 0M;
        }

        private string[] GetFilteredItems(List<OhlcvObject> dataOhlcv, AnalysisType type, Func<AccuracyObject, bool> filter)
        {
            var allNames = GetAllMethodNames(dataOhlcv, type);
            var result = new List<string>(allNames.Length / 2); // Estimate half will pass

            foreach (var name in allNames)
            {
                var acc = GetAccuracyForType(dataOhlcv, name, type);
                if (filter(acc))
                {
                    result.Add(name);
                }
            }

            return result.ToArray();
        }

        private AccuracyObject GetAccuracyResults(List<OhlcvObject> signalsList, string name)
        {
            var multiplier = GetMultiplier(name);

            // Early exit if no valid signals
            var signalCount = 0;
            for (int i = 0; i < signalsList.Count; i++)
            {
                if (signalsList[i].Signal)
                {
                    signalCount++;
                }
            }

            if (signalCount == 0 || multiplier == 0M)
            {
                return new AccuracyObject
                {
                    AccuracyToAverageClose = 0,
                    AccuracyToEndClose = 0,
                };
            }

            // Pre-allocate lists with known capacity
            var accuracyEndResults = new List<decimal>(signalCount);
            var accuracyAverResults = new List<decimal>(signalCount);

            var lastClose = signalsList[signalsList.Count - 1].Close;
            var count = signalsList.Count;

            // Pre-calculate cumulative sums for average calculation
            var cumulativeSums = new decimal[count + 1];
            for (int i = 0; i < count; i++)
            {
                cumulativeSums[i + 1] = cumulativeSums[i] + signalsList[i].Close;
            }

            for (int i = 0; i < count; i++)
            {
                if (!signalsList[i].Signal) continue;

                var currentClose = signalsList[i].Close;
                if (currentClose == 0) continue;

                // Calculate end accuracy
                accuracyEndResults.Add((lastClose - currentClose) * multiplier / currentClose * 100);

                // Calculate average accuracy using cumulative sums
                var remainingCount = count - i;
                var averCloseAfterSignal = (cumulativeSums[count] - cumulativeSums[i]) / remainingCount;
                accuracyAverResults.Add((averCloseAfterSignal - currentClose) * multiplier / currentClose * 100);
            }

            return new AccuracyObject
            {
                AccuracyToAverageClose = accuracyAverResults.Count > 0 ? accuracyAverResults.Average() : 0,
                AccuracyToEndClose = accuracyEndResults.Count > 0 ? accuracyEndResults.Average() : 0,
            };



            /*
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
            };*/
        }

        public string[] GetPositiveAccuracyToAverPatterns(List<OhlcvObject> dataOhlcv)
        {
            return GetFilteredItems(dataOhlcv, AnalysisType.Pattern, acc => acc.AccuracyToAverageClose > 0);

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

        public string[] GetPositiveAccuracyToAverFibos(List<OhlcvObject> dataOhlcv) // formations ???
        {
            return GetFilteredItems(dataOhlcv, AnalysisType.Formation, acc => acc.AccuracyToAverageClose > 0); // ???

            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();

            var res = new List<string>();

            foreach (var formation in allFormations)
            {
                //var acc = GetAverPercentAccuracy(dataOhlcv, formation);
                var acc = GetAverPercentFormationAccuracy(dataOhlcv, formation);

                if (acc.AccuracyToAverageClose > 0)
                {
                    res.Add(formation);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToAverFibo(List<OhlcvObject> dataOhlcv)
        {
            return GetFilteredItems(dataOhlcv, AnalysisType.Fibonacci, acc => acc.AccuracyToAverageClose > 0);

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
            return GetFilteredItems(dataOhlcv, AnalysisType.Pattern, acc => acc.AccuracyToEndClose > 0);

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
            return GetFilteredItems(dataOhlcv, AnalysisType.Formation, acc => acc.AccuracyToEndClose > 0);

            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();

            var res = new List<string>();

            foreach (var formation in allFormations)
            {
                //var acc = GetAverPercentAccuracy(dataOhlcv, formation);
                var acc = GetAverPercentFormationAccuracy(dataOhlcv, formation);

                if (acc.AccuracyToEndClose > 0)
                {
                    res.Add(formation);
                }
            }

            return res.ToArray();
        }

        public string[] GetPositiveAccuracyToEndFibos(List<OhlcvObject> dataOhlcv)
        {
            return GetFilteredItems(dataOhlcv, AnalysisType.Fibonacci, acc => acc.AccuracyToEndClose > 0);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Pattern);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Formation);

            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();
            int qty = allFormations.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var formation in allFormations)
            {
                //var acc = GetAverPercentAccuracy(dataOhlcv, formation);
                var acc = GetAverPercentFormationAccuracy(dataOhlcv, formation);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Fibonacci);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Pattern, candlesAhead);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Formation, candlesAhead);

            _formations = new Formations(dataOhlcv);
            var allFormations = _formations.GetAllMethodNames();
            int qty = allFormations.Count * topPercentage / 100;

            var values = new Dictionary<string, decimal>();

            foreach (var formation in allFormations)
            {
                //var acc = GetAverPercentAccuracy(dataOhlcv, formation, candlesAhead);
                var acc = GetAverPercentFormationAccuracy(dataOhlcv, formation, candlesAhead);

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
            return GetBestAccuracyItems(dataOhlcv, topPercentage, AnalysisType.Fibonacci, candlesAhead);

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

        private string[] GetBestAccuracyItems(List<OhlcvObject> dataOhlcv, int topPercentage, AnalysisType type, int? candlesAhead = null)
        {
            var allNames = GetAllMethodNames(dataOhlcv, type);
            int qty = Math.Max(1, allNames.Length * topPercentage / 100);

            var values = new Dictionary<string, decimal>(allNames.Length);

            foreach (var name in allNames)
            {
                var acc = candlesAhead.HasValue
                    ? GetAccuracyForType(dataOhlcv, name, type, candlesAhead.Value)
                    : GetAccuracyForType(dataOhlcv, name, type);

                if (acc.AccuracyToEndClose > 0 && acc.AccuracyToAverageClose > 0)
                {
                    values[name] = (acc.AccuracyToEndClose + acc.AccuracyToAverageClose) * 0.5M;
                }
            }

            if (values.Count == 0)
            {
                return Array.Empty<string>();
            }

            qty = Math.Min(qty, values.Count);

            return values.OrderByDescending(x => x.Value).Take(qty).Select(x => x.Key).ToArray();
        }

        private string[] GetAllMethodNames(List<OhlcvObject> dataOhlcv, AnalysisType type)
        {
            List<string> cached = null;

            switch (type)
            {
                case AnalysisType.Pattern:
                    if (_cachedPatternNames == null)
                    {
                        InitializePatterns(dataOhlcv);
                        _cachedPatternNames = _patterns.GetAllMethodNames();
                    }
                    cached = _cachedPatternNames;
                    break;

                case AnalysisType.Formation:
                    if (_cachedFormationNames == null)
                    {
                        InitializeFormations(dataOhlcv);
                        _cachedFormationNames = _formations.GetAllMethodNames();
                    }
                    cached = _cachedFormationNames;
                    break;

                case AnalysisType.Fibonacci:
                    if (_cachedFibonacciNames == null)
                    {
                        InitializeFibonacci(dataOhlcv);
                        _cachedFibonacciNames = _fibonacci.GetAllMethodNames();
                    }
                    cached = _cachedFibonacciNames;
                    break;

                default:
                    return Array.Empty<string>();
            }

            return cached.ToArray();
        }

        private AccuracyObject GetAccuracyForType(List<OhlcvObject> dataOhlcv, string name, AnalysisType type, int? candlesAhead = null)
        {
            switch (type)
            {
                case AnalysisType.Pattern:
                    return candlesAhead.HasValue
                        ? GetAverPercentPatternAccuracy(dataOhlcv, name, candlesAhead.Value)
                        : GetAverPercentPatternAccuracy(dataOhlcv, name);

                case AnalysisType.Formation:
                    return candlesAhead.HasValue
                        ? GetAverPercentFormationAccuracy(dataOhlcv, name, candlesAhead.Value)
                        : GetAverPercentFormationAccuracy(dataOhlcv, name);

                case AnalysisType.Fibonacci:
                    return candlesAhead.HasValue
                        ? GetAverPercentAccuracy(dataOhlcv, name, candlesAhead.Value)
                        : GetAverPercentAccuracy(dataOhlcv, name);

                default:
                    return new AccuracyObject { AccuracyToAverageClose = 0, AccuracyToEndClose = 0 };
            }
        }
    }
}
