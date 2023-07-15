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
}).ToList();

var bullishCount = _signals.GetBullishSignalsCount(dataOhlcv);
var bearishCount = _signals.GetBearishSignalsCount(dataOhlcv);
var signalsCountMulti = _signals.GetSignalsCount(dataOhlcv, new string[] { "BearishBeltHold", "BearishBlackClosingMarubozu" });
var signalsCountSingle = _signals.GetSignalsCount(dataOhlcv, "BearishBlackClosingMarubozu" );
var signalsCountMultiWeightened = _signals.GetSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "BearishBeltHold", 0.5M }, { "BearishBlackClosingMarubozu", 0.5M } });
var signalsCountSingleWeightened = _signals.GetSignalsIndex(dataOhlcv, "BearishBlackClosingMarubozu", 0.5M );
var ohlcSingleSignals = _signals.GetOhlcvWithSignals(dataOhlcv, "BearishBlackClosingMarubozu");
var ohlcMultiSignals = _signals.GetOhlcvWithSignals(dataOhlcv, new string[] { "BearishBeltHold", "BearishBlackClosingMarubozu" });

Console.ReadLine();