namespace Candlestick_Patterns
{
    public interface ISignalEngine
    {
        List<string> GetAllMethodNames();
        int GetSignalsCount(string name);
    }
}