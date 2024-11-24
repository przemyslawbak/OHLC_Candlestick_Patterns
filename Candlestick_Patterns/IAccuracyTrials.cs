using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public interface IAccuracyTrials
    {
        /// <summary>
        /// Tests selected pattern accuracy in given data comparing to average close prices after signal appears
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected pattern</param>
        /// <returns>AccuracyObject containing values comparing to AVER and END closing</returns>
        AccuracyObject GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName);
    }
}
