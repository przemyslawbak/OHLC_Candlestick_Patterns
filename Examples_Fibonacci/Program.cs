using Candlestick_Patterns;
using Newtonsoft.Json;
using OHLC_Candlestick_Patterns;

namespace Examples_Fibonacci
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
            var accuracyPercentageSummary = _accuracy.GetAverPercentFiboAccuracy(dataOhlcv, "Bearish 3 Drive");
            Console.WriteLine("Accuracy percentage summary comparing to end of data set result: {0}", accuracyPercentageSummary.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary comparing to average close result: {0}", accuracyPercentageSummary.AccuracyToAverageClose);

            var accuracyForSelectedPattern30CandlesAhead = _accuracy.GetAverPercentFiboAccuracy(dataOhlcv, "Bearish 3 Drive", 30);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to end of data set result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to average close result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToAverageClose);

            //SIGNALS
            var bullishCount = _signals.GetFiboBullishSignalsCount(dataOhlcv);
            Console.WriteLine("Bullish signals count: {0}", bullishCount);

            var bearishCount = _signals.GetFiboBearishSignalsCount(dataOhlcv);
            Console.WriteLine("Bearish signals count: {0}", bearishCount);

            var signalsCountMulti = _signals.GetMultipleFiboSignalsCount(dataOhlcv, new string[] { "BearishABCD", "Bearish 3 Drive" });
            Console.WriteLine("Multiple fibo signals count: {0}", signalsCountMulti);

            var signalsCountSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BearishABCD");
            Console.WriteLine("Signals count for Bearish Double Tops: {0}", signalsCountSingle);

            var signalsCountMultiWeightened = _signals.GetMultipleFiboSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "BearishABCD", 0.5M }, { "Bearish 3 Drive", 0.5M } });
            Console.WriteLine("Weightened index for selected multiple fibo: {0}", signalsCountMultiWeightened);

            var signalsCountSingleWeightened = _signals.GetFiboSignalsIndex(dataOhlcv, "BearishABCD", 0.5M);
            Console.WriteLine("Weightened index for selected single fibo: {0}", signalsCountSingleWeightened);

            var ohlcSingleSignals = _signals.GetFiboOhlcvWithSignals(dataOhlcv, "BearishABCD");
            Console.WriteLine("Signals for single fibo: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count());

            var ohlcMultiSignals = _signals.GetMultipleFiboOhlcvWithSignals(dataOhlcv, new string[] { "BearishABCD", "Bearish 3 Drive" });
            Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count());

            //END
            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
