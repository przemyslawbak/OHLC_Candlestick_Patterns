using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public class AccuracyTrials : IAccuracyTrials
    {
        public decimal GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName)
        {
            //todo #1:
            //- for each signal get close value
            //- for each signal get list end close minus signal close value -> result
            //- sum all results

            //todo: #2:
            //- for each signal get close value
            //- for each signal get average price after the signal minus signal close value -> result
            //- sum all results

            return 0M;
        }
    }
}
