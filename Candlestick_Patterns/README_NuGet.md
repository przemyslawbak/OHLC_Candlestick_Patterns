# OHLC_Candlestick_Patterns

## Updates history
- 2026 Jun - Added missing Enumerators.
- 2026 Mar - Licence changed to MIT.
- 2026 Jan - Mar - Performance speed increased by code optimisation.
- 2026 Jan - Mar - Charting improved.
- 2026 Feb - Mar - Examples bugs fixed.

## Purpose

The project was created due to the lack of a similar library for C#. It was preceded by the research for algorithmic formulas for individual OHLC patterns. As a result, you can test the OHLC candlestick list for:
- 37 bullish and 37 bearish candlestick patterns,
- 9 bullish, 9 bearish and additionally 2 continuation classic chart formations,
- 6 bullish, 6 bearish fibonacci chart patterns.

Additionally, you can test their effectiveness on the OHLCV data provided. A list of patterns, formations and an example of use can be found below.

The library is an analysis component, not a trading or investment recommendation. Validate results against your own market, timeframe, and execution rules.

NuGet package can be found here: https://www.nuget.org/packages/OHLC_Candlestick_Patterns

## Features

- Counting bullish or bearish signals that appear in the OHLC list.
- Gives number of signals that appear in the OHLC list for selected multiple, single patterns or formation.
- Computes weightened index for the selected multiple or single pattern.
- Calculating signals for selected multiply, single pattern or formation.
- You can test selected pattern / formation effectiveness on the data provided.

## Requirements and installation

