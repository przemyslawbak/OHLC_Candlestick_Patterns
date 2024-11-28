namespace Candlestick_Patterns
{
   public interface IFibonacci
   {
       List<string> GetFibonacciAllMethodNames();
       List<ZigZagObject> GetFibonacciSignalsList(string patternName);
       int GetFibonacciSignalsCount(string patternName);
   }
}
