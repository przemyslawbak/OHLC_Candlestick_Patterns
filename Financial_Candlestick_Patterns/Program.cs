using Candlestick_Patterns;
using Newtonsoft.Json;

string json = string.Empty;
ISignals _signals = new Signals();
var client = new HttpClient();
var url = "https://gist.githubusercontent.com/przemyslawbak/2058d9aeddfe09d2a26da81dfc16e5d0/raw/json_data_sample.txt";

using (HttpResponseMessage response = await client.GetAsync(url))
{
    using (HttpContent content = response.Content)
    {
        json = content.ReadAsStringAsync().Result;
    }
}

var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
{
    Open = x.Open,
    High = x.High,
    Low = x.Low,
    Close = x.Close,
    Volume = x.Volume,
}).Reverse().ToList();


var formationsSignalsCountSingle = _signals.GetFormationSignalsCount(dataOhlcv, "BearishDoubleTops");

var bullishCount = _signals.GetPatternsBullishSignalsCount(dataOhlcv);
Console.WriteLine("Bullish signals count: {0}", bullishCount); //Bullish signals count: 89

var bearishCount = _signals.GetPatternsBearishSignalsCount(dataOhlcv);
Console.WriteLine("Bearish signals count: {0}", bearishCount); //Bearish signals count: 128

var signalsCountMulti = _signals.GetPatternsSignalsCount(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Multiple patterns signals count: {0}", signalsCountMulti); //Multiple patterns signals count: 6

var signalsCountSingle = _signals.GetPatternsSignalsCount(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Single pattern signals count: {0}", signalsCountSingle); //Single pattern signals count: 6

var signalsCountMultiWeightened = _signals.GetPatternsSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Belt Hold", 0.5M }, { "Bearish Black Closing Marubozu", 0.5M } });
Console.WriteLine("Weightened index for selected multiple patterns: {0}", signalsCountMultiWeightened); //Weightened index for selected multiple patterns: 3,0

var signalsCountSingleWeightened = _signals.GetPatternsSignalsIndex(dataOhlcv, "Bearish Black Closing Marubozu", 0.5M);
Console.WriteLine("Weightened index for selected single pattern: {0}", signalsCountSingleWeightened); //Weightened index for selected single pattern: 3,0

var ohlcSingleSignals = _signals.GetPatternsOhlcvWithSignals(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Signals for single pattern: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count()); //Signals for single pattern: 6

var ohlcMultiSignals = _signals.GetPatternsOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count()); //Number of lists returned: 2

Console.ReadLine();