- `.NET 8.0` or later.
- NuGet package: [OHLC_Candlestick_Patterns](https://www.nuget.org/packages/OHLC_Candlestick_Patterns).

Additionally in the solution:
- `Newtonsoft.Json` in Console presentation project.
- `ScottPlot` in Console presentation project.

```bash
dotnet add package OHLC_Candlestick_Patterns
```

The package exposes the `Candlestick_Patterns` namespace for signal analysis and `OHLC_Candlestick_Patterns` for accuracy analysis. `Newtonsoft.Json` is used by the model attributes and is installed as a package dependency.

# Patterns and Formations

## Fibonacci Patterns

[Complete list of supported Fibonacci patterns](docs/Fibonacci_list.md)

[Examples using of methods with Fibonacci patterns](docs/Fibonacci_examples.md)

## Chart Formations

[Complete list of supported chart Formations](docs/Formations_list.md)

[Examples using of methods with chart Formations](docs/Formations_examples.md)

## Candlestick Patterns

[Complete list of supported Candlestick Patterns](docs/Patterns_list.md)

## Quick start

```csharp
using Candlestick_Patterns;

var candles = new List<OhlcvObject>
{
	new() { Open = 100m, High = 105m, Low = 98m, Close = 104m, Volume = 1_000m },
	new() { Open = 104m, High = 106m, Low = 101m, Close = 102m, Volume = 1_200m },
	new() { Open = 102m, High = 108m, Low = 100m, Close = 107m, Volume = 1_500m }
};

ISignals signals = new Signals();

int bullish = signals.GetPatternsBullishSignalsCount(candles);
int bearish = signals.GetPatternsBearishSignalsCount(candles);
int engulfing = signals.GetPatternsSignalsCount(candles, "Bullish Engulfing");
List<OhlcvObject> marked = signals.GetPatternsOhlcvWithSignals(candles, "Bullish Engulfing");
```

Detection methods are synchronous and accept an in-memory `List<OhlcvObject>`. Load and validate data asynchronously at the application boundary, then pass the resulting list to the SDK.

## Input model: `OhlcvObject`

Each item represents one candle:

| Property | Type | Meaning |
| --- | --- | --- |
| `Open` | `decimal` | Opening price. |
| `High` | `decimal` | Highest price. |
| `Low` | `decimal` | Lowest price. |
| `Close` | `decimal` | Closing price. |
| `Volume` | `decimal` | Volume; used for OHLCV/accuracy workflows and available to consumers. |
| `Signal` | `bool` | Set on returned signal lists when that candle is a detected signal. |

Provide candles in chronological order. Ensure `High` is not below `Open`/`Close`, `Low` is not above them, and remove incomplete or zero-price records according to the source's data policy. The library does not provide timestamps, so keep timestamps in a parallel collection when they are needed by the application.

## Naming rules

The public API uses strings for pattern, formation, and Fibonacci names. Names are case-insensitive, leading/trailing whitespace is ignored, and spaces are ignored during lookup. For example, these forms resolve to the same candlestick method:

```csharp
"Bullish Engulfing"
" bullish engulfing "
"BullishEngulfing"
```

Prefer the spaced names shown in the documentation. Use the exact supported name rather than relying on the compatibility behavior for unknown names. An unknown or blank name produces the input data with no matching signal, so validate user-supplied names before analysis.

## Signal API

Instantiate `Signals` directly or through `ISignals`:

```csharp
ISignals signals = new Signals();
```

### Candlestick patterns

| Goal | Methods | Result |
| --- | --- | --- |
| Count all bullish/bearish signals | `GetPatternsBullishSignalsCount`, `GetPatternsBearishSignalsCount` | `int` |
| Count one or more named patterns | `GetPatternsSignalsCount`, `GetMultiplePatternsSignalsCount` | `int` |
| Calculate a weighted index | `GetPatternSignalsIndex`, `GetMultiplePatternsSignalsIndex` | `decimal` |
| Return marked candles | `GetPatternsOhlcvWithSignals`, `GetMultiplePatternsOhlcvWithSignals` | One `List<OhlcvObject>` or one list per requested name |

```csharp
var count = signals.GetMultiplePatternsSignalsCount(
	candles,
	new[] { "Bearish Belt Hold", "Bearish Black Closing Marubozu" });

var index = signals.GetMultiplePatternsSignalsIndex(
	candles,
	new Dictionary<string, decimal>
	{
		["Bearish Belt Hold"] = 0.5m,
		["Bearish Black Closing Marubozu"] = 0.5m
	});

var marked = signals.GetPatternsOhlcvWithSignals(candles, "Bearish Engulfing");
var signalCount = marked.Count(x => x.Signal);
```

The weighted index is the sum of each selected signal count multiplied by its supplied weight. Weights are not normalized automatically; normalize them in the application when that is required by your scoring model.

### Chart formations

| Goal | Methods | Result |
| --- | --- | --- |
| Count all bullish/bearish signals | `GetFormationsBullishSignalsCount`, `GetFormationsBearishSignalsCount` | `int` |
| Count one or more formations | `GetFormationSignalsCount`, `GetMultipleFormationsSignalsCount` | `int` |
| Calculate a weighted index | `GetFormationSignalsIndex`, `GetMultipleFormationsSignalsIndex` | `decimal` |
| Return marked zigzag points | `GetFormationsZigZagWithSignals`, `GetMultipleFormationsZigZagWithSignals` | One `List<ZigZagObject>` or one list per requested name |

### Fibonacci patterns

| Goal | Methods | Result |
| --- | --- | --- |
| Count all bullish/bearish signals | `GetFiboBullishSignalsCount`, `GetFiboBearishSignalsCount` | `int` |
| Count one or more Fibonacci patterns | `GetFibonacciSignalsCount`, `GetFiboSignalsCount`, `GetMultipleFiboSignalsCount` | `int` |
| Calculate a weighted index | `GetFiboSignalsIndex`, `GetMultipleFiboSignalsIndex` | `decimal` |
| Return marked zigzag points | `GetFiboZigZagWithSignals`, `GetMultipleFiboZigZagWithSignals` | One `List<ZigZagObject>` or one list per requested name |

`GetFibonacciSignalsCount` and `GetFiboSignalsCount` are compatibility aliases for the single Fibonacci count operation. The `Fibo` spelling is also used by the remaining Fibonacci methods.

## Zigzag output model: `ZigZagObject`

Formation and Fibonacci methods return points rather than full candles:

| Property | Type | Meaning |
| --- | --- | --- |
| `Close` | `decimal` | Close value at the zigzag point. |
| `IndexOHLCV` | `int` | Index of the related input candle. |
| `Signal` | `bool` | Indicates a detected signal at the point. |
| `Initiation` | `bool` | Indicates the formation/Fibonacci initiation point. |

For multi-name methods, returned lists follow the order of the input names. An empty name result is represented by an empty list in the multi-result APIs.

## Accuracy trials

Use `AccuracyTrials` through `IAccuracyTrials`:

```csharp
using OHLC_Candlestick_Patterns;

IAccuracyTrials accuracy = new AccuracyTrials();
AccuracyObject result = accuracy.GetAverPercentPatternAccuracy(
	candles,
	"Bullish Engulfing",
	candlesAheadQty: 30);

Console.WriteLine(result.AccuracyToAverageClose);
Console.WriteLine(result.AccuracyToEndClose);
```

`AccuracyObject` contains:

- `AccuracyToAverageClose`: comparison with average close prices after a signal.
- `AccuracyToEndClose`: comparison with the final close in the supplied evaluation range.

Positive values indicate movement in line with the detected direction; negative values indicate movement against it. These are historical measurements, not guarantees of future performance.

### Accuracy method groups

| Analysis | Selected item | Positive results | Best results |
| --- | --- | --- | --- |
| Candlestick pattern | `GetAverPercentPatternAccuracy(data, name)` and the overload with `candlesAheadQty` | `GetPositiveAccuracyToAverPatterns`, `GetPositiveAccuracyToEndPatterns` | `GetBestAccuracyPatterns(data, topPercentage)` and the overload with `candlesAhead` |
| Chart formation | `GetAverPercentFormationAccuracy(data, name)` and the overload with `candlesAheadQty` | `GetPositiveAccuracyToEndFormations`; `GetPositiveAccuracyToAverFibos` is a legacy misnamed formation method | `GetBestAccuracyFormations(data, topPercentage)` and the overload with `candlesAhead` |
| Fibonacci pattern | `GetAverPercentAccuracy(data, name)` and the overload with `candlesAheadQty` | `GetPositiveAccuracyToAverFibo`, `GetPositiveAccuracyToEndFibos` | `GetBestAccuracyFibos(data, topPercentage)` and the overload with `candlesAhead` |

`topPercentage` selects the best percentage of the available names. `candlesAheadQty`/`candlesAhead` changes the evaluation horizon; use a positive value appropriate for the data window and avoid interpreting a short horizon as a long-term result.

## Supported names and examples

- [Candlestick pattern list](docs/Patterns_list.md) and [pattern examples](docs/Patterns_examples.md).
- [Chart formation list](docs/Formations_list.md) and [formation examples](docs/Formations_examples.md).
- [Fibonacci pattern list](docs/Fibonacci_list.md) and [Fibonacci examples](docs/Fibonacci_examples.md).

Common names include `Bullish Engulfing`, `Bearish Dark Cloud Cover`, `Bullish Double Bottoms`, `Bearish Head And Shoulders`, `Bullish Gartley`, and `Bearish ABCD`. The detailed lists are the source of truth for all supported names.

## Data loading guidance

The SDK does not download market data. Deserialize your provider's JSON/CSV response into `OhlcvObject` values, check for missing or invalid prices, and preserve chronological order before calling analysis methods. The repository examples use Newtonsoft.Json and a public sample data URL, but production applications should use their own authenticated data provider and appropriate HTTP timeouts.

## Repository examples

The solution includes console examples for patterns, formations, and Fibonacci analysis in `Examples_Patterns`, `Examples_Formations`, and `Examples_Fibonacci`. They demonstrate data loading, accuracy trials, counts, indexes, and marked outputs.


## License

This project is licensed under the MIT License. See the repository `LICENSE` file.
