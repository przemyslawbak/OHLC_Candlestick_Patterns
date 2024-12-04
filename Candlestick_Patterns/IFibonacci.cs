namespace Candlestick_Patterns
{
   public interface IFibonacci
   {
       List<string> GetFibonacciAllMethodNames();
       List<ZigZagObject> GetFibonacciSignalsList(string patternName);
       int GetFibonacciSignalsCount(string patternName);
        List<string> GetAllMethodNames();
        int GetSignalsCount(string patternName);
    }
}
