﻿namespace Candlestick_Patterns
{
   internal interface IFormations
    {
        List<string> GetFormationsAllMethodNames();
        List<ZigZagObject> GetFormationsSignalsList(string patternName);
        int GetFormationsSignalsCount(string patternName);
    }
}
