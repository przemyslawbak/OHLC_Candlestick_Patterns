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

            return GetAccuracyResults(signalsList);

            //todo:
            //OK - for each signal get close value
            //OK - create accuracy model containing AVER and END results props

            //todo:
            //OK - for each signal get close value
            //OK - create accuracy model containing AVER and END results props
        }

        private AccuracyObject GetAccuracyResults(List<OhlcvObject> signalsList)
        {
            for (int i = 0; i < signalsList.Count; i++)
            {
                if (signalsList[i].Signal)
                {
                    //- for each signal get average price after the signal minus signal close value -> result
                    //- for each signal get list end close minus signal close value -> result
                }
            }

            //- sum all results

            return new AccuracyObject();
        }
    }
}
