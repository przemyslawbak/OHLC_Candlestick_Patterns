using Candlestick_Patterns;
using Newtonsoft.Json;
using System.Net.Http;

namespace WPFGraphMaker
{
    public interface IDataFromJson
    {
        List<ZigZagObject> GetDataFromJson(string patternName, string json, string groupName);
    }
    
    public class DataFromJson : IDataFromJson
    {
        IFibonacci _fibonacci;
        IFormations _formations;

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

        public List<ZigZagObject> GetDataFromJson(string patternName, string json, string groupName)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json, settings).Select(x => new OhlcvObject()
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
            }).ToList();

            dataOhlcv = dataOhlcv.Where(x => x.Open != 0 && x.High != 0 && x.Low != 0 && x.Close != 0).ToList();
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
    }
}


