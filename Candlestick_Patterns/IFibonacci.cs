namespace Candlestick_Patterns
{
   internal interface IFibonacci
   {
       List<string> GetFibonacciAllMethodNames();
       List<ZigZagObject> GetFibonacciSignalsQuantities(string patternName);
       int GetFibonacciSignalsCount(string patternName);
   }
}
