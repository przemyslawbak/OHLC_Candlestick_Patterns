using Candlestick_Patterns;
using Newtonsoft.Json;
using OHLC_Candlestick_Patterns;

namespace Examples_Formations
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
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

            //ACCURACY TRIALS
            var accuracyPercentageSummary = _accuracy.GetAverPercentFormationAccuracy(dataOhlcv, "Bearish Double Tops");
            Console.WriteLine("Accuracy percentage summary comparing to end of data set result: {0}", accuracyPercentageSummary.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary comparing to average close result: {0}", accuracyPercentageSummary.AccuracyToAverageClose);

            var accuracyForSelectedPattern30CandlesAhead = _accuracy.GetAverPercentFormationAccuracy(dataOhlcv, "Bearish Double Tops", 30);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to end of data set result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to average close result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToAverageClose);

            //SIGNALS
            var formationsSignalsCountSingle = _signals.GetFormationSignalsCount(dataOhlcv, "Bearish Double Tops");
            Console.WriteLine("Signals count for Bearish Double Tops: {0}", formationsSignalsCountSingle);

            //END
            Console.ReadLine();
        }
    }
}
