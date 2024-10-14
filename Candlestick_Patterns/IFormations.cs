namespace Candlestick_Patterns
{
   internal interface IFormations
    {
        List<string> GetFormationsAllMethodNames();
        List<ZigZagObject> GetFormationsSignalsQuantities(string patternName);
        int GetFormationsSignalsCount(string patternName);
    }
}
