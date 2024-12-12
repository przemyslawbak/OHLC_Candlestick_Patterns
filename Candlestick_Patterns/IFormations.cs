
namespace Candlestick_Patterns
{
   public interface IFormations
    {
        List<string> GetFormationsAllMethodNames();
        List<ZigZagObject> GetFormationsSignalsList(string patternName);
        int GetFormationsSignalsCount(string patternName);
        List<string> GetAllMethodNames();
        int GetSignalsCount(string patternName);
    }
}
