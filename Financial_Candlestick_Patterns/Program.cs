using Candlestick_Patterns;
using Newtonsoft.Json;

string json = string.Empty;

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

//var bullishSignalsCount = _patterns.GetBullishSignalsCount();
//var bearishSignalsCount = _patterns.GetBearishSignalsCount();

Console.ReadLine();