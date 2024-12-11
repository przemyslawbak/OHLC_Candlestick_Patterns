## Examples Candlestick Patterns

```cs
string json = string.Empty;
ISignals _signals = new Signals();
IAccuracyTrials _accuracy = new AccuracyTrials();
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
    MissingMemberHandling = MissingMemberHandling.Ignore,
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
var accuracyPercentageSummary = _accuracy.GetAverPercentPatternAccuracy(dataOhlcv, "Bullish 3 Inside Up");
Console.WriteLine("Accuracy percentage summary comparing to end of data set result: {0}", accuracyPercentageSummary.AccuracyToEndClose);
Console.WriteLine("Accuracy percentage summary comparing to average close result: {0}", accuracyPercentageSummary.AccuracyToAverageClose);

var accuracyForSelectedPattern30CandlesAhead = _accuracy.GetAverPercentPatternAccuracy(dataOhlcv, "Bullish 3 Inside Up", 30);
Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to end of data set result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToEndClose);
Console.WriteLine("Accuracy percentage summary 30 candles ahead comparing to average close result: {0}", accuracyForSelectedPattern30CandlesAhead.AccuracyToAverageClose);

var accuracyAverPositive = _accuracy.GetPositiveAccuracyToAverPatterns(dataOhlcv);
Console.WriteLine("Patterns with positive accuracy rate comaring to aver. close price: {0}", string.Join(",", accuracyAverPositive));

var accuracyEndPositive = _accuracy.GetPositiveAccuracyToEndPatterns(dataOhlcv);
Console.WriteLine("Patterns with positive accuracy rate comaring to end close price: {0}", string.Join(",", accuracyEndPositive));

var accuracyBest = _accuracy.GetBestAccuracyPatterns(dataOhlcv, 25);
Console.WriteLine("25% of best patterns comparing to end and aver. close price: {0}", string.Join(",", accuracyBest));

var accuracyBest30CandlesAhead = _accuracy.GetBestAccuracyPatterns(dataOhlcv, 25, 30);
Console.WriteLine("25% of best patterns 30 candles ahead comparing to end and aver. close price: {0}", string.Join(",", accuracyBest30CandlesAhead));

//SIGNALS
var bullishCount = _signals.GetPatternsBullishSignalsCount(dataOhlcv);
Console.WriteLine("Bullish signals count: {0}", bullishCount);

var bearishCount = _signals.GetPatternsBearishSignalsCount(dataOhlcv);
Console.WriteLine("Bearish signals count: {0}", bearishCount);

var signalsCountMulti = _signals.GetMultiplePatternsSignalsCount(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Multiple patterns signals count: {0}", signalsCountMulti);

var signalsCountSingle = _signals.GetPatternsSignalsCount(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Single pattern signals count: {0}", signalsCountSingle);

var signalsCountMultiWeightened = _signals.GetMultiplePatternsSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Belt Hold", 0.5M }, { "Bearish Black Closing Marubozu", 0.5M } });
Console.WriteLine("Weightened index for selected multiple patterns: {0}", signalsCountMultiWeightened);

var signalsCountSingleWeightened = _signals.GetPatternSignalsIndex(dataOhlcv, "Bearish Black Closing Marubozu", 0.5M);
Console.WriteLine("Weightened index for selected single pattern: {0}", signalsCountSingleWeightened);

var ohlcSingleSignals = _signals.GetPatternsOhlcvWithSignals(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Signals for single pattern: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count());

var ohlcMultiSignals = _signals.GetMultiplePatternsOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count());

//END
Console.WriteLine("END");
Console.ReadLine();
```
