namespace Candlestick_Patterns
{
   internal interface IFibonacci
   {
       List<string> GetFibonacciAllMethodNames();
       List<ZigZagObject> GetFibonacciSignalsList(string patternName);
       int GetFibonacciSignalsCount(string patternName);
   }
}
