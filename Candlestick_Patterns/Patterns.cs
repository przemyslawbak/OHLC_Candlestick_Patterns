using Enumerators;
using System.Collections.Concurrent;
using System.Reflection;
namespace Candlestick_Patterns
{
    public class Patterns : IPatterns, ISignalEngine
    {

        private sealed class CandleThresholds
        {
            public decimal insideBarMaxChange;
            public decimal minimumCandleSize;
            public decimal maximumShortCandleSize;
            public decimal minimumCandleDifference;
            public decimal maximumDojiShadowSizes;
            public decimal maximumDojiBodySize;
            public decimal minimumCandleShadowSize;
            public decimal maximumShadowSize;
            public decimal maximumPriceDifference;
            public decimal maximumCloseDifference;
            public decimal maximumCandleBodySize;
            public decimal maximumCandleShadowSize;
            public decimal maximumDifference;
            public decimal maximumShadowChange;
            public decimal maximumCloseHighChange;
            public decimal maximumCandleSize;
            public decimal maximumOpenDifference;
        }

        private readonly ThreadLocal<CandleThresholds> _threadThresholds =
            new(() => new CandleThresholds());

        private decimal _insideBarMaxChange => _threadThresholds.Value!.insideBarMaxChange;
        private decimal _minimumCandleSize => _threadThresholds.Value!.minimumCandleSize;
        private decimal _maximumShortCandleSize => _threadThresholds.Value!.maximumShortCandleSize;
        private decimal _minimumCandleDifference => _threadThresholds.Value!.minimumCandleDifference;
        private decimal _maximumDojiShadowSizes => _threadThresholds.Value!.maximumDojiShadowSizes;
        private decimal _maximumDojiBodySize => _threadThresholds.Value!.maximumDojiBodySize;
        private decimal _minimumCandleShadowSize => _threadThresholds.Value!.minimumCandleShadowSize;
        private decimal _maximumShadowSize => _threadThresholds.Value!.maximumShadowSize;
        private decimal _maximumPriceDifference => _threadThresholds.Value!.maximumPriceDifference;
        private decimal _maximumCloseDifference => _threadThresholds.Value!.maximumCloseDifference;
        private decimal _maximumCandleBodySize => _threadThresholds.Value!.maximumCandleBodySize;
        private decimal _maximumCandleShadowSize => _threadThresholds.Value!.maximumCandleShadowSize;
        private decimal _maximumDifference => _threadThresholds.Value!.maximumDifference;
        private decimal _maximumShadowChange => _threadThresholds.Value!.maximumShadowChange;
        private decimal _maximumCloseHighChange => _threadThresholds.Value!.maximumCloseHighChange;
        private decimal _maximumCandleSize => _threadThresholds.Value!.maximumCandleSize;
        private decimal _maximumOpenDifference => _threadThresholds.Value!.maximumOpenDifference;

        private readonly List<OhlcvObject> _data;
        private static List<string>? _cachedMethodNames;
        private static readonly object _methodNamesLock = new object();
        private static readonly ConcurrentDictionary<string, MethodInfo> _methodCache = new();

        [AttributeUsage(AttributeTargets.Method)]
        public class PatternMethodAttribute : Attribute
        {
            public PatternType Type { get; }
            public int LookbackPeriod { get; }

            public PatternMethodAttribute(PatternType type, int lookbackPeriod = 5)
            {
                Type = type;
                LookbackPeriod = lookbackPeriod;
            }
        }


        public Patterns(List<OhlcvObject> data)
        {
            ArgumentNullException.ThrowIfNull(data);
            _data = data.Select(CopyWithoutSignal).ToList();
        }

        private void UpdateThresholds(int index)
        {
            // A trailing window prevents future candles from changing past signals.
            const int windowSize = 20;
            int first = Math.Max(0, index - windowSize + 1);
            decimal totalBodySize = 0M;
            for (int j = first; j <= index; j++)
                totalBodySize += Math.Abs(GetBodySizePercentSigned(_data[j]));

            decimal average = totalBodySize / (index - first + 1);
            var thresholds = _threadThresholds.Value!;
            thresholds.insideBarMaxChange = average / 2M;
            thresholds.minimumCandleSize = average * 2M;
            thresholds.maximumShortCandleSize = average / 2M;
            thresholds.minimumCandleDifference = average / 2M;
            thresholds.maximumDojiShadowSizes = average / 2M;
            thresholds.maximumDojiBodySize = average / 2M;
            thresholds.minimumCandleShadowSize = average * 2M;
            thresholds.maximumShadowSize = average * 3M;
            thresholds.maximumPriceDifference = average * 3M;
            thresholds.maximumCloseDifference = average * 3M;
            thresholds.maximumCandleBodySize = average / 2M;
            thresholds.maximumCandleShadowSize = average / 2M;
            thresholds.maximumDifference = average / 2M;
            thresholds.maximumShadowChange = average / 2M;
            thresholds.maximumCloseHighChange = average * 3M;
            thresholds.maximumCandleSize = average / 2M;
            thresholds.maximumOpenDifference = average * 2M;
        }

