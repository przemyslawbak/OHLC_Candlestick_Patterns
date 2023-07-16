# OHLC_Candlestick_Patterns

## Purpose

The project was created due to the lack of a similar library for C#. It was preceded by the research for algorithmic formulas for individual OHLC patterns. As a result, you can test the OHLC candlestick list for 37 bullish and 37 bearish OHLC price action patterns. A list of patterns and an example of use can be found below.

## Features

1. Counting bullish signals that appear in the OHLC list.
2. Counting bearish signals that appear in the OHLC list.
3. Counting the number of signals that appear in the OHLC list for selected multiple patterns.
4. Counting the number of signals that appear in the OHLC list for selected single pattern.
5. Calculates the weighted index for the selected single pattern.
6. Calculates the weighted index for the selected multiple patterns.
7. Calculates signals for selected single pattern.
8. Calculates signals for selected multiply patterns.

## Technology

2. Solution is using:
  - C# language,
  - .NET 7.0 Standars Class Library,
  - .NET 7.0 Console project for the presentation of examples,
  - XUnit for unit tests.
  - Newtonsoft.Json in Console project

## Example usage

The example of use is prepared for a console application in the .NET 7.0 framework using sample data placed in a file in GitHub Gist.
```
using Candlestick_Patterns;
using Newtonsoft.Json;

string json = string.Empty;
ISignals _signals = new Signals();
var client = new HttpClient();
var url = "https://gist.githubusercontent.com/przemyslawbak/2058d9aeddfe09d2a26da81dfc16e5d0/raw/json_data_sample.txt";

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

var bullishCount = _signals.GetBullishSignalsCount(dataOhlcv);
Console.WriteLine("Bullish signals count: {0}", bullishCount); //Bullish signals count: 89

var bearishCount = _signals.GetBearishSignalsCount(dataOhlcv);
Console.WriteLine("Bearish signals count: {0}", bearishCount); //Bearish signals count: 128

var signalsCountMulti = _signals.GetSignalsCount(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Multiple patterns signals count: {0}", signalsCountMulti); //Multiple patterns signals count: 6

var signalsCountSingle = _signals.GetSignalsCount(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Single pattern signals count: {0}", signalsCountSingle); //Single pattern signals count: 6

var signalsCountMultiWeightened = _signals.GetSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Belt Hold", 0.5M }, { "Bearish Black Closing Marubozu", 0.5M } });
Console.WriteLine("Weightened index for selected multiple patterns: {0}", signalsCountMultiWeightened); //Weightened index for selected multiple patterns: 3,0

var signalsCountSingleWeightened = _signals.GetSignalsIndex(dataOhlcv, "Bearish Black Closing Marubozu", 0.5M);
Console.WriteLine("Weightened index for selected single pattern: {0}", signalsCountSingleWeightened); //Weightened index for selected single pattern: 3,0

var ohlcSingleSignals = _signals.GetOhlcvWithSignals(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Signals for single pattern: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count()); //Signals for single pattern: 6

var ohlcMultiSignals = _signals.GetOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count()); //Number of lists returned: 2

Console.ReadLine();
```
## Pattern list

Bearish 2 Crows,
Bearish 3 BlackCrows,
Bearish 3 Inside Down,
Bearish 3 Outside Down,
Bearish 3 Line Strike,
Bearish Advance Block,
Bearish Belt Hold,
Bearish Black Closing Marubozu,
Bearish Black Marubozu,
Bearish Black Opening Marubozu,
Bearish Breakaway,
Bearish Deliberation,
Bearish Dark Cloud Cover,
Bearish Doji Star,
Bearish Downside Gap 3 Methods,
Bearish Downside Tasuki Gap,
Bearish Dragonfly Doji,
Bearish Engulfing,
Bearish Evening Doji Star,
Bearish Evening Star,
Bearish Falling 3 Methods,
Bearish Gravestone Doji,
Bearish Harami,
Bearish Identical 3 Crows,
Bearish Harami Cross,
Bearish In Neck,
Bearish Kicking,
Bearish Long Black Candelstick,
Bearish Meeting Lines,
Bearish On Neck,
Bearish Separating Lines,
Bearish Shooting Star,
Bearish Side By Side White Lines,
Bearish Thrusting,
Bearish Tri Star,
Bearish Tweezer Top,
Bearish Upside Gap 2 Crows,
Bullish 3 Inside Up,
Bullish 3 Outside Up,
Bullish 3 Starsinthe South,
Bullish 3 White Soldiers,
Bullish 3 Line Strike,
Bullish Belt Hold,
Bullish Breakaway,
Bullish Concealing Baby Swallow,
Bullish Doji Star,
Bullish Dragonfly Doji,
Bullish Engulfing,
Bullish Gravestone Doji,
Bullish Harami,
Bullish Harami Cross,
Bullish Homing Pigeon,
Bullish Inverted Hammer,
Bullish Kicking,
Bullish Ladder Bottom,
Bullish Long White Candlestick,
Bullish Mat Hold,
Bullish Matching Low,
Bullish Meeting Lines,
Bullish Morning Doji Star,
Bullish Morning Star,
Bullish Piercing Line,
Bullish Rising 3 Methods,
Bullish Separating Lines,
Bullish Side By Side White Lines,
Bullish Stick Sandwich,
Bullish Tri Star,
Bullish Tweezer Bottom,
Bullish Unique 3 River Bottom,
Bullish Upside Gap 3 Methods,
Bullish Upside Tasuki Gap,
Bullish White Closing Marubozu,
Bullish White Marubozu,
Bullish White Opening Marubozu
  
## Production

16 Jul 2023
