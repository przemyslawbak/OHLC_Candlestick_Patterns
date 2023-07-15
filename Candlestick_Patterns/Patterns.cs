using System.Reflection;

namespace Candlestick_Patterns
{
    public class Patterns : IPatterns
    {
        private readonly decimal _minCandleSize;
        private readonly decimal _maxShortCandleSize;
        private readonly decimal _minCandleDifference;
        private readonly decimal _maxDojiShadowSizes;
        private readonly decimal _maxDojiBodySize;
        private readonly decimal _minCandleShadowSize;
        private readonly decimal _inBarMaxChange;
        private readonly decimal _maxShadowSize;
        private readonly decimal _maxPriceDifference;
        private readonly decimal _maxCloseDifference;
        private readonly decimal _maxCandleBodySize;
        private readonly decimal _maxCandleShadowSize;
        private readonly decimal _maxDifference;
        private readonly decimal _maxShadowChange;
        private readonly decimal _maxCloseHighChange;
        private readonly decimal _maxCandleSize;
        private readonly decimal _maxOpenDifference;

        private readonly List<OhlcvObject> _data;
        public Patterns(List<OhlcvObject> data)
        {
            _data = data;
            _minCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
            _maxShortCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _minCandleDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxDojiShadowSizes = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxDojiBodySize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _minCandleShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
            _maxShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maxPriceDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maxCloseDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maxCandleBodySize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxCandleShadowSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxShadowChange = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxCloseHighChange = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 3;
            _maxCandleSize = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) / 2;
            _maxOpenDifference = data.Select(x => Math.Abs(x.Open - x.Close)).Average(x => x) * 2;
        }

