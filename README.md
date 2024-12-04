# OHLC_Candlestick_Patterns

## Purpose

The project was created due to the lack of a similar library for C#. It was preceded by the research for algorithmic formulas for individual OHLC patterns. As a result, you can test the OHLC candlestick list for:
- 37 bullish and 37 bearish candlestick patterns,
- 9 bullish, 9 bearish and additionally 2 continuation classic chart formations,
- 6 bullish, 6 bearish fibonacci chart patterns.

Additionally, you can test their effectiveness on the OHLCV data provided. A list of patterns, formations and an example of use can be found below.

NuGet package can be found here: https://www.nuget.org/packages/OHLC_Candlestick_Patterns

## Features

- Counting bullish or bearish signals that appear in the OHLC list.
- Gives number of signals that appear in the OHLC list for selected multiple, single patterns or formation.
- Computes weightened index for the selected multiple or single pattern.
- Calculating signals for selected multiply, single pattern or formation.
- You can test selected pattern / formation effectiveness on the data provided.

## Technology

Library is using:
  - C# language,
  - .NET 7.0 Standars Class Library.

Additionally in the solution:
  - Newtonsoft.Json in Console presentation project.
  - ScottPlot in Console presentation project.

## Fibonacci Patterns

[Complete list of supported Fibonacci patterns](docs/Fibonacci_list.md)

[Examples using of methods with Fibonacci patterns](docs/Fibonacci_examples.md)

## Chart Formations

[Complete list of supported chart Formations](docs/Formations_list.md)

[Examples using of methods with chart Formations](docs/Formations_examples.md)

## Candlestick Patterns

[Complete list of supported Candlestick Patterns](docs/Patterns_list.md)

[Examples using of methods with Candlestick Patterns](docs/Patterns_examples.md)
