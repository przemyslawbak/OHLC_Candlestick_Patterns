using Candlestick_Patterns;
using GraphMaker;
using Newtonsoft.Json;

string json = string.Empty;
ISignals _signals = new Signals();
IFiboTester _fiboTester = new FiboTester();
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

_fiboTester.ShowOnGraph(dataOhlcv, "BullishButterfly");
