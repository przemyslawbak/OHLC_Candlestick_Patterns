using Candlestick_Patterns;
using Newtonsoft.Json;
using OHLC_Candlestick_Patterns;

string json = string.Empty;
ISignals _signals = new Signals();
IAccuracyTrials _accuracy = new AccuracyTrials();
var client = new HttpClient();
var url = "https://gist.githubusercontent.com/przemyslawbak/c90528453d512a8d85ad2deea5cf6ad2/raw/aapl_us_d.csv";

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

var acc = _accuracy.GetPatternAccuracy(dataOhlcv, "Bullish3InsideUp");

var fibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bearish3Drive");
var otherFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bullish3Drive");

var formationsSignalsCountSingle = _signals.GetFormationSignalsCount(dataOhlcv, "BearishDoubleTops");

var bullishCount = _signals.GetPatternsBullishSignalsCount(dataOhlcv);
Console.WriteLine("Bullish signals count: {0}", bullishCount); //Bullish signals count:

var bearishCount = _signals.GetPatternsBearishSignalsCount(dataOhlcv);
Console.WriteLine("Bearish signals count: {0}", bearishCount); //Bearish signals count:

var signalsCountMulti = _signals.GetPatternsSignalsCount(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Multiple patterns signals count: {0}", signalsCountMulti); //Multiple patterns signals count:

var signalsCountSingle = _signals.GetPatternsSignalsCount(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Single pattern signals count: {0}", signalsCountSingle); //Single pattern signals count:

var signalsCountMultiWeightened = _signals.GetPatternsSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Belt Hold", 0.5M }, { "Bearish Black Closing Marubozu", 0.5M } });
Console.WriteLine("Weightened index for selected multiple patterns: {0}", signalsCountMultiWeightened); //Weightened index for selected multiple patterns:

var signalsCountSingleWeightened = _signals.GetPatternsSignalsIndex(dataOhlcv, "Bearish Black Closing Marubozu", 0.5M);
Console.WriteLine("Weightened index for selected single pattern: {0}", signalsCountSingleWeightened); //Weightened index for selected single pattern:

var ohlcSingleSignals = _signals.GetPatternsOhlcvWithSignals(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Signals for single pattern: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count()); //Signals for single pattern:

var ohlcMultiSignals = _signals.GetPatternsOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count()); //Number of lists returned:

Console.ReadLine();