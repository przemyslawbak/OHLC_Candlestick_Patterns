# OHLC_Candlestick_Patterns

## Purpose

The project was created due to the lack of a similar library for C#. It was preceded by the research for algorithmic formulas for individual OHLC patterns. As a result, you can test the OHLC candlestick list for 37 bullish and 37 bearish OHLC price action patterns. A list of patterns and an example of use can be found below.

NuGet package can be found here: https://www.nuget.org/packages/OHLC_Candlestick_Patterns

## Features

1. Counting bullish signals that appear in the OHLC list.
2. Counting bearish signals that appear in the OHLC list.
3. Counting the number of signals that appear in the OHLC list for selected multiple patterns.
4. Counting the number of signals that appear in the OHLC list for selected single pattern.
5. Counting the number of signals that appear in the OHLC list for selected formation.
6. Calculates the weighted index for the selected single pattern.
7. Calculates the weighted index for the selected multiple patterns.
8. Calculates signals for selected single pattern.
9. Calculates signals for selected multiply patterns.
10. Calculates signals for selected single formation.

## Technology

Solution is using:
  - C# language,
  - .NET 7.0 Standars Class Library,
  - .NET 7.0 Console project for the presentation of examples,
  - Newtonsoft.Json in Console project.

## Formation list

Bearish Double Tops
![Bearish Double Tops]()

Bearish Triple Tops

Bearish Head And Shoulders
Bearish Descending Triangle
Bearish Rising Wedge
Bearish Bear Flags Pennants
Bullish Double Bottoms
Bullish Triple Bottoms
Bullish Cup And Handle
Bullish Inverse Head And Shoulders
Bullish Ascending Triangle
Bullish Symmetric Triangle
Bullish Falling Wedge
Bullish Bull Flags Pennants
Bullish Ascending Price Channel
Bullish Descending Price Channel
Bullish Rounding Bottom Pattern
Continuation Diamond Formation

## Pattern list
| Bearish 2 Crows | Bearish 3 BlackCrows | Bearish 3 Inside Down | Bearish 3 Outside Down | Bearish 3 Line Strike | Bearish Advance Block | Bearish Belt Hold |
| :-------------: | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: |
| ![Bearish 2 Crows](https://github.com/user-attachments/assets/f2ffd966-328b-4401-bcc9-0a0f1723a681) | ![Bearish 3 BlackCrows](https://github.com/user-attachments/assets/5009a177-403a-4fa1-ad7b-44a3db56be5e) | ![Bearish 3 Inside Down](https://github.com/user-attachments/assets/1f6cd28d-f07d-4473-8efa-580fd2b2df7c) | ![Bearish 3 Outside Dow](https://github.com/user-attachments/assets/beaa8ff8-b9e7-4b9c-ada6-946bb338adc3) | ![Bearish 3 Line Strike](https://github.com/user-attachments/assets/4635e682-9d40-4ad2-803c-11ce13f51b2d) | ![Bearish Advance Block](https://github.com/user-attachments/assets/89399eec-a091-477d-a874-b2906e8c86f0) | ![Bearish Belt Hold](https://github.com/user-attachments/assets/a1794664-05c0-4d26-9ed0-fee74d383bce)

| Bearish Black Closing Marubozu | Bearish Black Marubozu | Bearish Black Opening Marubozu | Bearish Breakaway | Bearish Deliberation | Bearish Dark Cloud Cover | Bearish Doji Star |
| :-------------: | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: |
|![Bearish Black Closing Marubozu](https://github.com/user-attachments/assets/f0c881c4-4ed6-46e0-90b1-871731592d98)|![Bearish Black Marubozu](https://github.com/user-attachments/assets/b8954d91-080e-4f3a-948f-f5a79cb175f4)|![Bearish Black Opening Marubozu](https://github.com/user-attachments/assets/eb268e2a-b9e9-40ae-8244-9f5db83869fe)|![Bearish Breakaway](https://github.com/user-attachments/assets/5ca94f1e-d9c2-4dc2-b798-e11636e97183)|![Bearish Deliberation](https://github.com/user-attachments/assets/8dea0b19-4f7b-49db-924f-e5aea7c9025c)|![Bearish Dark Cloud Cover](https://github.com/user-attachments/assets/5459b34f-ae92-413c-8310-279687c2d9e8)|![Bearish Doji Star](https://github.com/user-attachments/assets/d541ee12-2f3e-4020-909a-72faa57d28d7)|
















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

## Example usage

The example of use is prepared for a console application in the .NET 7.0 framework using sample data placed in a file in GitHub Gist.

To use this package please install latest NuGet version from: https://www.nuget.org/packages/OHLC_Candlestick_Patterns

```cs
using Candlestick_Patterns;
using Newtonsoft.Json;

string json = string.Empty;
ISignals _signals = new Signals();
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

var formationsSignalsCountSingle = _signals.GetFormationSignalsCount(dataOhlcv, "BearishDoubleTops");

var bullishCount = _signals.GetBullishSignalsCount(dataOhlcv);
Console.WriteLine("Bullish signals count: {0}", bullishCount);
//Bullish signals count: 89

var bearishCount = _signals.GetBearishSignalsCount(dataOhlcv);
Console.WriteLine("Bearish signals count: {0}", bearishCount);
//Bearish signals count: 128

var signalsCountMulti = _signals.GetSignalsCount(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Multiple patterns signals count: {0}", signalsCountMulti);
//Multiple patterns signals count: 6

var signalsCountSingle = _signals.GetSignalsCount(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Single pattern signals count: {0}", signalsCountSingle);
//Single pattern signals count: 6

var signalsCountMultiWeightened = _signals.GetSignalsIndex(dataOhlcv, new Dictionary<string, decimal>() { { "Bearish Belt Hold", 0.5M }, { "Bearish Black Closing Marubozu", 0.5M } });
Console.WriteLine("Weightened index for selected multiple patterns: {0}", signalsCountMultiWeightened);
//Weightened index for selected multiple patterns: 3,0

var signalsCountSingleWeightened = _signals.GetSignalsIndex(dataOhlcv, "Bearish Black Closing Marubozu", 0.5M);
Console.WriteLine("Weightened index for selected single pattern: {0}", signalsCountSingleWeightened);
//Weightened index for selected single pattern: 3,0

var ohlcSingleSignals = _signals.GetOhlcvWithSignals(dataOhlcv, "Bearish Black Closing Marubozu");
Console.WriteLine("Signals for single pattern: {0}", ohlcSingleSignals.Where(x => x.Signal == true).Count());
//Signals for single pattern: 6

var ohlcMultiSignals = _signals.GetOhlcvWithSignals(dataOhlcv, new string[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });
Console.WriteLine("Number of lists returned: {0}", ohlcMultiSignals.Count());
//Number of lists returned: 2

Console.ReadLine();
```
  
## Production

16 Jul 2023
