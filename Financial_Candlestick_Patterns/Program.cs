using Candlestick_Patterns;
using Newtonsoft.Json;

Patterns _patterns;
string dataSampleFile = "json_data_sample.txt";
string json = string.Empty;

using (StreamReader r = new StreamReader(dataSampleFile))
{
    json = r.ReadToEnd();
}

var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
{
    Open = x.Open,
    High = x.High,
    Low = x.Low,
    Close = x.Close,
    Volume = x.Volume,
}).ToList();

_patterns = new Patterns(dataOhlcv);
var bullishSignalsCount = _patterns.GetBullishSignalsCount();
var bearishSignalsCount = _patterns.GetBearishSignalsCount();

//todo: add volume weightening

Console.ReadLine();