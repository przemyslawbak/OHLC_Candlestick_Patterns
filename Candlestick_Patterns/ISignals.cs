namespace Candlestick_Patterns
{
    public interface ISignals
    {
        /// <summary>
        /// Counting bullish signals that appear in the OHLC list across all patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bullish signals count integer</returns>
        int GetPatternsBullishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting bearish signals that appear in the OHLC list across all patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bearish signals count integer</returns>
        int GetPatternsBearishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting the number of signals that appear in the OHLC list for selected multiple patterns 
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternNames">Selected patterns array</param>
        /// <returns>Signals count integer number</returns>
        int GetMultiplePatternsSignalsCount(List<OhlcvObject> dataOhlcv, string[] patternNames);

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
        decimal GetPatternSignalsIndex(List<OhlcvObject> dataOhlcv, string patternName, decimal weight);

        /// <summary>
        /// Calculates the weighted index for the selected multiple patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="patternNamesWithWeights">Dictionary of pattern names with their weights</param>
        /// <returns>Signals count integer number</returns>
        decimal GetMultiplePatternsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> patternNamesWithWeights);

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
        List<List<OhlcvObject>> GetMultiplePatternsOhlcvWithSignals(List<OhlcvObject> dataOhlcv, string[] patternNames);

        /// <summary>
        /// Counting bullish signals that appear in the OHLC list across all formations
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bullish signals count integer</returns>
        int GetFormationsBullishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting bearish signals that appear in the OHLC list across all formations
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bearish signals count integer</returns>
        int GetFormationsBearishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting the number of signals that appear in the OHLC list for selected multiple formations 
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationNames">Selected formations array</param>
        /// <returns>Signals count integer number</returns>
        int GetMultipleFormationsSignalsCount(List<OhlcvObject> dataOhlcv, string[] formationNames);

        /// <summary>
        /// Counts the number of formations appearing in the OHLC list for a selected single formation
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationName">Selected formation</param>
        /// <returns>Signals count integer number</returns>
        int GetFormationSignalsCount(List<OhlcvObject> dataOhlcv, string formationName);

        /// <summary>
        /// Calculates the weighted index for the selected single formation
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboName">Selected formation</param>
        /// <param name="weight">Signal weight</param>
        /// <returns>Weighted index decimal number</returns>
        decimal GetFormationSignalsIndex(List<OhlcvObject> dataOhlcv, string fiboName, decimal weight);

        /// <summary>
        /// Calculates the weighted index for the selected multiple formations
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationsNamesWithWeights">Dictionary of formations names with their weights</param>
        /// <returns>Signals count integer number</returns>
        decimal GetMultipleFormationsSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> formationsNamesWithWeights);

        /// <summary>
        /// Calculates signals for selected single formation
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboName">Selected formation</param>
        /// <returns>ZigZag list with updated signal values</returns>
        List<ZigZagObject> GetFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string fiboName);

        /// <summary>
        /// Calculates signals for selected multiply formation
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationsNames">Selected formation</param>
        /// <returns>List of ZigZag lists with updated signal values</returns>
        List<List<ZigZagObject>> GetMultipleFormationsZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] formationsNames);

        /// <summary>
        /// Counts the number of fibonacci patterns appearing in the OHLC list for a selected single fibonacci pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationName">Selected fibonacci pattern</param>
        /// <returns>Signals count integer number</returns>
        int GetFibonacciSignalsCount(List<OhlcvObject> dataOhlcv, string formationName);

        /// <summary>
        /// Counting bullish signals that appear in the OHLC list across all fibonacci patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bullish signals count integer</returns>
        int GetFiboBullishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting bearish signals that appear in the OHLC list across all fibonacci patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <returns>Bearish signals count integer</returns>
        int GetFiboBearishSignalsCount(List<OhlcvObject> dataOhlcv);

        /// <summary>
        /// Counting the number of signals that appear in the OHLC list for selected multiple fibonacci patterns 
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboNames">Selected fibonacci pattern names array</param>
        /// <returns>Signals count integer number</returns>
        int GetMultipleFiboSignalsCount(List<OhlcvObject> dataOhlcv, string[] fiboNames);

        /// <summary>
        /// Counts the number of formations appearing in the OHLC list for a selected single fibonacci pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationName">Selected formation</param>
        /// <returns>Signals count integer number</returns>
        int GetFiboSignalsCount(List<OhlcvObject> dataOhlcv, string formationName);

        /// <summary>
        /// Calculates the weighted index for the selected single fibonacci pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboName">Selected fibonacci pattern</param>
        /// <param name="weight">Signal weight</param>
        /// <returns>Weighted index decimal number</returns>
        decimal GetFiboSignalsIndex(List<OhlcvObject> dataOhlcv, string fiboName, decimal weight);

        /// <summary>
        /// Calculates the weighted index for the selected multiple fibonacci patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="formationsNamesWithWeights">Dictionary of fibonacci pattern names with their weights</param>
        /// <returns>Signals count integer number</returns>
        decimal GetMultipleFiboSignalsIndex(List<OhlcvObject> dataOhlcv, Dictionary<string, decimal> fibosNamesWithWeights);

        /// <summary>
        /// Calculates signals for selected single fibonacci pattern
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboName">Selected formation</param>
        /// <returns>ZigZag list with updated signal values</returns>
        List<ZigZagObject> GetFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string fiboName);

        /// <summary>
        /// Calculates signals for selected multiply fibonacci patterns
        /// </summary>
        /// <param name="dataOhlcv">OHLC object list</param>
        /// <param name="fiboNames">Selected fibonacci patterns</param>
        /// <returns>List of ZigZag lists with updated signal values</returns>
        List<List<ZigZagObject>> GetMultipleFiboZigZagWithSignals(List<OhlcvObject> dataOhlcv, string[] fiboNames);
    }
}