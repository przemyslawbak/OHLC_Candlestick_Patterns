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

        public AccuracyObject GetAverPercentFormationAccuracy(List<OhlcvObject> dataOhlcv, string formationName)
        {
            _formations = new Formations(dataOhlcv);

            var signalsList = _formations.GetFormationsSignalsList(formationName);

            var ohlcvList = signalsList.Select(s => new OhlcvObject() 
            {
                Close = s.Close,
                High = s.Close, 
                Low = s.Close, 
                Open = s.Close, 
                Signal = s.Signal, 
                Volume = 0M,
            }).ToList();

            return GetAccuracyResults(ohlcvList, formationName);
        }

        public AccuracyObject GetAverPercentFormationAccuracy(List<OhlcvObject> dataOhlcv, string formationName, int candlesAheadQty)
        {
            _formations = new Formations(dataOhlcv);

            var signalsList = _formations.GetFormationsSignalsList(formationName);

            var ohlcvList = signalsList.Select(s => new OhlcvObject()
            {
                Close = s.Close,
                High = s.Close,
                Low = s.Close,
                Open = s.Close,
                Signal = s.Signal,
                Volume = 0M,
            }).ToList();

            return GetAccuracyResultsForNCandlesAhead(ohlcvList, formationName, candlesAheadQty);
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

                    var averCloseAfterSignal = signalsList.Skip(i);
                    accuracyAverResuts.Add((averCloseAfterSignal.Average(x => x.Close) - signalsList[lastIndex].Close) * multiplier / signalsList[lastIndex].Close * 100);
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

                    var averCloseAfterSignal = signalsList.Skip(i);
                    accuracyAverResuts.Add((averCloseAfterSignal.Average(x => x.Close) - signalsList[i].Close) * multiplier / signalsList[i].Close * 100);
                }
            }

            return new AccuracyObject()
            {
                AccuracyToAverageClose = accuracyAverResuts.Average(),
                AccuracyToEndClose = accuracyEndResuts.Average(),
            };
        }
    }
}
