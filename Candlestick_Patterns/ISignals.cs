using Candlestick_Patterns;

namespace Candlestick_Patterns
{
    public interface ISignals
    {
        /// <summary>
        /// Counting bullish signals that appear in the OHLC list
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bullish signals count integer</returns>
        int GetPatternsBullishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting bearish signals that appear in the OHLC list
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bearish signals count integer</returns>
        int GetPatternsBearishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting the number of signals that appear in the OHLC list for selected multiple patterns 
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternNames">Selected patterns</param>
        /// <returns>Signals count integer number</returns>
        int GetPatternsSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames);

        /// <summary>
        /// Counting the number of signals that appear in the OHLC list for selected single pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected pattern</param>
        /// <returns>Signals count integer number</returns>
        int GetPatternsSignalsCount(List<OhlcvObject> dataOhlcv, string patternName);

        /// <summary>
        /// Calculates the weighted index for the selected single pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected pattern</param>
        /// <param name="weight">Signal weight</param>
        /// <returns>Weighted index decimal number</returns>
        decimal GetPatternsSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight);

        /// <summary>
        /// Calculates the weighted index for the selected multiple patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternNamesWithWeights">Dictionary of pattern names with their weights</param>
        /// <returns>Signals count integer number</returns>
        decimal GetPatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights);

        /// <summary>
        /// Calculates signals for selected single pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected pattern</param>
        /// <returns>OHLC list with updated signal values</returns>
        List<OhlcvObject> GetPatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string patternName);

        /// <summary>
        /// Calculates signals for selected multiply patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternNames">Selected patterns</param>
        /// <returns>List of OHLC lists with updated signal values</returns>
        List<List<OhlcvObject>> GetPatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames);

        /// <summary>
        /// Counts the number of formations appearing in the OHLC list for a selected single formation
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected formation</param>
        /// <returns>Signals count integer number</returns>
        int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string patternName);

        /// <summary>
        /// Counts the number of Fibonacci patterns appearing in the OHLC list for a selected single Fibonacci pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternName">Selected Fibonacci pattern</param>
        /// <returns>Signals count integer number</returns>
        int GetFibonacciSignalsCount(List<OhlcvObject> dataOhlcv, string patternName);
    }
}