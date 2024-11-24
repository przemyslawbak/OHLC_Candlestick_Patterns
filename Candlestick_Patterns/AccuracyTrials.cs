using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public class AccuracyTrials : IAccuracyTrials
    {
        IPatterns _patterns;
        IFormations _formations;
        IFibonacci _fibonacci;

        public decimal GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);

            //todo:
            //OK - for each signal get close value
            //- create accuracy model containing AVER and END results props
            //- for each signal get average price after the signal minus signal close value -> result
            //- sum all results

            //todo:
            //OK - for each signal get close value
            //- create accuracy model containing AVER and END results props
            //- for each signal get list end close minus signal close value -> result
            //- sum all results

            return 0M;
        }
    }
}
