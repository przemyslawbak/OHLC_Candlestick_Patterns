namespace Candlestick_Patterns
{
   internal interface IFormations
    {
        List<string> GetFormationsAllMethodNames();
        List<ZigZagObject> GetFormationsSignals(string patternName);
        int GetFormationsSignalsCount(string patternName);
    }
}
