using System;
using System.Collections.Concurrent;
using System.Reflection;
namespace Candlestick_Patterns
{
    public class Patterns : IPatterns
    {
        private readonly decimal _insideBarMaxChange;
        private readonly decimal _minimumCandleSize;
        private readonly decimal _maximumShortCandleSize;
        private readonly decimal _minimumCandleDifference;
        private readonly decimal _maximumDojiShadowSizes;
        private readonly decimal _maximumDojiBodySize;
        private readonly decimal _minimumCandleShadowSize;
        private readonly decimal _maximumShadowSize;
        private readonly decimal _maximumPriceDifference;
        private readonly decimal _maximumCloseDifference;
        private readonly decimal _maximumCandleBodySize;
        private readonly decimal _maximumCandleShadowSize;
        private readonly decimal _maximumDifference;
        private readonly decimal _maximumShadowChange;
        private readonly decimal _maximumCloseHighChange;
        private readonly decimal _maximumCandleSize;
        private readonly decimal _maximumOpenDifference;
        private readonly List<OhlcvObject> _data;
        private static List<string>? _cachedMethodNames;
        private readonly decimal _averageBodySize;
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

        public enum PatternType
        {
            Bullish,
            Bearish,
            Continuation
        }

        public enum PatternNameEnum
        {
            None,
            Bearish2Crows,
            Bearish3BlackCrows,
            Bearish3InsideDown,
            Bearish3OutsideDown,
            Bearish3LineStrike,
            BearishAdvanceBlock,
            BearishBeltHold,
            BearishBlackClosingMarubozu,
            BearishBlackMarubozu,
            BearishBlackOpeningMarubozu,
            BearishBreakaway,
            BearishDeliberation,
            BearishDarkCloudCover,
            BearishDojiStar,
            BearishDownsideGap3Methods,
            BearishDownsideTasukiGap,
            BearishDragonflyDoji,
            BearishEngulfing,
            BearishEveningDojiStar,
            BearishEveningStar,
            BearishFalling3Methods,
            BearishGravestoneDoji,
            BearishHarami,
            BearishIdentical3Crows,
            BearishHaramiCross,
            BearishInNeck,
            BearishKicking,
            BearishLongBlackCandelstick,
            BearishMeetingLines,
            BearishOnNeck,
            BearishSeparatingLines,
            BearishShootingStar,
            BearishSideBySideWhiteLines,
            BearishThrusting,
            BearishTriStar,
            BearishTweezerTop,
            BearishUpsideGap2Crows,
            Bullish3InsideUp,
            Bullish3OutsideUp,
            Bullish3StarsintheSouth,
            Bullish3WhiteSoldiers,
            Bullish3LineStrike,
            BullishBeltHold,
            BullishBreakaway,
            BullishConcealingBabySwallow,
            BullishDojiStar,
            BullishDragonflyDoji,
            BullishEngulfing,
            BullishGravestoneDoji,
            BullishHarami,
            BullishHaramiCross,
            BullishHomingPigeon,
            BullishInvertedHammer,
            BullishKicking,
            BullishLadderBottom,
            BullishLongWhiteCandlestick,
            BullishMatHold,
            BullishMatchingLow,
            BullishMeetingLines,
            BullishMorningDojiStar,
            BullishMorningStar,
            BullishPiercingLine,
            BullishRising3Methods,
            BullishSeparatingLines,
            BullishSideBySideWhiteLines,
            BullishStickSandwich,
            BullishTriStar,
            BullishTweezerBottom,
            BullishUnique3RiverBottom,
            BullishUpsideGap3Methods,
            BullishUpsideTasukiGap,
            BullishWhiteClosingMarubozu,
            BullishWhiteMarubozu,
            BullishWhiteOpeningMarubozu
        }