        private List<OhlcvObject> Bearish2Crows()
        {
            var data = _data.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = false, Volume = x.Volume }).ToList();

            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < _data[i - 2].Close && _data[i - 2].Close <= _data[i - 1].Open && _data[i - 1].Open <= _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close < _data[i - 0].Open && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < _maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open > _data[i - 3].Close && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Open && _data[i - 1].Open < _data[i - 2].Open && _data[i - 1].Close < _data[i - 2].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && ((100 + _minCandleDifference) / 100) * (_data[i - 1].Close - _data[i - 1].Open) < _data[i - 2].Close - _data[i - 2].Open && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && ((100 + _minCandleDifference) / 100) * (_data[i - 0].Close - _data[i - 0].Open) < _data[i - 1].Close - _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].High < _data[i - 0].Low)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].High && _data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High > _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close > _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 4].High < _data[i - 3].Low && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 3].Close < _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _minCandleSize)
                            {
                                // Check whether the fifth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Open < _data[i - 1].Open && _data[i - 2].Close < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close <= _data[i - 0].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maxDojiShadowSizes)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Low > _data[i - 1].High && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minCandleShadowSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close > _data[i - 2].Open && _data[i - 0].Close < _data[i - 2].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open < _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Open && _data[i - 4].Close < _data[i - 3].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Close) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Open < _data[i - 4].Open && _data[i - 3].Close < _data[i - 2].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Close < _data[i - 4].Open && Math.Max(_data[i - 2].Close, _data[i - 2].Open) < _data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close < _data[i - 4].Close && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < -1 * _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close == _data[i - 1].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < -1 * _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 1].Close == _data[i - 0].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < -1 * _minCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 0].Open)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close < _inBarMaxChange)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Open - _data[i - 1].Low) / _data[i - 1].Open < _maxShadowSize && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maxShadowSize && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Open > _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maxShadowSize && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < _maxShadowSize && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && (100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minCandleSize) && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maxCloseDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Low) / _data[i - 0].Close) < _maxPriceDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maxCloseDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && 3 * Math.Abs(_data[i - 0].Open - _data[i - 0].Close) < _data[i - 0].High - _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxCandleBodySize && 100 * (Math.Min(_data[i - 0].Open, _data[i - 0].Close) - _data[i - 0].Low) / Math.Min(_data[i - 0].Open, _data[i - 0].Close) < _maxCandleShadowSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].High < _data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 1].Open) / _data[i - 0].Open) < _maxDifference && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 0].Close) < _maxDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < _maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maxDojiBodySize && _data[i - 2].High < _data[i - 1].Low && _data[i - 0].High < _data[i - 1].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if ((Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open >= _data[i - 0].Close && _data[i - 0].High == _data[i - 1].High && (Math.Abs(100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maxShortCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].High < _data[i - 1].Low && _data[i - 1].Open > _data[i - 1].Close)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].Close && _data[i - 1].Close < _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize))
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open < _maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].Close > _data[i - 2].Open && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Open) / _data[i - 2].Open < _maxShadowChange && 100 * (_data[i - 2].Close - _data[i - 2].Low) / _data[i - 2].Low > _minCandleSize && (-100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < _maxShadowChange && _data[i - 1].Low > _data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].High && _data[i - 0].Close > _data[i - 1].Low && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < _maxShadowChange && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Low < _maxShadowChange)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < _maxCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maxCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 1].Close > _data[i - 0].Open && _data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maxCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open < _data[i - 3].Close && 100 * (_data[i - 3].High - _data[i - 3].Close) / _data[i - 3].Close < _maxCloseHighChange && 100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open < _data[i - 2].Close && _data[i - 3].Close > _data[i - 2].Open && _data[i - 2].Open > _data[i - 3].Open && _data[i - 2].Close > _data[i - 3].Close && 100 * (_data[i - 2].High - _data[i - 2].Close) / _data[i - 2].Close < _maxCloseHighChange && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Open && _data[i - 1].Open > _data[i - 2].Open && _data[i - 1].Close > _data[i - 2].Close && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maxCloseHighChange && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Low > _data[i - 0].High)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].Low && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maxCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open > _data[i - 4].Close && (-100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High < _data[i - 4].Low && (-100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open < _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 3].Close > _data[i - 2].Close && Math.Abs((100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open)) < _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _minCandleSize))
                            {
                                // Check whether the fifth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].High == _data[i - 3].Open && _data[i - 3].Low == _data[i - 3].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].High == _data[i - 2].Open && _data[i - 2].Low == _data[i - 2].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open < _data[i - 2].Close && _data[i - 1].High > _data[i - 2].Close)
                        {
                            // Check whether the fourth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maxDojiShadowSizes)
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
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minCandleShadowSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize))
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Close < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Open)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close < _maxShadowSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && _data[i - 0].High - _data[i - 0].Low > 3 * Math.Abs(_data[i - 0].Close - _data[i - 0].Open) && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maxShadowSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maxCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < _maxShadowSize && -100 * (_data[i - 1].Low - _data[i - 1].Close) / _data[i - 1].Close < _maxShadowSize && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maxShadowSize && -100 * (_data[i - 0].Low - _data[i - 0].Open) / _data[i - 0].Open < _maxShadowSize && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if ((_data[i - 4].Open > _data[i - 4].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if ((_data[i - 3].Open > _data[i - 3].Close && _data[i - 3].Close < _data[i - 4].Close && _data[i - 4].Close <= _data[i - 3].Open && _data[i - 3].Open <= _data[i - 4].Open && -100 * (_data[i - 3].Close - _data[i - 3].Open) / _data[i - 3].Open > _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open > _data[i - 2].Close && _data[i - 3].Close < _data[i - 2].Open && _data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize))
                        {
                            // Check whether the fourth candlestick matches. 
                            if ((_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].High - _data[i - 1].Open > 2 * (_data[i - 1].Open - _data[i - 1].Close)))
                            {
                                // Check whether the fifth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].Close < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if ((_data[i - 2].Open < _data[i - 2].Close && _data[i - 2].Close < _data[i - 3].Open && _data[i - 2].Open > _data[i - 4].Open) || (_data[i - 2].Open > _data[i - 2].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 2].Close > _data[i - 4].Open && _data[i - 2].Open < _data[i - 3].Open)
                            {
                                // Check whether the fifth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minCandleSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maxCloseDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 1].High - _data[i - 1].Low) / _data[i - 1].High) < _maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches.  
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 2].Low && Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches.  
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 4].Open < _data[i - 4].Close && 100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 3].Open > _data[i - 3].Close && _data[i - 4].High > _data[i - 3].Open && _data[i - 4].Low < _data[i - 3].Close && -100 * (_data[i - 4].Close - _data[i - 4].Open) / _data[i - 4].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 2].Open < _data[i - 3].Open && _data[i - 2].Close < _data[i - 3].Open && _data[i - 4].High > _data[i - 2].Open && _data[i - 4].High > _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Close && _data[i - 4].Low < _data[i - 2].Open)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Close < Math.Min(_data[i - 2].Open, _data[i - 2].Close) && _data[i - 4].High > _data[i - 1].Open && _data[i - 4].Low < _data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maxOpenDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Open > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maxDifference && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maxDifference)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 2].Open - _data[i - 2].Close) / _data[i - 2].Open) < _maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 1].Open - _data[i - 1].Close) / _data[i - 1].Open) < _maxDojiBodySize && _data[i - 1].High < _data[i - 2].Low && _data[i - 1].High < _data[i - 0].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize)
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
                // Check whether the first candlestick matches. 
                if (100 * Math.Abs((_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open) > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxShortCandleSize && _data[i - 0].Low == _data[i - 1].Low)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open > _data[i - 2].Close && -100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open > _data[i - 1].Close && _data[i - 1].Open == _data[i - 1].High && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close > _minCandleSize)
                    {
                        // Check whether the third candlestick matches.  
                        if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize && _data[i - 1].Close > _data[i - 0].Close)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close && 100 * (_data[i - 2].Close - _data[i - 2].Open) / _data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 2].Open < _data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].Open < _data[i - 1].Close && _data[i - 1].Low > _data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open != _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
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
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }

        public List<OhlcvObject> GetSignals(string patternMethodName)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(patternMethodName);

            List<OhlcvObject> result = (List<OhlcvObject>)theMethod.Invoke(this, null);
            return result;
        }

        public int GetSignalsCount(string patternMethodName)
        {
            return GetSignals(patternMethodName).Where(x => x.Signal == true).Count();
        }

        public IEnumerable<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();

            foreach (MethodInfo item in typeof(Patterns).GetMethods())
            {
                methods.Add(item.Name);
            }

            return methods;
        }
    }
}