        private bool Is2Crows(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c1.Low > c2.High)
                {
                    if (c0.Open > c0.Close && c1.Close <= c0.Open && c0.Open <= c1.Open && c2.Open <= c0.Close && c0.Close <= c2.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bearish2Crows()
        {
            return DetectPattern(Is2Crows, lookbackPeriod: 2);
        }

        private bool Is3BlackCrows(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c1.Close < c2.Close && c2.Close <= c1.Open && c1.Open <= c2.Open && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open > c0.Close && c1.Close < c0.Open && c0.Open < c1.Open && c0.Close < c1.Close && -GetBodySizePercentSigned(c0) > _minimumCandleSize)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bearish3BlackCrows()
        {
            return DetectPattern(Is3BlackCrows, lookbackPeriod: 2);
        }

        private bool IsInsideDown(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c1.Open < c2.Close && c1.Close > c2.Open && -GetBodySizePercentSigned(c1) < _maximumShortCandleSize)
                {
                    if (c0.Open > c0.Close && c0.Close < c1.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bearish3InsideDown()
        {
            return DetectPattern(IsInsideDown, lookbackPeriod: 2);
        }

        private bool Is3OutsideDown(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) < _maximumShortCandleSize)
            {
                if (c1.Open > c1.Close && c1.Open > c2.Close && c1.Close < c2.Open && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open > c0.Close && c0.Close < c1.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bearish3OutsideDown()
        {
            return DetectPattern(Is3OutsideDown, lookbackPeriod: 2);
        }
        private bool Is3LineStrike(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];

            if (c3.Open > c3.Close && -GetBodySizePercentSigned(c3) > _minimumCandleSize)
            {
                if (c2.Open > c2.Close && c3.Close < c2.Open && c2.Open < c3.Open && c2.Close < c3.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
                {
                    if (c1.Open > c1.Close && c2.Close < c1.Open && c1.Open < c2.Open && c1.Close < c2.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
                    {
                        if (c0.Open < c0.Close && c0.Open <= c1.Close && c0.Close >= c3.Open)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 4)]
        private List<OhlcvObject> Bearish3LineStrike()
        {
            return DetectPattern(Is3LineStrike, lookbackPeriod: 3);
        }

        private bool IsAdvanceBlock(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open < c1.Close && c2.Close > c1.Open && c1.Open > c2.Open && c1.Close > c2.Close && ((100 + _minimumCandleDifference) / 100) * (c1.Close - c1.Open) < c2.Close - c2.Open && GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close > c0.Open && c0.Open > c1.Open && c0.Close > c1.Close && ((100 + _minimumCandleDifference) / 100) * (c0.Close - c0.Open) < c1.Close - c1.Open && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishAdvanceBlock()
        {
            return DetectPattern(IsAdvanceBlock, lookbackPeriod: 2);
        }

        private bool IsBeltHold2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.High < c0.Low)
            {
                if (c0.Open == c0.High && c0.Open > c0.Close && -GetBodySizePercentSigned(c0) > _minimumCandleSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishBeltHold()
        {
            return DetectPattern(IsBeltHold2, lookbackPeriod: 1);
        }

        private bool IsBlackClosingMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High > candle.Open && candle.Close == candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 1)]
        private List<OhlcvObject> BearishBlackClosingMarubozu()
        {
            return DetectPattern(IsBlackClosingMarubozu, lookbackPeriod: 0);
        }

        private bool IsBlackMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High == candle.Open && candle.Close == candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 1)]
        private List<OhlcvObject> BearishBlackMarubozu()
        {
            return DetectPattern(IsBlackMarubozu, lookbackPeriod: 0);
        }
        private bool IsBlackOpeningMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High == candle.Open && candle.Close > candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 1)]
        private List<OhlcvObject> BearishBlackOpeningMarubozu()
        {
            return DetectPattern(IsBlackOpeningMarubozu, lookbackPeriod: 0);
        }

        private bool IsBreakaway(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open < c4.Close && GetBodySizePercentSigned(c4) > _minimumCandleSize)
            {
                if (c3.Open < c3.Close && c4.High < c3.Low && 100 * (c3.Close - c3.Open) / c3.Open < _minimumCandleSize)
                {
                    if (c3.Close < c2.Close && Math.Abs((100 * (c2.Close - c2.Open) / c2.Open)) < _minimumCandleSize)
                    {
                        if (c1.Open < c1.Close && c2.Close < c1.Close && GetBodySizePercentSigned(c1) < _minimumCandleSize)
                        {
                            if (c0.Open > c0.Close && c0.Close > c4.High && c0.Close < c3.Low)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private List<OhlcvObject> BearishBreakaway()
        {
            return DetectPattern(IsBreakaway, lookbackPeriod: 4);
        }

        private bool IsDeliberation(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && 100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
            {
                if (c1.Open < c1.Close && c2.Open < c1.Open && c2.Close < c1.Close && GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close <= c0.Open && GetBodySizePercentSigned(c0) < _minimumCandleSize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishDeliberation()
        {
            return DetectPattern(IsDeliberation, lookbackPeriod: 2);
        }

        private bool IsDarkCloudCover(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open > c1.High && c0.Close > c1.Open && c0.Close < c1.Open + ((c1.Close - c1.Open) / 2))
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishDarkCloudCover()
        {
            return DetectPattern(IsDarkCloudCover, lookbackPeriod: 1);
        }

        private bool IsDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c1.High < c0.Low && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) < _maximumDojiShadowSizes)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishDojiStar()
        {
            return DetectPattern(IsDojiStar, lookbackPeriod: 1);
        }

        private bool IsDownsideGap3Methods(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c2.Low > c1.High && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close < c0.Open && c1.Open > c0.Open && c2.Close < c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishDownsideGap3Methods()
        {
            return DetectPattern(IsDownsideGap3Methods, lookbackPeriod: 2);
        }

        private bool IsDownsideTasukiGap(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c2.Low > c1.High && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close < c0.Open && c1.Open > c0.Open && c2.Low > c0.Close && c1.High < c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishDownsideTasukiGap()
        {
            return DetectPattern(IsDownsideTasukiGap, lookbackPeriod: 2);
        }

        private bool IsDragonflyDoji(int index)
        {
            var c0 = _data[index - 0];

            return (Math.Abs(100 * (c0.High - c0.Close) / c0.High) < _maximumDojiBodySize && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) > _minimumCandleShadowSize);
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 1)]
        private List<OhlcvObject> BearishDragonflyDoji()
        {
            return DetectPattern(IsDragonflyDoji, lookbackPeriod: 0);
        }

        private bool IsEngulfing(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (100 * (c1.Close - c1.Open) / c1.Open < _maximumShortCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open > c1.Close && c0.Close < c1.Open && (-GetBodySizePercentSigned(c0) > _minimumCandleSize))
                {
                    return true;
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishEngulfing()
        {
            return DetectPattern(IsEngulfing, lookbackPeriod: 1);
        }

        private bool IsEveningDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c2.High < c1.Low && Math.Abs(GetBodySizePercentSigned(c1)) < _maximumDojiBodySize && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _maximumDojiShadowSizes)
                {
                    if (c0.Open > c0.Close && c0.Close > c2.Open && c0.Close < (c2.Open + c2.Close) / 2M && -100 * (c0.Close - c0.Open) / c0.Open > _minimumCandleSize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishEveningDojiStar()
        {
            return DetectPattern(IsEveningDojiStar, lookbackPeriod: 2);
        }

        private bool IsEveningStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c2.High < c1.Low && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _minimumCandleSize)
                {
                    if (c0.Open > c0.Close && c0.Close > c2.Open && c0.Close < (c2.Open + c2.Close) / 2M)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishEveningStar()
        {
            return DetectPattern(IsEveningStar, lookbackPeriod: 2);
        }

        private bool IsFalling3Methods(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open > c4.Close && (-GetBodySizePercentSigned(c4) > _minimumCandleSize))
            {
                if (c3.Open < c3.Close && c3.Close < c4.Open && c4.Close < c3.Close)
                {
                    if ((c2.Open < c2.Close && c2.Close < c4.Open && c3.Close < c2.Close) || (c2.Open > c2.Close && c2.Open < c4.Open && c3.Close < c2.Open))
                    {
                        if (c1.Open < c1.Close && c1.Close < c4.Open && Math.Max(c2.Close, c2.Open) < c1.Close)
                        {
                            if (c0.Open > c0.Close && c0.Close < c4.Close && (-GetBodySizePercentSigned(c0) > _minimumCandleSize))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private List<OhlcvObject> BearishFalling3Methods()
        {
            return DetectPattern(IsFalling3Methods, lookbackPeriod: 4);
        }

        private bool IsGravestoneDoji(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
            {
                if (c1.High < c0.Low && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && c0.Low == c0.Open)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishGravestoneDoji()
        {
            return DetectPattern(IsGravestoneDoji, lookbackPeriod: 1);
        }

        private bool IsHarami(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open < c1.Close && c0.Close > c1.Open && (-GetBodySizePercentSigned(c0) < _maximumShortCandleSize))
                {
                    return true;
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishHarami()
        {
            return DetectPattern(IsHarami, lookbackPeriod: 1);
        }

        private bool IsIdentical3Crows(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && (GetBodySizePercentSigned(c2) < -1 * _minimumCandleSize))
            {
                if (c1.Open > c1.Close && c2.Close == c1.Open && (GetBodySizePercentSigned(c1) < -1 * _minimumCandleSize))
                {
                    if (c0.Open > c0.Close && c1.Close == c0.Open && (GetBodySizePercentSigned(c0) < -1 * _minimumCandleSize))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishIdentical3Crows()
        {
            return DetectPattern(IsIdentical3Crows, lookbackPeriod: 2);
        }

        private bool IsHaramiCross(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && c0.Open < c1.Close && c1.Open < c0.Open)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishHaramiCross()
        {
            return DetectPattern(IsHaramiCross, lookbackPeriod: 1);
        }

        private bool IsInNeck(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize))
            {
                if (c0.Open < c0.Close && c0.Open < c1.Low && c0.Close > c1.Close && 100 * (c0.Close - c1.Close) / c1.Close < _insideBarMaxChange)
                {
                    return true;
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishInNeck()
        {
            return DetectPattern(IsInNeck, lookbackPeriod: 1);
        }

        private bool IsKicking(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && 100 * (c1.Open - c1.Low) / c1.Open < _maximumShadowSize && 100 * (c1.High - c1.Close) / c1.Close < _maximumShadowSize && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open < c1.Open && c0.Open > c0.Close && 100 * (c0.Close - c0.Low) / c0.Close < _maximumShadowSize && 100 * (c0.High - c0.Open) / c0.Open < _maximumShadowSize && (-GetBodySizePercentSigned(c0) > _minimumCandleSize))
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishKicking()
        {
            return DetectPattern(IsKicking, lookbackPeriod: 1);
        }

        private bool IsLongBlackCandelstick(int index)
        {
            var candle = _data[index];

            return (candle.Open > candle.Close && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 1)]
        private List<OhlcvObject> BearishLongBlackCandelstick()
        {
            return DetectPattern(IsLongBlackCandelstick, lookbackPeriod: 0);
        }

        private bool IsMeetingLines(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open > c1.High && (100 * (c0.Open - c0.Close) / c0.Close > _minimumCandleSize) && 100 * Math.Abs((c0.Close - c1.Close) / c1.Close) < _maximumCloseDifference)
                {
                    return true;
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishMeetingLines()
        {
            return DetectPattern(IsMeetingLines, lookbackPeriod: 1);
        }

        private bool IsOnNeck(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open < c0.Close && c0.Open < c1.Low && Math.Abs(100 * (c0.Close - c1.Low) / c0.Close) < _maximumPriceDifference)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishOnNeck()
        {
            return DetectPattern(IsOnNeck, lookbackPeriod: 1);
        }

        private bool IsSeparatingLines(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open > c0.Close && 100 * Math.Abs((c0.Open - c1.Open) / c1.Open) < _maximumCloseDifference)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishSeparatingLines()
        {
            return DetectPattern(IsSeparatingLines, lookbackPeriod: 1);
        }

        private bool IsShootingStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close)
            {
                if (c1.High < c0.Low && 3 * Math.Abs(c0.Open - c0.Close) < c0.High - c0.Low && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumCandleBodySize && 100 * (Math.Min(c0.Open, c0.Close) - c0.Low) / Math.Min(c0.Open, c0.Close) < _maximumCandleShadowSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishShootingStar()
        {
            return DetectPattern(IsShootingStar, lookbackPeriod: 1);
        }

        private bool IsSideBySideWhiteLines(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (_data[i - 2].Open > _data[i - 2].Close)
            {
                if (c1.Open < c1.Close && c1.High < _data[i - 2].Low)
                {
                    if (c0.Open < c0.Close && c0.High < _data[i - 2].Low && Math.Abs(100 * (c0.Open - c1.Open) / c0.Open) < _maximumDifference && Math.Abs(100 * (c0.Close - c1.Close) / c0.Close) < _maximumDifference)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishSideBySideWhiteLines()
        {
            return DetectPattern(IsSideBySideWhiteLines, lookbackPeriod: 2);
        }

        private bool IsThrusting(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close)
            {
                if (c0.Open < c0.Close && c0.Open < c1.Close && c0.Close > c1.Close && c0.Close < c1.Close + (c1.Open - c1.Close) / 2)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishThrusting()
        {
            return DetectPattern(IsThrusting, lookbackPeriod: 1);
        }

        private bool IsTriStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (Math.Abs(GetBodySizePercentSigned(c2)) < _maximumDojiBodySize)
            {
                if (Math.Abs(GetBodySizePercentSigned(c1)) < _maximumDojiBodySize && c2.High < c1.Low && c0.High < c1.Low)
                {
                    if ((Math.Abs(100 * (c0.Open - c0.Close) / c0.Open) < _maximumDojiBodySize))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishTriStar()
        {
            return DetectPattern(IsTriStar, lookbackPeriod: 2);
        }

        private bool IsTweezerTop(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open >= c0.Close && c0.High == c1.High && (Math.Abs(GetBodySizePercentSigned(c0)) < _maximumShortCandleSize))
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 2)]
        private List<OhlcvObject> BearishTweezerTop()
        {
            return DetectPattern(IsTweezerTop, lookbackPeriod: 1);
        }

        private bool IsUpsideGap2Crows(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c2.High < c1.Low && c1.Open > c1.Close)
                {
                    if (c0.Open > c0.Close && c0.Open > c1.Open && c0.Close < c1.Close && c0.Close > c2.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 3)]
        private List<OhlcvObject> BearishUpsideGap2Crows()
        {
            return DetectPattern(IsUpsideGap2Crows, lookbackPeriod: 2);
        }
        private bool Is3InsideUp(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && (-100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize))
            {
                if (c1.Open < c1.Close && c1.Open > c2.Close && c1.Close < c2.Open && (100 * (c1.Close - c1.Open) / c1.Open < _maximumShortCandleSize))
                {
                    if ((c0.Open < c0.Close && c0.Close > c1.Close))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bullish3InsideUp()
        {
            return DetectPattern(Is3InsideUp, lookbackPeriod: 2);
        }
        private bool Is3OutsideUp(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) < _maximumShortCandleSize)
            {
                if (c1.Open < c1.Close && c1.Open < c2.Close && c1.Close > c2.Open && GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c0.Close > c1.Close)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bullish3OutsideUp()
        {
            return DetectPattern(Is3OutsideUp, lookbackPeriod: 2);
        }

        private bool Is3StarsintheSouth(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && 100 * (c2.High - c2.Open) / c2.Open < _maximumShadowChange && 100 * (c2.Close - c2.Low) / c2.Low > _minimumCandleSize && (-100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize))
            {
                if (c1.Open > c1.Close && 100 * (c1.High - c1.Open) / c1.Open < _maximumShadowChange && c1.Low > c2.Low)
                {
                    if (c0.Open > c0.Close && c0.Open < c1.High && c0.Close > c1.Low && 100 * (c0.High - c0.Open) / c0.Open < _maximumShadowChange && 100 * (c0.Close - c0.Low) / c0.Low < _maximumShadowChange)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bullish3StarsintheSouth()
        {
            return DetectPattern(Is3StarsintheSouth, lookbackPeriod: 2);
        }

        private bool Is3WhiteSoldiers(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && 100 * (c2.High - c2.Close) / c2.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.Open < c1.Close && c2.Close > c1.Open && c1.Open > c2.Open && c1.Close > c2.Close && 100 * (c1.High - c1.Close) / c1.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c1) > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close > c0.Open && c0.Open > c1.Open && c0.Close > c1.Close && 100 * (c0.High - c0.Close) / c0.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> Bullish3WhiteSoldiers()
        {
            return DetectPattern(Is3WhiteSoldiers, lookbackPeriod: 2);
        }

        private bool Is3LineStrike2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];

            if (c3.Open < c3.Close && 100 * (c3.High - c3.Close) / c3.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c3) > _minimumCandleSize)
            {
                if (c2.Open < c2.Close && c3.Close > c2.Open && c2.Open > c3.Open && c2.Close > c3.Close && 100 * (c2.High - c2.Close) / c2.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c2) > _minimumCandleSize)
                {
                    if (c1.Open < c1.Close && c2.Close > c1.Open && c1.Open > c2.Open && c1.Close > c2.Close && 100 * (c1.High - c1.Close) / c1.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c1) > _minimumCandleSize)
                    {
                        if (c0.Open > c0.Close && c0.Open > c1.Close && c0.Close < c3.Open)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 4)]
        private List<OhlcvObject> Bullish3LineStrike()
        {
            return DetectPattern(Is3LineStrike2, lookbackPeriod: 3);
        }

        private bool IsBeltHold(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Low > c0.High)
            {
                if (c0.Open == c0.Low && c0.Open < c0.Close && 100 * (c0.High - c0.Close) / c0.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishBeltHold()
        {
            return DetectPattern(IsBeltHold, lookbackPeriod: 1);
        }

        private bool IsBreakaway2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open > c4.Close && (-100 * (c4.Close - c4.Open) / c4.Open > _minimumCandleSize))
            {
                if (c3.Open > c3.Close && c3.High < c4.Low && (-100 * (c3.Close - c3.Open) / c3.Open < _minimumCandleSize))
                {
                    if (c3.Close > c2.Close && Math.Abs(GetBodySizePercentSigned(c2)) < _minimumCandleSize)
                    {
                        if (c1.Open > c1.Close && c2.Close > c1.Close && -GetBodySizePercentSigned(c1) < _minimumCandleSize)
                        {
                            if (c0.Open < c0.Close && c0.Close < c4.Low && c0.Close > c3.High)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private List<OhlcvObject> BullishBreakaway()
        {
            return DetectPattern(IsBreakaway2, lookbackPeriod: 4);
        }

        private bool IsConcealingBabySwallow(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];

            if (c3.Open > c3.Close && c3.High == c3.Open && c3.Low == c3.Close)
            {
                if (c2.Open > c2.Close && c2.High == c2.Open && c2.Low == c2.Close)
                {
                    if (c1.Open > c1.Close && c1.Open < c2.Close && c1.High > c2.Close)
                    {
                        if (c0.Open > c0.Close && c0.Open >= c1.High && c0.Close < c1.Low)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 4)]
        private List<OhlcvObject> BullishConcealingBabySwallow()
        {
            return DetectPattern(IsConcealingBabySwallow, lookbackPeriod: 3);
        }

        private bool IshDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize)
            {
                if (c0.High < c1.Low && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) < _maximumDojiShadowSizes)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishDojiStar()
        {
            return DetectPattern(IshDojiStar, lookbackPeriod: 1);
        }

        private bool IsDragonflyDoji2(int i)
        {
            var c0 = _data[i - 0];
            return (Math.Abs(100 * (c0.High - c0.Close) / c0.High) < _maximumDojiBodySize && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) > _minimumCandleShadowSize);

        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 1)]
        private List<OhlcvObject> BullishDragonflyDoji()
        {
            return DetectPattern(IsDragonflyDoji2, lookbackPeriod: 0);
        }

        private bool IsEngulfing2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) < _maximumShortCandleSize)
            {
                if (c0.Open < c0.Close && c0.Open < c1.Close && c0.Close > c1.Open && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishEngulfing()
        {
            return DetectPattern(IsEngulfing2, lookbackPeriod: 1);
        }

        private bool IsGravestoneDoji2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize)
            {
                if (c0.High < c1.Open && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && c0.Low == c0.Open)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishGravestoneDoji()
        {
            return DetectPattern(IsGravestoneDoji2, lookbackPeriod: 1);
        }

        private bool IsHarami2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open < c0.Close && c0.Open > c1.Close && c0.Close < c1.Open && (GetBodySizePercentSigned(c0) < _maximumShortCandleSize))
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishHarami()
        {
            return DetectPattern(IsHarami2, lookbackPeriod: 1);
        }

        private bool IsHaramiCross2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && c0.Close < c1.Open && c1.Close < c0.Close && c0.Open < c1.Open && c1.Close < c0.Open)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishHaramiCross()
        {
            return DetectPattern(IsHaramiCross2, lookbackPeriod: 1);
        }

        private bool IsHomingPigeon(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c0.Open > c0.Close && c0.Open < c1.Open && c0.Close > c1.Close && -GetBodySizePercentSigned(c0) < _maximumShortCandleSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishHomingPigeon()
        {
            return DetectPattern(IsHomingPigeon, lookbackPeriod: 1);
        }

        private bool IsInvertedHammer(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && 100 * (c1.Close - c1.Low) / c1.Close < _maximumShadowSize)
            {
                if (c0.High < c1.Open && c0.High - c0.Low > 3 * Math.Abs(c0.Close - c0.Open) && 100 * (c0.Close - c0.Low) / c0.Close < _maximumShadowSize && 100 * Math.Abs((c0.Close - c0.Open) / c0.Open) < _maximumCandleSize)
                {
                    return true;
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishInvertedHammer()
        {
            return DetectPattern(IsInvertedHammer, lookbackPeriod: 1);
        }

        private bool IsKicking2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && 100 * (c1.High - c1.Open) / c1.Open < _maximumShadowSize && -100 * (c1.Low - c1.Close) / c1.Close < _maximumShadowSize && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c0.Open > c1.Open && c0.Open < c0.Close && 100 * (c0.High - c0.Close) / c0.Close < _maximumShadowSize && -100 * (c0.Low - c0.Open) / c0.Open < _maximumShadowSize && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishKicking()
        {
            return DetectPattern(IsKicking2, lookbackPeriod: 1);
        }

        private bool IsLadderBottom(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if ((c4.Open > c4.Close && -100 * (c4.Close - c4.Open) / c4.Open > _minimumCandleSize))
            {
                if ((c3.Open > c3.Close && c3.Close < c4.Close && c4.Close <= c3.Open && c3.Open <= c4.Open && -GetBodySizePercentSigned(c3) > _minimumCandleSize))
                {
                    if ((c2.Open > c2.Close && c3.Close < c2.Open && c2.Open < c3.Open && c2.Close < c3.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize))
                    {
                        if ((c1.Open > c1.Close && c1.High - c1.Open > 2 * (c1.Open - c1.Close)))
                        {
                            if ((c0.Open < c0.Close && c0.Open > c1.Open))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private List<OhlcvObject> BullishLadderBottom()
        {
            return DetectPattern(IsLadderBottom, lookbackPeriod: 4);
        }

        private bool IsLongWhiteCandlestick(int i)
        {
            var c0 = _data[i - 0];
            return (c0.Open < c0.Close && GetBodySizePercentSigned(c0) > _minimumCandleSize);

        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 1)]
        private List<OhlcvObject> BullishLongWhiteCandlestick()
        {
            return DetectPattern(IsLongWhiteCandlestick, lookbackPeriod: 0);
        }

        private bool IsMatHold(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open < c4.Close && GetBodySizePercentSigned(c4) > _minimumCandleSize)
            {
                if (c3.Open > c3.Close && c4.Close < c3.Close && -GetBodySizePercentSigned(c3) < _minimumCandleSize)
                {
                    if ((c2.Open < c2.Close && c2.Close < c3.Open && c2.Open > c4.Open) || (c2.Open > c2.Close && c2.Close > c4.Open && c2.Open < c3.Open))
                    {
                        if (c1.Open > c1.Close && c2.Close > c4.Open && c2.Open < c3.Open)
                        {
                            if (c0.Open < c0.Close && c0.Open > c1.Open && c0.Close > c4.Close && c0.Close > c3.Open)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private List<OhlcvObject> BullishMatHold()
        {
            return DetectPattern(IsMatHold, lookbackPeriod: 4);
        }

        private bool IsMatchingLow(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c0.Open > c0.Close && c0.Close == c1.Close)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishMatchingLow()
        {
            return DetectPattern(IsMatchingLow, lookbackPeriod: 2);
        }

        private bool IsMeetingLines2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c0.Open < c0.Close && c0.Close < c1.Close && -100 * (c0.Open - c0.Close) / c0.Close > _minimumCandleSize && 100 * Math.Abs((c0.Close - c1.Close) / c1.Close) < _maximumCloseDifference)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishMeetingLines()
        {
            return DetectPattern(IsMeetingLines2, lookbackPeriod: 1);
        }

        private bool IsMorningDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
            {
                if (c1.High < c2.Low && Math.Abs(100 * (c1.Open - c1.Close) / c1.Open) < _maximumDojiBodySize && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _maximumDojiShadowSizes)
                {
                    if ((c0.Open < c0.Close && c0.Close > (c2.Open + c2.Close) / 2M))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishMorningDojiStar()
        {
            return DetectPattern(IsMorningDojiStar, lookbackPeriod: 2);
        }

        private bool IsMorningStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.High < c2.Low && Math.Abs(100 * (c1.Open - c1.Close) / c1.Open) < _maximumShortCandleSize)
                {
                    if (c0.Open < c0.Close && c0.Close > (c2.Open + c2.Close) / 2M)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishMorningStar()
        {
            return DetectPattern(IsMorningStar, lookbackPeriod: 2);
        }

        private bool IsPiercingLine(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open < c0.Close && c0.Open < c1.Close && c0.Close > c1.Close + (c1.Open - c1.Close) / 2)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishPiercingLine()
        {
            return DetectPattern(IsPiercingLine, lookbackPeriod: 1);
        }

        private bool IsRising3Methods(int index)
        {
            var c0 = _data[index];
            var c1 = _data[index - 1];
            var c2 = _data[index - 2];
            var c3 = _data[index - 3];
            var c4 = _data[index - 4];

            if (c4.Open < c4.Close && GetBodySizePercentSigned(c4) > _minimumCandleSize)
            {
                if (c3.Open > c3.Close && c4.High > c3.Open && c4.Low < c3.Close && -GetBodySizePercentSigned(c3) < _minimumCandleSize)
                {
                    if (c2.Open < c3.Open && c2.Close < c3.Open && c4.High > c2.Open && c4.High > c2.Close && c4.Low < c2.Close && c4.Low < c2.Open)
                    {
                        if (c1.Open > c1.Close && c1.Close < Math.Min(c2.Open, c2.Close) && c4.High > c1.Open && c4.Low < c1.Close)
                        {
                            if (c0.Open < c0.Close && c0.Open > c1.Close && c0.Close > c4.Close)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private List<OhlcvObject> BullishRising3Methods()
        {
            return DetectPattern(IsRising3Methods, lookbackPeriod: 4);
        }

        private bool IsSeparatingLine(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c0.Open < c0.Close && 100 * (c0.Close - c0.Open) / c0.Open > _minimumCandleSize && 100 * Math.Abs((c0.Open - c1.Open) / c1.Open) < _maximumOpenDifference)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishSeparatingLines()
        {
            return DetectPattern(IsSeparatingLine, lookbackPeriod: 1);
        }
        private bool IsSideBySideWhiteLines2(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];

            if (_data[i - 2].Open < _data[i - 2].Close)
            {
                if (c1.Open < c1.Close && c1.Open > _data[i - 2].High)
                {
                    if (c0.Open < c0.Close && 100 * Math.Abs((c0.Open - c1.Open) / c1.Open) < _maximumDifference && 100 * Math.Abs((c0.Close - c1.Close) / c1.Close) < _maximumDifference)
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishSideBySideWhiteLines()
        {
            return DetectPattern(IsSideBySideWhiteLines2, lookbackPeriod: 2);
        }

        private bool IsStickSandwich(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (_data[i - 2].Open > _data[i - 2].Close)
            {
                if (c1.Open < c1.Close && c2.Close < c1.Close)
                {
                    if (c0.Open > c0.Close && c2.Close == c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishStickSandwich()
        {
            return DetectPattern(IsStickSandwich, lookbackPeriod: 2);
        }

        private bool IsTriStar2(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (Math.Abs(100 * (c2.Open - c2.Close) / c2.Open) < _maximumDojiBodySize)
            {
                if (Math.Abs(100 * (c1.Open - c1.Close) / c1.Open) < _maximumDojiBodySize && c1.High < c2.Low && c1.High < c0.Low)
                {
                    if (Math.Abs(100 * (c0.Open - c0.Close) / c0.Open) < _maximumDojiBodySize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishTriStar()
        {
            return DetectPattern(IsTriStar2, lookbackPeriod: 2);
        }

        private bool IsTweezerBottom(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (Math.Abs(100 * (c0.Open - c0.Close) / c0.Open) < _maximumShortCandleSize && c0.Low == c1.Low)
                {
                    return true;
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 2)]
        private List<OhlcvObject> BullishTweezerBottom()
        {
            return DetectPattern(IsTweezerBottom, lookbackPeriod: 1);
        }

        private bool IsUnique3RiverBottom(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
            {
                if (c1.Open > c1.Close && c1.Open == c1.High && 100 * (c1.Close - c1.Low) / c1.Close > _minimumCandleSize)
                {
                    if (c0.Open < c0.Close && 100 * (c0.Close - c0.Open) / c0.Open < _maximumShortCandleSize && c1.Close > c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishUnique3RiverBottom()
        {
            return DetectPattern(IsUnique3RiverBottom, lookbackPeriod: 2);
        }

        private bool IsUpsideGap3Methods(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && 100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
            {
                if (c1.Open < c1.Close && c1.Low > c2.High && 100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize)
                {
                    if (c0.Open > c0.Close && c1.Open < c0.Open && c2.Close > c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishUpsideGap3Methods()
        {
            return DetectPattern(IsUpsideGap3Methods, lookbackPeriod: 2);
        }

        private bool IsUpsideTasukiGap(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close)
            {
                if (c1.Open < c1.Close && c1.Low > c2.High)
                {
                    if (c0.Open > c0.Close && c1.Open < c0.Open && c0.Open < c1.Close && c1.Open > c0.Close && c2.Close < c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 3)]
        private List<OhlcvObject> BullishUpsideTasukiGap()
        {
            return DetectPattern(IsUpsideTasukiGap, lookbackPeriod: 2);
        }

        private bool IsWhiteClosingMarubozu(int index)
        {
            var candle = _data[index];
            if (candle.Open == 0) return false;
            return candle.Open < candle.Close && candle.Close == candle.High && candle.Open != candle.Low && 100 * (candle.Close - candle.Open) / candle.Open > _minimumCandleSize;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 1)]
        private List<OhlcvObject> BullishWhiteClosingMarubozu()
        {
            return DetectPattern(IsWhiteClosingMarubozu, lookbackPeriod: 0);
        }


        private bool IsWhiteMarubozu(int index)
        {
            var candle = _data[index];
            return candle.Open < candle.Close && candle.Close == candle.High && candle.Open == candle.Low && GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 1)]
        private List<OhlcvObject> BullishWhiteMarubozu()
        {
            return DetectPattern(IsWhiteMarubozu, lookbackPeriod: 0);
        }

        private bool IsWhiteOpeningMarubozu(int index)
        {
            var candle = _data[index];
            return candle.Open < candle.Close && candle.Close < candle.High && candle.Open == candle.Low && GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 1)]
        private List<OhlcvObject> BullishWhiteOpeningMarubozu()
        {
            return DetectPattern(IsWhiteOpeningMarubozu, lookbackPeriod: 0);
        }


        private decimal GetBodySizePercentSigned(OhlcvObject candle)
        {
            if (candle.Open == 0) return 0;
            return 100 * (candle.Close - candle.Open) / candle.Open;
        }

        public interface IPatternDetector
        {
            int LookbackPeriod { get; }
            bool Detect(List<OhlcvObject> data, int index);
        }
        private OhlcvObject CopyWithoutSignal(OhlcvObject source)
        {
            return new OhlcvObject
            {
                Open = source.Open,
                High = source.High,
                Low = source.Low,
                Close = source.Close,
                Volume = source.Volume,
                Signal = false
            };
        }

        private OhlcvObject CopyWithSignal(OhlcvObject source, bool signal)
        {
            return new OhlcvObject
            {
                Open = source.Open,
                High = source.High,
                Low = source.Low,
                Close = source.Close,
                Volume = source.Volume,
                Signal = signal
            };
        }

        private List<OhlcvObject> DetectPattern(Func<int, bool> patternMatcher, int lookbackPeriod)
        {
            var data = new List<OhlcvObject>(_data.Count);

            for (int i = 0; i < lookbackPeriod && i < _data.Count; i++)
            {
                data.Add(CopyWithoutSignal(_data[i]));
            }

            for (int i = lookbackPeriod; i < _data.Count; i++)
            {
                UpdateThresholds(i);
                bool validWindow = true;
                for (int j = i - lookbackPeriod; j <= i; j++)
                {
                    var candle = _data[j];
                    if (candle.Open <= 0M || candle.Close <= 0M || candle.High < Math.Max(candle.Open, candle.Close)
                        || candle.Low <= 0M || candle.Low > Math.Min(candle.Open, candle.Close))
                    {
                        validWindow = false;
                        break;
                    }
                }

                data.Add(CopyWithSignal(_data[i], validWindow && patternMatcher(i)));
            }

            return data;
        }

        public List<OhlcvObject> GetPatternsSignalsList(PatternNameEnum patternName)
        {
            return GetPatternsSignalsList(patternName.ToString());
        }

        public List<OhlcvObject> GetPatternsSignalsList(string patternName)
        {
            if (string.IsNullOrWhiteSpace(patternName))
                return _data.Select(CopyWithoutSignal).ToList();

            string normalizedName = patternName.Trim().Replace(" ", "");
            if (!_methodCache.TryGetValue(normalizedName, out var method))
            {
                method = GetType().GetMethod(normalizedName,
                    BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
                if (method == null || method.GetCustomAttribute<PatternMethodAttribute>() == null
                    || method.GetParameters().Length != 0)
                    return _data.Select(CopyWithoutSignal).ToList();

                _methodCache.TryAdd(normalizedName, method);
            }

            return (List<OhlcvObject>)method.Invoke(this, null)!;
        }

        public int GetSignalsCount(string patternName) => GetPatternsSignalsList(patternName).Count(x => x.Signal);

        public int GetSignalsCount(PatternNameEnum patternName) => GetSignalsCount(patternName.ToString());

        public List<string> GetAllMethodNames()
        {
            if (_cachedMethodNames != null)
                return new List<string>(_cachedMethodNames);

            lock (_methodNamesLock)
            {
                if (_cachedMethodNames != null)
                    return new List<string>(_cachedMethodNames);

                _cachedMethodNames = typeof(Patterns)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<PatternMethodAttribute>() != null)
                .Select(m => m.Name/*.Replace("Is", "")*/) // Remove "Is" prefix if needed
                .Distinct()
                .OrderBy(name => name)
                .ToList();

                return new List<string>(_cachedMethodNames);
            }
        }

        private bool IsStrongBullish(OhlcvObject candle)
        {
            return candle.Open < candle.Close &&
                   GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        private bool IsStrongBearish(OhlcvObject candle)
        {
            return candle.Open > candle.Close &&
                   -GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }
    }
}