        public Patterns(List<OhlcvObject> data)
        {
            _data = data;
            _averageBodySize = data.Average(x => Math.Abs(x.Open - x.Close));
            _insideBarMaxChange = _averageBodySize / 2;
            _minimumCandleSize = _averageBodySize * 2;
            _maximumShortCandleSize = _averageBodySize / 2;
            _minimumCandleDifference = _averageBodySize / 2;
            _maximumDojiShadowSizes = _averageBodySize / 2;
            _maximumDojiBodySize = _averageBodySize / 2;
            _minimumCandleShadowSize = _averageBodySize * 2;
            _maximumShadowSize = _averageBodySize * 3;
            _maximumPriceDifference = _averageBodySize * 3;
            _maximumCloseDifference = _averageBodySize * 3;
            _maximumCandleBodySize = _averageBodySize / 2;
            _maximumCandleShadowSize = _averageBodySize / 2;
            _maximumDifference = _averageBodySize / 2;
            _maximumShadowChange = _averageBodySize / 2;
            _maximumCloseHighChange = _averageBodySize * 3;
            _maximumCandleSize = _averageBodySize / 2;
            _maximumOpenDifference = _averageBodySize * 2;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bearish2Crows()
        {
            return DetectPattern(Is2Crows, lookbackPeriod: 5);

            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close <= _data[i - 0].Open && _data[i - 0].Open <= _data[i - 1].Open && _data[i - 2].Open <= _data[i - 0].Close && _data[i - 0].Close <= _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bearish3BlackCrows()
        {
            return DetectPattern(Is3BlackCrows, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            { 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < _data[i - 2].Close && _data[i - 2].Close <= _data[i - 1].Open && _data[i - 1].Open <= _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bearish3InsideDown()
        {
            return DetectPattern(IsInsideDown, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maximumShortCandleSize)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bearish3OutsideDown()
        {
            return DetectPattern(Is3OutsideDown, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < _maximumShortCandleSize)
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bearish3LineStrike()
        {
            return DetectPattern(Is3LineStrike, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 3].Open > _data[i - 3].Close && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Open && _data[i - 1].Open < _data[i - 2].Open && _data[i - 1].Close < _data[i - 2].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                        {
                            if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open <= _data[i - 1].Close && _data[i - 0].Close >= _data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishAdvanceBlock()
        {
            return DetectPattern(IsAdvanceBlock, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && ((100 + _minimumCandleDifference) / 100) * (_data[i - 1].Close - _data[i - 1].Open) < _data[i - 2].Close - _data[i - 2].Open && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && ((100 + _minimumCandleDifference) / 100) * (_data[i - 0].Close - _data[i - 0].Open) < _data[i - 1].Close - _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishBeltHold()
        {
            return DetectPattern(IsBeltHold2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].High < _data[i - 0].Low)
                {
                    
                    if (_data[i - 0].Open == _data[i - 0].High && _data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsBlackClosingMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High > candle.Open && candle.Close == candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        private List<OhlcvObject> BearishBlackClosingMarubozu()
        {
            return DetectPattern(IsBlackClosingMarubozu, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High > _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsBlackMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High == candle.Open && candle.Close == candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        private List<OhlcvObject> BearishBlackMarubozu()
        {
            return DetectPattern(IsBlackMarubozu, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsBlackOpeningMarubozu(int i)
        {
            var candle = _data[i - 0];

            return (candle.Open > candle.Close && candle.High == candle.Open && candle.Close > candle.Low && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }


        private List<OhlcvObject> BearishBlackOpeningMarubozu()
        {
            return DetectPattern(IsBlackOpeningMarubozu, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close > _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishBreakaway()
        {
            return DetectPattern(IsBreakaway, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 4].High < _data[i - 3].Low && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < _minimumCandleSize)
                    {
                        
                        if (_data[i - 3].Close < _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < _minimumCandleSize)
                        {
                            
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _minimumCandleSize)
                            {
                                
                                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 4].High && _data[i - 0].Close < _data[i - 3].Low)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }
        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishDeliberation()
        {
            return DetectPattern(IsDeliberation, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Open < _data[i - 1].Open && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close <= _data[i - 0].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _minimumCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsDarkCloudCover(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open > c1.High && c0.Close < c1.Open + ((c1.Close - c1.Open) / 2))
                {
                    return true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishDarkCloudCover()
        {
            return DetectPattern(IsDarkCloudCover, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && _data[i - 0].Close < _data[i - 1].Open + ((_data[i - 1].Close - _data[i - 1].Open) / 2))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && GetBodySizePercentSigned(c1) > _minimumCandleSize)
            {
                if (c1.High < c0.Low && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) < _maximumDojiShadowSizes)
                {
                    return  true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishDojiStar()
        {
            return DetectPattern(IsDojiStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maximumDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishDownsideGap3Methods()
        {
            return DetectPattern(IsDownsideGap3Methods, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 1].Open > _data[i - 0].Open && _data[i - 2].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishDownsideTasukiGap()
        {
            return DetectPattern(IsDownsideTasukiGap, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 1].Open > _data[i - 0].Open && _data[i - 2].Low > _data[i - 0].Close && _data[i - 1].High < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsDragonflyDoji(int index)
        {
            var c0 = _data[index - 0];

            return (Math.Abs(100 * (c0.High - c0.Close) / c0.High) < _maximumDojiBodySize && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) > _minimumCandleShadowSize);
        }

        private List<OhlcvObject> BearishDragonflyDoji()
        {
            return DetectPattern(IsDragonflyDoji, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minimumCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsEngulfing(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (100 * (c1.Close - c1.Open) / c1.Open < _maximumShortCandleSize))
            {
                if (c0.Open > c0.Close && c0.Open > c1.Close && c0.Close < c1.Open && (-GetBodySizePercentSigned(c0)> _minimumCandleSize))
                {
                    return true;
                }
            }

            return false;
        }


        private List<OhlcvObject> BearishEngulfing()
        {
            return DetectPattern(IsEngulfing, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maximumShortCandleSize))
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsEveningDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c2.High < c1.Low && Math.Abs(GetBodySizePercentSigned(c1)) < _maximumDojiBodySize && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _maximumDojiShadowSizes)
                {
                    if (c0.Open > c0.Close && c0.Close > c2.Open && c0.Close < c2.Close && -100 * (c0.Close - c0.Open) / c0.Open > _minimumCandleSize)
                    {
                        return  true;
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishEveningDojiStar()
        {
            return DetectPattern(IsEveningDojiStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _maximumDojiShadowSizes)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 2].Open && _data[i - 0].Close < _data[i - 2].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsEveningStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open < c2.Close && GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c2.High < c1.Low && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _minimumCandleSize)
                {
                    if (c0.Open > c0.Close && c0.Close > c2.Open && c0.Close < c2.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishEveningStar()
        {
            return DetectPattern(IsEveningStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 2].Open && _data[i - 0].Close < _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishFalling3Methods()
        {
            return DetectPattern(IsFalling3Methods, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize))
                {
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Open && _data[i - 4].Close < _data[i - 3].Close)
                    {
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Close) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Open < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Open))
                        {
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Close < _data[i - 4].Open && Math.Max(_data[i - 2].Close, _data[i - 2].Open) < _data[i - 1].Close)
                            {
                                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 4].Close && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize))
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishGravestoneDoji()
        {
            return DetectPattern(IsGravestoneDoji, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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


        private List<OhlcvObject> BearishHarami()
        {
            return DetectPattern(IsHarami, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maximumShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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
                        return  true;
                    }
                }
            }

            return false;
        }


        private List<OhlcvObject> BearishIdentical3Crows()
        {
            return DetectPattern(IsIdentical3Crows, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && (100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < -1 * _minimumCandleSize))
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close == _data[i - 1].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < -1 * _minimumCandleSize))
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close == _data[i - 0].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < -1 * _minimumCandleSize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsHaramiCross(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && c0.Open < c1.Close && c1.Open < c0.Open)
                {
                   return  true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishHaramiCross()
        {
            return DetectPattern(IsHaramiCross, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsInNeck(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open > c1.Close && (-100 * (c1.Close - c1.Open) / c1.Open > _minimumCandleSize))
            {
                if (c0.Open < c0.Close && c0.Open < c1.Low && c0.Close > c1.Close && 100 * (c0.Close - c1.Close) / c1.Close < _insideBarMaxChange)
                {
                    return  true;
                }
            }

            return false;
        }


        private List<OhlcvObject> BearishInNeck()
        {
            return DetectPattern(IsInNeck, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close < _insideBarMaxChange)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsKicking(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Open < c1.Close && 100 * (c1.Open - c1.Low) / c1.Open < _maximumShadowSize && 100 * (c1.High - c1.Close) / c1.Close < _maximumShadowSize && (GetBodySizePercentSigned(c1) > _minimumCandleSize))
            {
                if (c0.Open < c1.Open && c0.Open > c0.Close && 100 * (c0.Close - c0.Low) / c0.Close < _maximumShadowSize && 100 * (c0.High - c0.Open) / c0.Open < _maximumShadowSize && (-GetBodySizePercentSigned(c0) > _minimumCandleSize))
                {
                    return  true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishKicking()
        {
            return DetectPattern(IsKicking, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Open - _data[i - 1].Low) / _data[i - 1].Open < _maximumShadowSize && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maximumShadowSize && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Open > _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maximumShadowSize && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < _maximumShadowSize && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
        private bool IsLongBlackCandelstick(int index)
        {
            var candle = _data[index];

            return (candle.Open > candle.Close && -GetBodySizePercentSigned(candle) > _minimumCandleSize);
        }

        private List<OhlcvObject> BearishLongBlackCandelstick()
        {
            return DetectPattern(IsLongBlackCandelstick, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishMeetingLines()
        {
            return DetectPattern(IsMeetingLines, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && (100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minimumCandleSize) && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maximumCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishOnNeck()
        {
            return DetectPattern(IsOnNeck, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Low) / _data[i - 0].Close) < _maximumPriceDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishSeparatingLines()
        {
            return DetectPattern(IsSeparatingLines, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maximumCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishShootingStar()
        {
            return DetectPattern(IsShootingStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close)
                {
                    
                    if (_data[i - 1].High < _data[i - 0].Low && 3 * Math.Abs(_data[i - 0].Open - _data[i - 0].Close) < _data[i - 0].High - _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumCandleBodySize && 100 * (Math.Min(_data[i - 0].Open, _data[i - 0].Close) - _data[i - 0].Low) / Math.Min(_data[i - 0].Open, _data[i - 0].Close) < _maximumCandleShadowSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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


        private List<OhlcvObject> BearishSideBySideWhiteLines()
        {
            return DetectPattern(IsSideBySideWhiteLines, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].High < _data[i - 2].Low)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 1].Open) / _data[i - 0].Open) < _maximumDifference && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 0].Close) < _maximumDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishThrusting()
        {
            return DetectPattern(IsThrusting, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close)
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishTriStar()
        {
            return DetectPattern(IsTriStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < _maximumDojiBodySize)
                {
                    
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maximumDojiBodySize && _data[i - 2].High < _data[i - 1].Low && _data[i - 0].High < _data[i - 1].Low)
                    {
                        
                        if ((Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BearishTweezerTop()
        {
            return DetectPattern(IsTweezerTop, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open >= _data[i - 0].Close && _data[i - 0].High == _data[i - 1].High && (Math.Abs(100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maximumShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bearish, lookbackPeriod: 5)]
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
                       return  true;
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BearishUpsideGap2Crows()
        {
            return DetectPattern(IsUpsideGap2Crows, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 2].High < _data[i - 1].Low && _data[i - 1].Open > _data[i - 1].Close)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close < _data[i - 1].Close && _data[i - 0].Close > _data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bullish3InsideUp()
        {
            return DetectPattern(Is3InsideUp, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maximumShortCandleSize))
                    {
                        
                        if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close > _data[i - 1].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bullish3OutsideUp()
        {
            return DetectPattern(Is3OutsideUp, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < _maximumShortCandleSize))
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close > _data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bullish3StarsintheSouth()
        {
            return DetectPattern(Is3StarsintheSouth, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Open) / _data[i - 2].Open < _maximumShadowChange && 100 * (_data[i - 2].Close - _data[i - 2].Low) / _data[i - 2].Low > _minimumCandleSize && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < _maximumShadowChange && _data[i - 1].Low > _data[i - 2].Low)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].High && _data[i - 0].Close > _data[i - 1].Low && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < _maximumShadowChange && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Low < _maximumShadowChange)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> Bullish3WhiteSoldiers()
        {
            return DetectPattern(Is3WhiteSoldiers, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < _maximumCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maximumCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maximumCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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
        

        private List<OhlcvObject> Bullish3LineStrike()
        {
            return DetectPattern(Is3LineStrike2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 3].Open < _data[i - 3].Close && 100 * (_data[i - 3].High - _data[i - 3].Close) / _data[i - 3].Close < _maximumCloseHighChange && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 2].Open < _data[i - 2].Close && _data[i - 3].Close > _data[i - 2].Open && _data[i - 2].Open > _data[i - 3].Open && _data[i - 2].Close > _data[i - 3].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < _maximumCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maximumCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                        {
                            
                            if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsBeltHold(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];

            if (c1.Low > c0.High)
            {
                if (c0.Open == c0.Low && c0.Open < c0.Close && 100 * (c0.High - c0.Close) /c0.Close < _maximumCloseHighChange && GetBodySizePercentSigned(c0) > _minimumCandleSize)
                {
                    return  true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishBeltHold()
        {
            return DetectPattern(IsBeltHold, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Low > _data[i - 0].High)
                {
                    
                    if (_data[i - 0].Open == _data[i - 0].Low && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maximumCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsBreakaway2(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open > c4.Close && (-100 * (c4.Close - c4.Open) / c4.Open > _minimumCandleSize))
            {
                if (c3.Open >c3.Close &&c3.High < c4.Low && (-100 * (c3.Close -c3.Open) /c3.Open < _minimumCandleSize))
                {
                    if (c3.Close > c2.Close && Math.Abs(GetBodySizePercentSigned(c2)) < _minimumCandleSize)
                    {
                        if (c1.Open > c1.Close && c2.Close > c1.Close && -GetBodySizePercentSigned(c1) < _minimumCandleSize)
                        {
                            if (c0.Open < c0.Close && c0.Close < c4.Low && c0.Close >c3.High)
                            {
                               return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishBreakaway()
        {
            return DetectPattern(IsBreakaway2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High < _data[i - 4].Low && (-100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < _minimumCandleSize))
                    {
                        
                        if (_data[i - 3].Close > _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < _minimumCandleSize)
                        {
                            
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _minimumCandleSize))
                            {
                                
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 4].Low && _data[i - 0].Close > _data[i - 3].High)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishConcealingBabySwallow()
        {
            return DetectPattern(IsConcealingBabySwallow, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High == _data[i - 3].Open && _data[i - 3].Low == _data[i - 3].Close)
                {
                    
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].High == _data[i - 2].Open && _data[i - 2].Low == _data[i - 2].Close)
                    {
                        
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].High > _data[i - 2].Close)
                        {
                            
                            if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open >= _data[i - 1].High && _data[i - 0].Close < _data[i - 1].Low)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishDojiStar()
        {
            return DetectPattern(IshDojiStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maximumDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsDragonflyDoji2(int i)
        {
            var c0 = _data[i - 0];
            return (Math.Abs(100 * (c0.High - c0.Close) / c0.High) < _maximumDojiBodySize && Math.Abs(GetBodySizePercentSigned(c0)) < _maximumDojiBodySize && Math.Abs(100 * (c0.High - c0.Low) / c0.High) > _minimumCandleShadowSize);

        }

        private List<OhlcvObject> BullishDragonflyDoji()
        {
            return DetectPattern(IsDragonflyDoji2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minimumCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishEngulfing()
        {
            return DetectPattern(IsEngulfing2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maximumShortCandleSize)
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishGravestoneDoji()
        {
            return DetectPattern(IsGravestoneDoji2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].High < _data[i - 1].Open && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishHarami()
        {
            return DetectPattern(IsHarami2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maximumShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishHaramiCross()
        {
            return DetectPattern(IsHaramiCross2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize && _data[i - 0].Close < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishHomingPigeon()
        {
            return DetectPattern(IsHomingPigeon, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maximumShortCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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


        private List<OhlcvObject> BullishInvertedHammer()
        {
            return DetectPattern(IsInvertedHammer, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close < _maximumShadowSize)
                {
                    
                    if (_data[i - 0].High < _data[i - 1].Open && _data[i - 0].High - _data[i - 0].Low > 3 * Math.Abs(_data[i - 0].Close - _data[i - 0].Open) && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maximumShadowSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maximumCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishKicking()
        {
            return DetectPattern(IsKicking2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < _maximumShadowSize && -100 * (_data[i - 1].Low - _data[i - 1].Close) / _data[i - 1].Close < _maximumShadowSize && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maximumShadowSize && -100 * (_data[i - 0].Low - _data[i - 0].Open) / _data[i - 0].Open < _maximumShadowSize && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishLadderBottom()
        {
            return DetectPattern(IsLadderBottom, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if ((_data[i - 4].Open > _data[i - 4].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize))
                {
                    
                    if ((_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Close && _data[i - 4].Close <= _data[i - 3].Open && _data[i - 3].Open <= _data[i - 4].Open && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minimumCandleSize))
                    {
                        
                        if ((_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize))
                        {
                            
                            if ((_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].High - _data[i - 1].Open > 2 * (_data[i - 1].Open - _data[i - 1].Close)))
                            {
                                
                                if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open))
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsLongWhiteCandlestick(int i)
        {
            var c0 = _data[i - 0];
            return (c0.Open < c0.Close && GetBodySizePercentSigned(c0) > _minimumCandleSize);

        }

        private List<OhlcvObject> BullishLongWhiteCandlestick()
        {
            return DetectPattern(IsLongWhiteCandlestick, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsMatHold(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];
            var c3 = _data[i - 3];
            var c4 = _data[i - 4];

            if (c4.Open < c4.Close && GetBodySizePercentSigned(c4) > _minimumCandleSize)
            {
                if (c3.Open > c3.Close && c4.Close < c3.Close && -GetBodySizePercentSigned(c4) < _minimumCandleSize)
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

        private List<OhlcvObject> BullishMatHold()
        {
            return DetectPattern(IsMatHold, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].Close < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < _minimumCandleSize)
                    {
                        
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 3].Open && _data[i - 2].Open > _data[i - 4].Open) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open))
                        {
                            
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open)
                            {
                                
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 4].Close && _data[i - 0].Close > _data[i - 3].Open)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsMatchingLow(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c1.Open > c1.Close && -GetBodySizePercentSigned(c1)> _minimumCandleSize)
            {
                if (c0.Open > c0.Close && c0.Close == c1.Close)
                {
                    return true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishMatchingLow()
        {
            return DetectPattern(IsMatchingLow, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close == _data[i - 1].Close)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishMeetingLines()
        {
            return DetectPattern(IsMeetingLines2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minimumCandleSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maximumCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsMorningDojiStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
            {
                if (c1.High < c2.Low && Math.Abs(100 * (c1.Open - c1.Close) / c1.Open) < _maximumDojiBodySize && Math.Abs(100 * (c1.High - c1.Low) / c1.High) < _maximumDojiShadowSizes)
                {
                    if ((c0.Open < c0.Close && c1.Close < c0.Close))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishMorningDojiStar()
        {
            return DetectPattern(IsMorningDojiStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maximumDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _maximumDojiShadowSizes)
                    {
                         
                        if ((_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsMorningStar(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if (c2.Open > c2.Close && -GetBodySizePercentSigned(c2) > _minimumCandleSize)
            {
                if (c1.High < c2.Low && Math.Abs(100 * (c1.Open - c1.Close) / c1.Open) < _maximumShortCandleSize)
                {
                    if (c0.Open < c0.Close && c1.Close < c0.Close)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishMorningStar()
        {
            return DetectPattern(IsMorningStar, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maximumShortCandleSize)
                    {
                         
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishPiercingLine()
        {
            return DetectPattern(IsPiercingLine, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize))
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsRising3Methods(int index)
        {
            var c0 = _data[index];      
            var c1 = _data[index - 1];  
            var c2 = _data[index - 2];  
            var c3 = _data[index - 3];  
            var c4 = _data[index - 4];  

            if (c4.Open < c4.Close && GetBodySizePercentSigned(c4) > _minimumCandleSize)
            {
                if (c3.Open > c3.Close && c4.High > c3.Open && c4.Low < c3.Close && -GetBodySizePercentSigned(c4) < _minimumCandleSize)
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


        private List<OhlcvObject> BullishRising3Methods()
        {
            return DetectPattern(IsRising3Methods, lookbackPeriod: 5);

            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].High > _data[i - 3].Open && _data[i - 4].Low < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < _minimumCandleSize)
                    {
                        
                        if (_data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Open && _data[i - 4].High > _data[i - 2].Open && _data[i - 4].High > _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Open)
                        {
                            
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < Math.Min(_data[i - 2].Open, _data[i - 2].Close) && _data[i - 4].High > _data[i - 1].Open && _data[i - 4].Low < _data[i - 1].Close)
                            {
                                
                                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close > _data[i - 4].Close)
                                {
                                    data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishSeparatingLines()
        {
            return DetectPattern(IsSeparatingLine, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maximumOpenDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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


        private List<OhlcvObject> BullishSideBySideWhiteLines()
        {
            return DetectPattern(IsSideBySideWhiteLines2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].High)
                    {
                        
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maximumDifference && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maximumDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IstickSandwic(int i)
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

        private List<OhlcvObject> BullishStickSandwich()
        {
            return DetectPattern(IstickSandwic, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Close)
                    {
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 2].Close == _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishTriStar()
        {
            return DetectPattern(IsTriStar2, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < _maximumDojiBodySize)
                {
                    
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maximumDojiBodySize && _data[i - 1].High < _data[i - 2].Low && _data[i - 1].High < _data[i - 0].Low)
                    {
                        
                        if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumDojiBodySize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsTweezerBottom(int i)
        {
            var c0 = _data[i];
            var c1 = _data[i - 1];

            if (100 * Math.Abs((c1.Close - c1.Open) / c1.Open) > _minimumCandleSize)
            {
                if (Math.Abs(100 * (c0.Open - c0.Close) / c0.Open) < _maximumShortCandleSize && c0.Low == c1.Low)
                {
                    return true;
                }
            }

            return false;
        }

        private List<OhlcvObject> BullishTweezerBottom()
        {
            return DetectPattern(IsTweezerBottom, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (100 * Math.Abs((_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open) > _minimumCandleSize)
                {
                    
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maximumShortCandleSize && _data[i - 0].Low == _data[i - 1].Low)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
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

        private List<OhlcvObject> BullishUnique3RiverBottom()
        {
            return DetectPattern(IsUnique3RiverBottom, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open == _data[i - 1].High && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close > _minimumCandleSize)
                    {
                         
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maximumShortCandleSize && _data[i - 1].Close > _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IshUpsideGap3Methods(int i)
        {
            var c0 = _data[i - 0];
            var c1 = _data[i - 1];
            var c2 = _data[i - 2];

            if(c2.Open < c2.Close && 100 * (c2.Close - c2.Open) / c2.Open > _minimumCandleSize)
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

        private List<OhlcvObject> BullishUpsideGap3Methods()
        {
            return DetectPattern(IshUpsideGap3Methods, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minimumCandleSize)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minimumCandleSize)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Open < _data[i - 0].Open && _data[i - 2].Close > _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }

        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IshUpsideTasukiGap(int i)
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

        private List<OhlcvObject> BullishUpsideTasukiGap()
        {
            return DetectPattern(IshUpsideTasukiGap, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {
                
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Open < _data[i - 0].Open && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 0].Close && _data[i - 2].Close < _data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        
        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsWhiteClosingMarubozu(int index)
        {
            var candle = _data[index];
            if (candle.Open == 0) return false;
            return candle.Open < candle.Close && candle.Close == candle.High && candle.Open != candle.Low && 100 * (candle.Close - candle.Open) / candle.Open > _minimumCandleSize;
        }

        private List<OhlcvObject> BullishWhiteClosingMarubozu()
        {
            return DetectPattern(IsWhiteClosingMarubozu, lookbackPeriod: 5);

            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {

                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open != _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IsWhiteMarubozu(int index)
        {
            var candle = _data[index];
            return candle.Open < candle.Close && candle.Close == candle.High && candle.Open == candle.Low && GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        private List<OhlcvObject> BullishWhiteMarubozu()
        {
            return DetectPattern(IsWhiteMarubozu, lookbackPeriod: 5);

            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {

                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }


        [PatternMethod(PatternType.Bullish, lookbackPeriod: 5)]
        private bool IshWhiteOpeningMarubozu(int index)
        {
            var candle = _data[index];
            return candle.Open < candle.Close && candle.Close < candle.High && candle.Open == candle.Low && GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        private List<OhlcvObject> BullishWhiteOpeningMarubozu()
        {
            return DetectPattern(IshWhiteOpeningMarubozu, lookbackPeriod: 5);
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();
            for (int i = 5; i < _data.Count; i++)
            {

                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minimumCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }


        private decimal GetBodySizePercentSigned(OhlcvObject candle)
        {
            if (candle.Open == 0) return 0;
            return 100 * (candle.Close - candle.Open) / candle.Open;
        }

        /*public Dictionary<PatternNameEnum, List<OhlcvObject>> GetAllPatterns()
        {
            var dict = new Dictionary<PatternNameEnum,List<OhlcvObject>>();
            dict.Add(PatternNameEnum.Bearish2Crows, Bearish2Crows());
            dict.Add(PatternNameEnum.Bearish3BlackCrows,            Bearish3BlackCrows                    ());
            dict.Add(PatternNameEnum.Bearish3InsideDown,            Bearish3InsideDown                    ());
            dict.Add(PatternNameEnum.Bearish3OutsideDown,           Bearish3OutsideDown                   ());
            dict.Add(PatternNameEnum.Bearish3LineStrike,            Bearish3LineStrike                    ());
            dict.Add(PatternNameEnum.BearishAdvanceBlock,           BearishAdvanceBlock                   ());
            dict.Add(PatternNameEnum.BearishBeltHold,               BearishBeltHold                       ());
            dict.Add(PatternNameEnum.BearishBlackClosingMarubozu,   BearishBlackClosingMarubozu           ());
            dict.Add(PatternNameEnum.BearishBlackMarubozu,          BearishBlackMarubozu                  ());
            dict.Add(PatternNameEnum.BearishBlackOpeningMarubozu,   BearishBlackOpeningMarubozu           ());
            dict.Add(PatternNameEnum.BearishBreakaway,              BearishBreakaway                      ());
            dict.Add(PatternNameEnum.BearishDeliberation,           BearishDeliberation                   ());
            dict.Add(PatternNameEnum.BearishDarkCloudCover,         BearishDarkCloudCover                 ());
            dict.Add(PatternNameEnum.BearishDojiStar,               BearishDojiStar                       ());
            dict.Add(PatternNameEnum.BearishDownsideGap3Methods,    BearishDownsideGap3Methods            ());
            dict.Add(PatternNameEnum.BearishDownsideTasukiGap,      BearishDownsideTasukiGap              ());
            dict.Add(PatternNameEnum.BearishDragonflyDoji,          BearishDragonflyDoji                  ());
            dict.Add(PatternNameEnum.BearishEngulfing,              BearishEngulfing                      ());
            dict.Add(PatternNameEnum.BearishEveningDojiStar,        BearishEveningDojiStar                ());
            dict.Add(PatternNameEnum.BearishEveningStar,            BearishEveningStar                    ());
            dict.Add(PatternNameEnum.BearishFalling3Methods,        BearishFalling3Methods                ());
            dict.Add(PatternNameEnum.BearishGravestoneDoji,         BearishGravestoneDoji                 ());
            dict.Add(PatternNameEnum.BearishHarami,                 BearishHarami                         ());
            dict.Add(PatternNameEnum.BearishIdentical3Crows,        BearishIdentical3Crows                ());
            dict.Add(PatternNameEnum.BearishHaramiCross,            BearishHaramiCross                    ());
            dict.Add(PatternNameEnum.BearishInNeck,                 BearishInNeck                         ());
            dict.Add(PatternNameEnum.BearishKicking,                BearishKicking                        ());
            dict.Add(PatternNameEnum.BearishLongBlackCandelstick,   BearishLongBlackCandelstick           ());
            dict.Add(PatternNameEnum.BearishMeetingLines,           BearishMeetingLines                   ());
            dict.Add(PatternNameEnum.BearishOnNeck,                 BearishOnNeck                         ());
            dict.Add(PatternNameEnum.BearishSeparatingLines,        BearishSeparatingLines                ());
            dict.Add(PatternNameEnum.BearishShootingStar,           BearishShootingStar                   ());
            dict.Add(PatternNameEnum.BearishSideBySideWhiteLines,   BearishSideBySideWhiteLines           ());
            dict.Add(PatternNameEnum.BearishThrusting,              BearishThrusting                      ());
            dict.Add(PatternNameEnum.BearishTriStar,                BearishTriStar                        ());
            dict.Add(PatternNameEnum.BearishTweezerTop,             BearishTweezerTop                     ());
            dict.Add(PatternNameEnum.BearishUpsideGap2Crows,        BearishUpsideGap2Crows                ());
            dict.Add(PatternNameEnum.Bullish3InsideUp,              Bullish3InsideUp                      ());
            dict.Add(PatternNameEnum.Bullish3OutsideUp,             Bullish3OutsideUp                     ());
            dict.Add(PatternNameEnum.Bullish3StarsintheSouth,       Bullish3StarsintheSouth               ());
            dict.Add(PatternNameEnum.Bullish3WhiteSoldiers,         Bullish3WhiteSoldiers                 ());
            dict.Add(PatternNameEnum.Bullish3LineStrike,            Bullish3LineStrike                    ());
            dict.Add(PatternNameEnum.BullishBeltHold,               BullishBeltHold                       ());
            dict.Add(PatternNameEnum.BullishBreakaway,              BullishBreakaway                      ());
            dict.Add(PatternNameEnum.BullishConcealingBabySwallow,  BullishConcealingBabySwallow          ());
            dict.Add(PatternNameEnum.BullishDojiStar,               BullishDojiStar                       ());
            dict.Add(PatternNameEnum.BullishDragonflyDoji,          BullishDragonflyDoji                  ());
            dict.Add(PatternNameEnum.BullishEngulfing,              BullishEngulfing                      ());
            dict.Add(PatternNameEnum.BullishGravestoneDoji,         BullishGravestoneDoji                 ());
            dict.Add(PatternNameEnum.BullishHarami,                 BullishHarami                         ());
            dict.Add(PatternNameEnum.BullishHaramiCross,            BullishHaramiCross                    ());
            dict.Add(PatternNameEnum.BullishHomingPigeon,           BullishHomingPigeon                   ());
            dict.Add(PatternNameEnum.BullishInvertedHammer,         BullishInvertedHammer                 ());
            dict.Add(PatternNameEnum.BullishKicking,                BullishKicking                        ());
            dict.Add(PatternNameEnum.BullishLadderBottom,           BullishLadderBottom                   ());
            dict.Add(PatternNameEnum.BullishLongWhiteCandlestick,   BullishLongWhiteCandlestick           ());
            dict.Add(PatternNameEnum.BullishMatHold,                BullishMatHold                        ());
            dict.Add(PatternNameEnum.BullishMatchingLow,            BullishMatchingLow                    ());
            dict.Add(PatternNameEnum.BullishMeetingLines,           BullishMeetingLines                   ());
            dict.Add(PatternNameEnum.BullishMorningDojiStar,        BullishMorningDojiStar                ());
            dict.Add(PatternNameEnum.BullishMorningStar,            BullishMorningStar                    ());
            dict.Add(PatternNameEnum.BullishPiercingLine,           BullishPiercingLine                   ());
            dict.Add(PatternNameEnum.BullishRising3Methods,         BullishRising3Methods                 ());
            dict.Add(PatternNameEnum.BullishSeparatingLines,        BullishSeparatingLines                ());
            dict.Add(PatternNameEnum.BullishSideBySideWhiteLines,   BullishSideBySideWhiteLines           ());
            dict.Add(PatternNameEnum.BullishStickSandwich,          BullishStickSandwich                  ());
            dict.Add(PatternNameEnum.BullishTriStar,                BullishTriStar                        ());
            dict.Add(PatternNameEnum.BullishTweezerBottom,          BullishTweezerBottom                  ());
            dict.Add(PatternNameEnum.BullishUnique3RiverBottom,     BullishUnique3RiverBottom             ());
            dict.Add(PatternNameEnum.BullishUpsideGap3Methods,      BullishUpsideGap3Methods              ());
            dict.Add(PatternNameEnum.BullishUpsideTasukiGap,        BullishUpsideTasukiGap                ());
            dict.Add(PatternNameEnum.BullishWhiteClosingMarubozu,   BullishWhiteClosingMarubozu           ());
            dict.Add(PatternNameEnum.BullishWhiteMarubozu,          BullishWhiteMarubozu                  ());
            dict.Add(PatternNameEnum.BullishWhiteOpeningMarubozu, BullishWhiteOpeningMarubozu());

            return dict;
        }*/

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
                bool signal = patternMatcher(i);
                data.Add(CopyWithSignal(_data[i], signal));
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
                return new List<OhlcvObject>(_data);

            var normalizedName = patternName.Trim().Replace(" ", "");

            // Get or cache the method
            var method = _methodCache.GetOrAdd(normalizedName, name => GetType().GetMethod(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            if (method == null)
                return new List<OhlcvObject>(_data);

            try
            {
                return (List<OhlcvObject>)method.Invoke(this, null);
            }
            catch
            {
                return new List<OhlcvObject>(_data);
            }
        }

        public int GetSignalsCount(string patternName) => GetPatternsSignalsList(patternName).Count(x => x.Signal);

        public int GetSignalsCount(PatternNameEnum patternName) => GetSignalsCount(patternName.ToString());

        public List<string> GetAllMethodNames()
        {
            if (_cachedMethodNames != null)
                return new List<string>(_cachedMethodNames);

            _cachedMethodNames = typeof(Patterns)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<PatternMethodAttribute>() != null)
                .Select(m => m.Name.Replace("Is", "")) // Remove "Is" prefix if needed
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            return new List<string>(_cachedMethodNames);
        
        }
        private bool IsStrongBullish(OhlcvObject candle)
        {
            return candle.Open < candle.Close &&
                   GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        private bool IsStrongBearish(OhlcvObject candle)
        {
            return candle.Open > candle.Close &&
                   GetBodySizePercentSigned(candle) > _minimumCandleSize;
        }

        /*public List<OhlcvObject> GetPatternsSignalsList(PatternNameEnum patternName) => patternName switch {
            PatternNameEnum.Bearish2Crows => Bearish2Crows(),
            PatternNameEnum.Bearish3BlackCrows => Bearish3BlackCrows(),
            PatternNameEnum.Bearish3InsideDown => Bearish3InsideDown(),
            PatternNameEnum.Bearish3OutsideDown => Bearish3OutsideDown(),
            PatternNameEnum.Bearish3LineStrike => Bearish3LineStrike(),
            PatternNameEnum.BearishAdvanceBlock => BearishAdvanceBlock(),
            PatternNameEnum.BearishBeltHold => BearishBeltHold(),
            PatternNameEnum.BearishBlackClosingMarubozu => BearishBlackClosingMarubozu(),
            PatternNameEnum.BearishBlackMarubozu => BearishBlackMarubozu(),
            PatternNameEnum.BearishBlackOpeningMarubozu => BearishBlackOpeningMarubozu(),
            PatternNameEnum.BearishBreakaway => BearishBreakaway(),
            PatternNameEnum.BearishDeliberation => BearishDeliberation(),
            PatternNameEnum.BearishDarkCloudCover => BearishDarkCloudCover(),
            PatternNameEnum.BearishDojiStar => BearishDojiStar(),
            PatternNameEnum.BearishDownsideGap3Methods => BearishDownsideGap3Methods(),
            PatternNameEnum.BearishDownsideTasukiGap => BearishDownsideTasukiGap(),
            PatternNameEnum.BearishDragonflyDoji => BearishDragonflyDoji(),
            PatternNameEnum.BearishEngulfing => BearishEngulfing(),
            PatternNameEnum.BearishEveningDojiStar => BearishEveningDojiStar(),
            PatternNameEnum.BearishEveningStar => BearishEveningStar(),
            PatternNameEnum.BearishFalling3Methods => BearishFalling3Methods(),
            PatternNameEnum.BearishGravestoneDoji => BearishGravestoneDoji(),
            PatternNameEnum.BearishHarami => BearishHarami(),
            PatternNameEnum.BearishIdentical3Crows => BearishIdentical3Crows(),
            PatternNameEnum.BearishHaramiCross => BearishHaramiCross(),
            PatternNameEnum.BearishInNeck => BearishInNeck(),
            PatternNameEnum.BearishKicking => BearishKicking(),
            PatternNameEnum.BearishLongBlackCandelstick => BearishLongBlackCandelstick(),
            PatternNameEnum.BearishMeetingLines => BearishMeetingLines(),
            PatternNameEnum.BearishOnNeck => BearishOnNeck(),
            PatternNameEnum.BearishSeparatingLines => BearishSeparatingLines(),
            PatternNameEnum.BearishShootingStar => BearishShootingStar(),
            PatternNameEnum.BearishSideBySideWhiteLines => BearishSideBySideWhiteLines(),
            PatternNameEnum.BearishThrusting => BearishThrusting(),
            PatternNameEnum.BearishTriStar => BearishTriStar(),
            PatternNameEnum.BearishTweezerTop => BearishTweezerTop(),
            PatternNameEnum.BearishUpsideGap2Crows => BearishUpsideGap2Crows(),
            PatternNameEnum.Bullish3InsideUp => Bullish3InsideUp(),
            PatternNameEnum.Bullish3OutsideUp => Bullish3OutsideUp(),
            PatternNameEnum.Bullish3StarsintheSouth => Bullish3StarsintheSouth(),
            PatternNameEnum.Bullish3WhiteSoldiers => Bullish3WhiteSoldiers(),
            PatternNameEnum.Bullish3LineStrike => Bullish3LineStrike(),
            PatternNameEnum.BullishBeltHold => BullishBeltHold(),
            PatternNameEnum.BullishBreakaway => BullishBreakaway(),
            PatternNameEnum.BullishConcealingBabySwallow => BullishConcealingBabySwallow(),
            PatternNameEnum.BullishDojiStar => BullishDojiStar(),
            PatternNameEnum.BullishDragonflyDoji => BullishDragonflyDoji(),
            PatternNameEnum.BullishEngulfing => BullishEngulfing(),
            PatternNameEnum.BullishGravestoneDoji => BullishGravestoneDoji(),
            PatternNameEnum.BullishHarami => BullishHarami(),
            PatternNameEnum.BullishHaramiCross => BullishHaramiCross(),
            PatternNameEnum.BullishHomingPigeon => BullishHomingPigeon(),
            PatternNameEnum.BullishInvertedHammer => BullishInvertedHammer(),
            PatternNameEnum.BullishKicking => BullishKicking(),
            PatternNameEnum.BullishLadderBottom => BullishLadderBottom(),
            PatternNameEnum.BullishLongWhiteCandlestick => BullishLongWhiteCandlestick(),
            PatternNameEnum.BullishMatHold => BullishMatHold(),
            PatternNameEnum.BullishMatchingLow => BullishMatchingLow(),
            PatternNameEnum.BullishMeetingLines => BullishMeetingLines(),
            PatternNameEnum.BullishMorningDojiStar => BullishMorningDojiStar(),
            PatternNameEnum.BullishMorningStar => BullishMorningStar(),
            PatternNameEnum.BullishPiercingLine => BullishPiercingLine(),
            PatternNameEnum.BullishRising3Methods => BullishRising3Methods(),
            PatternNameEnum.BullishSeparatingLines => BullishSeparatingLines(),
            PatternNameEnum.BullishSideBySideWhiteLines => BullishSideBySideWhiteLines(),
            PatternNameEnum.BullishStickSandwich => BullishStickSandwich(),
            PatternNameEnum.BullishTriStar => BullishTriStar(),
            PatternNameEnum.BullishTweezerBottom => BullishTweezerBottom(),
            PatternNameEnum.BullishUnique3RiverBottom => BullishUnique3RiverBottom(),
            PatternNameEnum.BullishUpsideGap3Methods => BullishUpsideGap3Methods(),
            PatternNameEnum.BullishUpsideTasukiGap => BullishUpsideTasukiGap(),
            PatternNameEnum.BullishWhiteClosingMarubozu => BullishWhiteClosingMarubozu(),
            PatternNameEnum.BullishWhiteMarubozu => BullishWhiteMarubozu(),
            PatternNameEnum.BullishWhiteOpeningMarubozu => BullishWhiteOpeningMarubozu()
        };*/

        /*public List<OhlcvObject> GetPatternsSignalsList(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.IgnoreCase| BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod != null)
            {
                List<OhlcvObject> result = (List<OhlcvObject>)theMethod.Invoke(this, null);
                return result;
            }
            else
            {
                return _data;
            }
        }*/

        /*public int GetSignalsCount(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            return GetPatternsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }*/

        /*public List<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Patterns).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }

            return methods.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();
        }*/
    }
}
