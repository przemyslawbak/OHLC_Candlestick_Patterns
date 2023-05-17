// See https://aka.ms/new-console-template for more information
using Candlestick_Patterns;
using Financial_Candlestick_Patterns;
using Newtonsoft.Json;

//patterns: https://tickblaze.com/scripts/pattern

KeyLocker _keys = new KeyLocker();
Scrapper _scrapper = new Scrapper();
Patterns _patterns;

var key = _keys.GetApiKey();
var url = "https://financialmodelingprep.com/api/v3/historical-chart/1min/%5EGSPC?apikey=" + key;
var json = await _scrapper.GetHtml(url);
var data = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select((x, index) => new OhlcvObject()
{
    Index = index,
    Open = x.Open,
    High = x.High,
    Low = x.Low,
    Close = x.Close,
    Signal = false
}).ToList();

_patterns = new Patterns(data);
data = _patterns.AddSignals();

Console.ReadLine();