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
            var bullishButterflyFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BullishButterfly");
            var bearishButterflyFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BearishButterfly");
            Console.WriteLine("Bullish signals count for BullishButterfly: {0}", bullishButterflyFibSingle);
            Console.WriteLine("Bearish signals count for BearishButterfly: {0}", bearishButterflyFibSingle);

            var bearishabcdFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BearishABCD");
            var bullishabcdFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BullishABCD");

            var bullishDriveFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bullish3Drive");
            var bearishDriveFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bearish3Drive");

            var bearishExtensionFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bearish3Extension");
            var bulllishExtensionFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bullish3Extension");

            var BullishRetracementFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bullish3Retracement");
            var BearishRetracementFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "Bearish3Retracement");

            var bearishGartleyFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BearishGartley");
            var bullishGartleyFibSingle = _signals.GetFibonacciSignalsCount(dataOhlcv, "BullishGartley");


            //END
            Console.ReadLine();
        }
    }
}
