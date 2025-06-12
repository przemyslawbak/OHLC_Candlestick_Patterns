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
            _insideBarMaxChange = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _minimumCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
            _maximumShortCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _minimumCandleDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumDojiShadowSizes = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumDojiBodySize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _minimumCandleShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
            _maximumShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maximumPriceDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maximumCloseDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maximumCandleBodySize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumCandleShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumShadowChange = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumCloseHighChange = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maximumCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maximumOpenDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
        }

        private List<OhlcvObject> Bearish2Crows()
        {
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

        private List<OhlcvObject> Bearish3BlackCrows()
        {
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

        private List<OhlcvObject> Bearish3InsideDown()
        {
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
        private List<OhlcvObject> Bearish3OutsideDown()
        {
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

        private List<OhlcvObject> Bearish3LineStrike()
        {
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

        private List<OhlcvObject> BearishAdvanceBlock()
        {
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

        private List<OhlcvObject> BearishBeltHold()
        {
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

        private List<OhlcvObject> BearishBlackClosingMarubozu()
        {
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

        private List<OhlcvObject> BearishBlackMarubozu()
        {
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

        private List<OhlcvObject> BearishBlackOpeningMarubozu()
        {
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

        private List<OhlcvObject> BearishBreakaway()
        {
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

        private List<OhlcvObject> BearishDeliberation()
        {
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

        private List<OhlcvObject> BearishDarkCloudCover()
        {
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

        private List<OhlcvObject> BearishDojiStar()
        {
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

        private List<OhlcvObject> BearishDownsideGap3Methods()
        {
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

        private List<OhlcvObject> BearishDownsideTasukiGap()
        {
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

        private List<OhlcvObject> BearishDragonflyDoji()
        {
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

        private List<OhlcvObject> BearishEngulfing()
        {
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

        private List<OhlcvObject> BearishEveningDojiStar()
        {
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

        private List<OhlcvObject> BearishEveningStar()
        {
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

        private List<OhlcvObject> BearishFalling3Methods()
        {
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

        private List<OhlcvObject> BearishGravestoneDoji()
        {
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

        private List<OhlcvObject> BearishHarami()
        {
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

        private List<OhlcvObject> BearishIdentical3Crows()
        {
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

        private List<OhlcvObject> BearishHaramiCross()
        {
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

        private List<OhlcvObject> BearishInNeck()
        {
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

        private List<OhlcvObject> BearishKicking()
        {
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

        private List<OhlcvObject> BearishLongBlackCandelstick()
        {
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

        private List<OhlcvObject> BearishMeetingLines()
        {
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

        private List<OhlcvObject> BearishOnNeck()
        {
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

        private List<OhlcvObject> BearishSeparatingLines()
        {
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

        private List<OhlcvObject> BearishShootingStar()
        {
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

        private List<OhlcvObject> BearishSideBySideWhiteLines()
        {
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

        private List<OhlcvObject> BearishThrusting()
        {
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

        private List<OhlcvObject> BearishTriStar()
        {
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

        private List<OhlcvObject> BearishTweezerTop()
        {
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

        private List<OhlcvObject> BearishUpsideGap2Crows()
        {
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

        private List<OhlcvObject> Bullish3InsideUp()
        {
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

        private List<OhlcvObject> Bullish3OutsideUp()
        {
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

        private List<OhlcvObject> Bullish3StarsintheSouth()
        {
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

        private List<OhlcvObject> Bullish3WhiteSoldiers()
        {
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

        private List<OhlcvObject> Bullish3LineStrike()
        {
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

        private List<OhlcvObject> BullishBeltHold()
        {
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

        private List<OhlcvObject> BullishBreakaway()
        {
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

        private List<OhlcvObject> BullishConcealingBabySwallow()
        {
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

        private List<OhlcvObject> BullishDojiStar()
        {
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

        private List<OhlcvObject> BullishDragonflyDoji()
        {
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

        private List<OhlcvObject> BullishEngulfing()
        {
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

        private List<OhlcvObject> BullishGravestoneDoji()
        {
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

        private List<OhlcvObject> BullishHarami()
        {
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

        private List<OhlcvObject> BullishHaramiCross()
        {
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

        private List<OhlcvObject> BullishHomingPigeon()
        {
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

        private List<OhlcvObject> BullishInvertedHammer()
        {
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

        private List<OhlcvObject> BullishKicking()
        {
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

        private List<OhlcvObject> BullishLadderBottom()
        {
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

        private List<OhlcvObject> BullishLongWhiteCandlestick()
        {
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

        private List<OhlcvObject> BullishMatHold()
        {
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

        private List<OhlcvObject> BullishMatchingLow()
        {
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

        private List<OhlcvObject> BullishMeetingLines()
        {
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

        private List<OhlcvObject> BullishMorningDojiStar()
        {
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

        private List<OhlcvObject> BullishMorningStar()
        {
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

        private List<OhlcvObject> BullishPiercingLine()
        {
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

        private List<OhlcvObject> BullishRising3Methods()
        {
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

        private List<OhlcvObject> BullishSeparatingLines()
        {
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

        private List<OhlcvObject> BullishSideBySideWhiteLines()
        {
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

        private List<OhlcvObject> BullishStickSandwich()
        {
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

        private List<OhlcvObject> BullishTriStar()
        {
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

        private List<OhlcvObject> BullishTweezerBottom()
        {
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

        private List<OhlcvObject> BullishUnique3RiverBottom()
        {
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

        private List<OhlcvObject> BullishUpsideGap3Methods()
        {
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

        private List<OhlcvObject> BullishUpsideTasukiGap()
        {
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

        private List<OhlcvObject> BullishWhiteClosingMarubozu()
        {
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

        private List<OhlcvObject> BullishWhiteMarubozu()
        {
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

        private List<OhlcvObject> BullishWhiteOpeningMarubozu()
        {
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
        public Dictionary<PatternNameEnum, List<OhlcvObject>> GetAllPatterns()
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
        }
        public List<OhlcvObject> GetPatternsSignalsList(PatternNameEnum patternName) => patternName switch {
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
        };

        public List<OhlcvObject> GetPatternsSignalsList(string patternName)
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
        }

        public int GetSignalsCount(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            return GetPatternsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }

        public List<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Patterns).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }

            return methods.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();
        }
    }
}
