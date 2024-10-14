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

        public List<OhlcvObject> GetPatternsSignalsQuantities(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
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
            return GetPatternsSignalsQuantities(methodName).Where(x => x.Signal == true).Count();
        }

        public List<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Patterns).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }
    }
}
