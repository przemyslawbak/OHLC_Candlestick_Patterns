namespace Candlestick_Patterns
{
    public class Patterns
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

        public List<OhlcvObject> Bearish2Crows()
        {

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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bearish3BlackCrows()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bearish3InsideDown()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bearish3OutsideDown()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bearish3LineStrike()
        {
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
                                _data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishAdvanceBlock()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishBeltHold()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].High < _data[i - 0].Low)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].High && _data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishBlackClosingMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High > _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishBlackMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close == _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishBlackOpeningMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].High == _data[i - 0].Open && _data[i - 0].Close > _data[i - 0].Low && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishBreakaway()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDeliberation()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDarkCloudCover()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && _data[i - 0].Close < _data[i - 1].Open + ((_data[i - 1].Close - _data[i - 1].Open) / 2))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDojiStar()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maxDojiShadowSizes)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDownsideGap3Methods()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDownsideTasukiGap()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishDragonflyDoji()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minCandleShadowSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishEngulfing()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishEveningDojiStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishEveningStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishFalling3Methods()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishGravestoneDoji()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishHangingMan()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close)
                {
                    if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < (_maxDojiBodySize / 2) && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Open - _data[i - 0].Low > 2 * (_data[i - 0].Close - _data[i - 0].Open))
                    {
                        _data[i].Signal = true;
                    }
                }
                else
                {
                    if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].High) < (_maxDojiBodySize / 2) && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Close - _data[i - 0].Low > 2 * (_data[i - 0].Close - _data[i - 0].Open))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishHarami()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishIdentical3Crows()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishHaramiCross()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Open < _data[i - 1].Close && _data[i - 1].Open < _data[i - 0].Open)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishInNeck()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && _data[i - 0].Close > _data[i - 1].Close && 100 * (_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close < _inBarMaxChange)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishKicking()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && 100 * (_data[i - 1].Open - _data[i - 1].Low) / _data[i - 1].Open < _maxShadowSize && 100 * (_data[i - 1].High - _data[i - 1].Close) / _data[i - 1].Close < _maxShadowSize && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Open > _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maxShadowSize && 100 * (_data[i - 0].High - _data[i - 0].Open) / _data[i - 0].Open < _maxShadowSize && (-100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishLongBlackCandelstick()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open > _data[i - 0].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishMeetingLines()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].High && (100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minCandleSize) && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maxCloseDifference)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishOnNeck()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Close - _data[i - 1].Low) / _data[i - 0].Close) < _maxPriceDifference)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishSeparatingLines()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maxCloseDifference)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishShootingStar()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 1].High < _data[i - 0].Low && 3 * Math.Abs(_data[i - 0].Open - _data[i - 0].Close) < _data[i - 0].High - _data[i - 0].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxCandleBodySize && 100 * (Math.Min(_data[i - 0].Open, _data[i - 0].Close) - _data[i - 0].Low) / Math.Min(_data[i - 0].Open, _data[i - 0].Close) < _maxCandleShadowSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishSideBySideWhiteLines()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishThrusting()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishTriStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishTweezerTop()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open < _data[i - 1].Close && (100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open >= _data[i - 0].Close && _data[i - 0].High == _data[i - 1].High && (Math.Abs(100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maxShortCandleSize))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BearishUpsideGap2Crows()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bullish3InsideUp()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bullish3OutsideUp()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bullish3StarsintheSouth()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bullish3WhiteSoldiers()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> Bullish3LineStrike()
        {
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
                                _data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishBeltHold()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Low > _data[i - 0].High)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open == _data[i - 0].Low && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maxCloseHighChange && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishBreakaway()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishConcealingBabySwallow()
        {
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
                                _data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishDojiStar()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Low && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) < _maxDojiShadowSizes)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishDragonflyDoji()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (_data[i - 0].High - _data[i - 0].Low) / _data[i - 0].High) > _minCandleShadowSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishEngulfing()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open < _maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Open && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishGravestoneDoji()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Low == _data[i - 0].Open)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishHarami()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open > _data[i - 1].Close && _data[i - 0].Close < _data[i - 1].Open && (100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize))
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishHaramiCross()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxDojiBodySize && _data[i - 0].Close < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 1].Close < _data[i - 0].Open)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishHomingPigeon()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Open && _data[i - 0].Close > _data[i - 1].Close && -100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open < _maxShortCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishInvertedHammer()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].Close - _data[i - 1].Low) / _data[i - 1].Close < _maxShadowSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].High < _data[i - 1].Open && _data[i - 0].High - _data[i - 0].Low > 3 * Math.Abs(_data[i - 0].Close - _data[i - 0].Open) && 100 * (_data[i - 0].Close - _data[i - 0].Low) / _data[i - 0].Close < _maxShadowSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open) < _maxCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishKicking()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && 100 * (_data[i - 1].High - _data[i - 1].Open) / _data[i - 1].Open < _maxShadowSize && -100 * (_data[i - 1].Low - _data[i - 1].Close) / _data[i - 1].Close < _maxShadowSize && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 1].Open && _data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].High - _data[i - 0].Close) / _data[i - 0].Close < _maxShadowSize && -100 * (_data[i - 0].Low - _data[i - 0].Open) / _data[i - 0].Open < _maxShadowSize && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishLadderBottom()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishLongWhiteCandlestick()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishMatHold()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishMatchingLow()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open > _data[i - 0].Close && _data[i - 0].Close == _data[i - 1].Close)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishMeetingLines()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 1].Close && -100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Close > _minCandleSize && 100 * Math.Abs((_data[i - 0].Close - _data[i - 1].Close) / _data[i - 1].Close) < _maxCloseDifference)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishMorningDojiStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishMorningStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishPiercingLine()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && (-100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Open < _data[i - 1].Close && _data[i - 0].Close > _data[i - 1].Close + (_data[i - 1].Open - _data[i - 1].Close) / 2)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishRising3Methods()
        {
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
                                    _data[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishSeparatingLines()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 1].Open > _data[i - 1].Close && -100 * (_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (_data[i - 0].Open < _data[i - 0].Close && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize && 100 * Math.Abs((_data[i - 0].Open - _data[i - 1].Open) / _data[i - 1].Open) < _maxOpenDifference)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishSideBySideWhiteLines()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishStickSandwich()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishTriStar()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishTweezerBottom()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (100 * Math.Abs((_data[i - 1].Close - _data[i - 1].Open) / _data[i - 1].Open) > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (_data[i - 0].Open - _data[i - 0].Close) / _data[i - 0].Open) < _maxShortCandleSize && _data[i - 0].Low == _data[i - 1].Low)
                    {
                        _data[i].Signal = true;
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishUnique3RiverBottom()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishUpsideGap3Methods()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishUpsideTasukiGap()
        {
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
                            _data[i].Signal = true;
                        }
                    }
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishWhiteClosingMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open != _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishWhiteMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close == _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
        public List<OhlcvObject> BullishWhiteOpeningMarubozu()
        {
            for (int i = 5; i < _data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (_data[i - 0].Open < _data[i - 0].Close && _data[i - 0].Close < _data[i - 0].High && _data[i - 0].Open == _data[i - 0].Low && 100 * (_data[i - 0].Close - _data[i - 0].Open) / _data[i - 0].Open > _minCandleSize)
                {
                    _data[i].Signal = true;
                }
            }
            return _data;
        }
    }
}
