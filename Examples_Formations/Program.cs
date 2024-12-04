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

            var accuracyForSelectedFormation30CandlesAhead = _accuracy.GetAverPercentFormationAccuracy(dataOhlcv, "Bearish Double Tops", 30);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to end of data set result: {0}", accuracyForSelectedFormation30CandlesAhead.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to average close result: {0}", accuracyForSelectedFormation30CandlesAhead.AccuracyToAverageClose);

            //SIGNALS
            var bullishCount = _signals.GetFormationsBullishSignalsCount(dataOhlcv);
            Console.WriteLine("Bullish signals count: {0}", bullishCount);

            var bearishCount = _signals.GetFormationsBearishSignalsCount(dataOhlcv);
            Console.WriteLine("Bearish signals count: {0}", bearishCount);

            var signalsCountMulti = _signals.GetMultipleFormationsSignalsCount(dataOhlcv, new string[] { "Bearish Double Tops", "Bearish Head And Shoulders" });
            Console.WriteLine("Multiple formations signals count: {0}", signalsCountMulti);

            var formationsSignalsCountSingle = _signals.GetFormationSignalsCount(dataOhlcv, "Bearish Double Tops");
            Console.WriteLine("Signals count for Bearish Double Tops: {0}", formationsSignalsCountSingle);

            var signalsCountMultiWeightened = _signals.GetMultipleFormationsSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Double Tops", 0.5M }, { "Bearish Head And Shoulders", 0.5M } });
            Console.WriteLine("Weightened index for selected multiple formations: {0}", signalsCountMultiWeightened);

            var signalsCountSingleWeightened = _signals.GetFormationSignalsIndex(dataOhlcv, "Bearish Double Tops", 0.5M);
            Console.WriteLine("Weightened index for selected single formation: {0}", signalsCountSingleWeightened);

            var ohlcSingleSignals = _signals.GetFormationsOhlcvWithSignals(dataOhlcv, "Bearish Double Tops");
            Console.WriteLine("Signals for single formation: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count());

            var ohlcMultiSignals = _signals.GetMultipleFormationsOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Double Tops", "Bearish Head And Shoulders" });
            Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count());

            //END
            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
