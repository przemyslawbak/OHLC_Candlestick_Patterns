// See https://aka.ms/new-console-template for more information
using Financial_Candlestick_Patterns;
using Newtonsoft.Json;

//patterns: https://tickblaze.com/scripts/pattern

KeyLocker _keys = new KeyLocker();
Scrapper _scrapper = new Scrapper();

var key = _keys.GetApiKey();
var url = "https://financialmodelingprep.com/api/v3/historical-chart/1min/%5EGSPC?apikey=" + key;
var json = await _scrapper.GetHtml(url);
var data = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
{
    Open = x.Open,
    High = x.High,
    Low = x.Low,
    Close = x.Close,
    Signal = false
});

Console.ReadLine();