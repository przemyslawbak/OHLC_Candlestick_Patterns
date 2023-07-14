using Candlestick_Patterns;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

Patterns _patterns;

var client = new HttpClient();
string url = string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&outputsize=full&apikey=demo");
string json = string.Empty;


using (HttpResponseMessage response = await client.GetAsync(url))
{
    using (HttpContent content = response.Content)
    {
        json = content.ReadAsStringAsync().Result;
    }
}

var jObject = JObject.Parse(json);
var dupa = jObject["Time Series (5min)"].ToString();

var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(dupa).Select(x => new OhlcvObject()
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