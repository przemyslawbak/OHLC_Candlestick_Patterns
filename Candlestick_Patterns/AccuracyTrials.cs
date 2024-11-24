using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public class AccuracyTrials : IAccuracyTrials
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;

        public AccuracyObject GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResults(signalsList, patternName);
        }

        public AccuracyObject GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName, int candlesAhead)
        {
            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            return GetAccuracyResultsForCandlesAhead(signalsList, patternName, candlesAhead);
        }

        private AccuracyObject GetAccuracyResultsForCandlesAhead(List<OhlcvObject> signalsList, string patternName, int candlesAhead)
        {
            var multiplier = patternName.Contains("Bullish") ? 1M : patternName.Contains("Bearish") ? -1M : 0M;

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

                if (i + candlesAhead < lastIndex)
                {
                    lastClose = signalsList[i + candlesAhead].Close;
                    lastIndex = i + candlesAhead;
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

        private AccuracyObject GetAccuracyResults(List<OhlcvObject> signalsList, string patternName)
        {
            var multiplier = patternName.Contains("Bullish") ? 1M : patternName.Contains("Bearish") ? -1M : 0M;

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
