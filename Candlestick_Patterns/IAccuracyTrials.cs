using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public interface IAccuracyTrials
    {

        /// <summary>
        /// Tests selected pattern accuracy in given data
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected pattern</param>
        /// <returns>OHLC list with updated signal values</returns>
        List<OhlcvObject> GetPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName);
    }
}
