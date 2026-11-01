using Candlestick_Patterns;
using Newtonsoft.Json;
using System.Net.Http;

namespace WPFGraphMaker
{
    public interface IDataFromJson
    {
        List<OhlcvObject> GetCandlestickDataFromJson(string patternName, string json);
        List<OhlcvObject> GetOhlcvDataFromJson(string json);
        List<ZigZagObject> GetDataFromJson(string patternName, string json, string groupName);
    }
    
    public class DataFromJson : IDataFromJson
    {
        IFibonacci _fibonacci;
        IFormations _formations;
        IPatterns _patterns;

        public async Task<List<ZigZagObject>> GetPoints(string patternName)
        {
            string json = string.Empty;
            var client = new HttpClient();
            var url = "https://gist.githubusercontent.com/przemyslawbak/c90528453d512a8d85ad2deea5cf6ad2/raw/aapl_us_d.csv";

            using (var httpClient = new HttpClient())
            {
                json = await httpClient.GetStringAsync(url);
            }

            var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
            }).ToList();


            _fibonacci = new Fibonacci(dataOhlcv);
            var signalList = _fibonacci.GetFibonacciSignalsList(patternName);
            return signalList;
        }

        public List<OhlcvObject> GetOhlcvDataFromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            var candles = JsonConvert.DeserializeObject<List<OhlcvObject>>(json, settings)
                ?? throw new JsonSerializationException("The JSON document must contain an array of OHLCV candles.");

            // Preserve the same input filtering rules used by the existing detectors.
            return candles.Where(x => x != null && x.Open != 0M && x.High != 0M
                    && x.Low != 0M && x.Close != 0M)
                .Select(x => new OhlcvObject
                {
                    Open = x.Open,
                    High = x.High,
                    Low = x.Low,
                    Close = x.Close,
                    Volume = x.Volume,
                    Signal = false
                })
                .ToList();
        }

        public List<ZigZagObject> GetDataFromJson(string patternName, string json, string groupName)
        {
            var dataOhlcv = GetOhlcvDataFromJson(json);
            var signalList = new List<ZigZagObject>();
            if (groupName == "fibonacci")
            {
                _fibonacci = new Fibonacci(dataOhlcv);
                signalList = _fibonacci.GetFibonacciSignalsList(patternName);
            }
            if (groupName == "formations")
            {
               _formations = new Formations(dataOhlcv);
                signalList = _formations.GetFormationsSignalsList(patternName);
            }
            return signalList;
        }

        public List<OhlcvObject> GetCandlestickDataFromJson(string patternName, string json)
        {
            var dataOhlcv = GetOhlcvDataFromJson(json);

            _patterns = new Patterns(dataOhlcv);

            var signalsList = _patterns.GetPatternsSignalsList(patternName);
            return signalsList;
        }
    }
}


