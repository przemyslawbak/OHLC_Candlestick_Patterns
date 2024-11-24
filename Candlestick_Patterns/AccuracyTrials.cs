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

            var lastClose = signalsList.Last().Close;
            List<decimal> accuracyEndResuts = new List<decimal>();
            List<decimal> accuracyAverResuts = new List<decimal>();

            for (int i = 0; i < signalsList.Count; i++)
            {
                if (signalsList[i].Signal)
                {
                    accuracyEndResuts.Add((lastClose - signalsList[i].Close) * multiplier);

                    var averCloseAfterSignal = signalsList.Skip(i);
                    accuracyAverResuts.Add((averCloseAfterSignal.Average(x => x.Close) - signalsList[i].Close) * multiplier);
                }
            }

            //- sum all results

            return new AccuracyObject();
        }
    }
}
