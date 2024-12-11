using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    public interface IAccuracyTrials
    {
        /// <summary>
        /// Tests selected pattern accuracy in given data comparing to average close prices after signal appears
        /// Positive results indicate that they are in line with expectations. Negative results indicate poor accuracy
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="patternName">Selected pattern</param>
        /// <returns>AccuracyObject containing values comparing to AVER and END closing</returns>
        AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName);

        /// <summary>
        /// Tests selected pattern accuracy in given data 30 candles ahead comparing to average close prices after signal appears
        /// Positive results indicate that they are in line with expectations. Negative results indicate poor accuracy
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="patternName">Selected pattern</param>
        /// <param name="candlesAheadQty">Qty of candles ahead to be evaluated</param>
        /// <returns>AccuracyObject containing values comparing to AVER and END closing</returns>
        AccuracyObject GetAverPercentPatternAccuracy(List<OhlcvObject> dataOhlcv, string patternName, int candlesAheadQty);

        /// <summary>
        /// Tests selected formation accuracy in given data comparing to average close prices after signal appears
        /// Positive results indicate that they are in line with expectations. Negative results indicate poor accuracy
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="formation">Selected formation</param>
        /// <returns>AccuracyObject containing values comparing to AVER and END closing</returns>
        AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string formation);

        /// <summary>
        /// Tests selected formation accuracy in given data 30 candles ahead comparing to average close prices after signal appears
        /// Positive results indicate that they are in line with expectations. Negative results indicate poor accuracy
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="formation">Selected formation</param>
        /// <param name="candlesAheadQty">Qty of candles ahead to be evaluated</param>
        /// <returns>AccuracyObject containing values comparing to AVER and END closing</returns>
        AccuracyObject GetAverPercentAccuracy(List<OhlcvObject> dataOhlcv, string formation, int candlesAheadQty);

        /// <summary>
        /// Finds an array of candlestick pattern names resulting with positive accuracy in relation to average close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToAverPatterns(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of formations names resulting with positive accuracy in relation to average close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToAverFormations(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of fibonacci patterns names resulting with positive accuracy in relation to average close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToAverFibo(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of candlestick pattern names resulting with positive accuracy in relation to last close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToEndPatterns(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of formations names resulting with positive accuracy in relation to last close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToEndFormations(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of fibonacci patterns names resulting with positive accuracy in relation to last close prices in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <returns>Array of names</returns>
        string[] GetPositiveAccuracyToEndFibo(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Finds an array of candlestick pattern names resulting with best accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all patterns</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyPatterns(List<OhlcvObject> dataOhlcv, int topPercentage);

        /// <summary>
        /// Finds an array of formations names resulting with positive accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all formations</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyFormations(List<OhlcvObject> dataOhlcv, int topPercentage);

        /// <summary>
        /// Finds an array of fibonacci patterns names resulting with positive accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all fibo</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyFibo(List<OhlcvObject> dataOhlcv, int topPercentage);

        /// <summary>
        /// Finds an array of candlestick pattern names resulting with best accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all patterns</param>
        /// <param name="candlesAheadQty">Qty of candles ahead to be evaluated</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyPatterns(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead);

        /// <summary>
        /// Finds an array of formations names resulting with positive accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all formations</param>
        /// <param name="candlesAheadQty">Qty of candles ahead to be evaluated</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyFormations(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead);

        /// <summary>
        /// Finds an array of fibonacci patterns names resulting with positive accuracy in given data set
        /// </summary>
        /// <param name="dataOhlcv">OHLCV data set</param>
        /// <param name="topPercentage">Top percentage of all fibo</param>
        /// <param name="candlesAheadQty">Qty of candles ahead to be evaluated</param>
        /// <returns>Array of names</returns>
        string[] GetBestAccuracyFibo(List<OhlcvObject> dataOhlcv, int topPercentage, int candlesAhead);
    }
}
