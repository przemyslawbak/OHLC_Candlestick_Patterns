using Candlestick_Patterns;
using Newtonsoft.Json;
using OHLC_Candlestick_Patterns;

namespace Examples_Formations
{
    internal class Program
    {
        private static ISignals _signals = new Signals();
        private static IAccuracyTrials _accuracy = new AccuracyTrials();

        static async Task Main(string[] args)
        {
            string json = string.Empty;
            var client = new HttpClient();
            var url = "https://gist.github.com/przemyslawbak/92c3d4bba27cfd2b88d0dd916bbdad14/raw/AAL_1min.json";

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    json = content.ReadAsStringAsync().Result;
                }
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json, settings)
                .Select(x => new OhlcvObject()
                {
                    Open = x.Open,
                    High = x.High,
                    Low = x.Low,
                    Close = x.Close,
                    Volume = x.Volume,
                })
                .Where(x => x.Open != 0 && x.High != 0 && x.Low != 0 && x.Close != 0)
                .ToList();

            //ACCURACY TRIALS
            var accuracyPercentageSummary = _accuracy.GetAverPercentAccuracy(dataOhlcv, "Bullish 3 Inside Up");
            Console.WriteLine("Accuracy percentage summary comparing to end of data set result: {0}", accuracyPercentageSummary.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary comparing to average close result: {0}", accuracyPercentageSummary.AccuracyToAverageClose);

            var accuracyForSelectedFormation30CandlesAhead = _accuracy.GetAverPercentAccuracy(dataOhlcv, "Bullish 3 Inside Up", 30);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to end of data set result: {0}", accuracyForSelectedFormation30CandlesAhead.AccuracyToEndClose);
            Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to average close result: {0}", accuracyForSelectedFormation30CandlesAhead.AccuracyToAverageClose);

            var accuracyAverPositive = _accuracy.GetPositiveAccuracyToAverFibos(dataOhlcv);
            Console.WriteLine("Formations with positive accuracy rate comaring to aver. close price: {0}", string.Join(",", accuracyAverPositive));

            var accuracyEndPositive = _accuracy.GetPositiveAccuracyToEndFormations(dataOhlcv);
            Console.WriteLine("Formations with positive accuracy rate comaring to end close price: {0}", string.Join(",", accuracyEndPositive));

            var accuracyBest = _accuracy.GetBestAccuracyFormations(dataOhlcv, 25);
            Console.WriteLine("25% of best formations comparing to end and aver. close price: {0}", string.Join(",", accuracyBest));

            var accuracyBest30CandlesAhead = _accuracy.GetBestAccuracyFormations(dataOhlcv, 25, 30);
            Console.WriteLine("25% of best formations 30 candles ahead comparing to end and aver. close price: {0}", string.Join(",", accuracyBest30CandlesAhead));

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

            var zigZagSingleSignals = _signals.GetFormationsZigZagWithSignals(dataOhlcv, "Bearish Double Tops");
            Console.WriteLine("Signals for single formation: {0}", zigZagSingleSignals.Where(x => x.Signal == true).Count());

            var zigZagMultiSignals = _signals.GetMultipleFormationsZigZagWithSignals(dataOhlcv, new string[] { "Bearish Double Tops", "Bearish Head And Shoulders" });
            Console.WriteLine("Number of lists returned: {0}", zigZagMultiSignals.Count());

            var formationsBest = GetBestFormationsValue(accuracyBest, 1, dataOhlcv);

            //END
            Console.WriteLine("END");
            Console.ReadLine();
        }

        private static int GetBestFormationsValue(string[] accuracyBest, int multiplier, List<OhlcvObject> items)
        {
            var dataOhlcv = items
                .Select(x => new OhlcvObject()
                {
                    Open = x.Open,
                    High = x.High,
                    Low = x.Low,
                    Close = x.Close,
                    Volume = x.Volume,
                })
                .Where(x => x.Open != 0 && x.High != 0 && x.Low != 0 && x.Close != 0)
                .ToList();

            var bullishBest = accuracyBest.Where(x => x.Contains("Bullish")).ToArray();
            var bearishBest = accuracyBest.Where(x => x.Contains("Bearish")).ToArray();
            var signalsBullishCountMulti = _signals.GetMultipleFiboSignalsCount(dataOhlcv, bullishBest);
            var signalsBearishCountMulti = _signals.GetMultipleFiboSignalsCount(dataOhlcv, bearishBest);

            return (signalsBullishCountMulti - signalsBearishCountMulti) * multiplier;
        }
    }
}
