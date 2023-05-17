namespace Candlestick_Patterns
{
    public class Patterns
    {
        decimal _minCandleSize = 0.5M;

        public List<OhlcvObject> Bearish2Crows(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Low > data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 1].Close <= data[i - 0].Open && data[i - 0].Open <= data[i - 1].Open && data[i - 2].Open <= data[i - 0].Close && data[i - 0].Close <= data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3BlackCrows(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Close < data[i - 2].Close && data[i - 2].Close <= data[i - 1].Open && data[i - 1].Open <= data[i - 2].Open && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 1].Close < data[i - 0].Open && data[i - 0].Open < data[i - 1].Open && data[i - 0].Close < data[i - 1].Close && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3InsideDown(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Open < data[i - 2].Close && data[i - 1].Close > data[i - 2].Open && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close < data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3OutsideDown(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open < _maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Open > data[i - 2].Close && data[i - 1].Close < data[i - 2].Open && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close < data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bearish3LineStrike(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 3].Open > data[i - 3].Close && -100 * (data[i - 3].Close - data[i - 3].Open) / data[i - 3].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].Open > data[i - 2].Close && data[i - 3].Close < data[i - 2].Open && data[i - 2].Open < data[i - 3].Open && data[i - 2].Close < data[i - 3].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Close < data[i - 1].Open && data[i - 1].Open < data[i - 2].Open && data[i - 1].Close < data[i - 2].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                        {
                            if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open <= data[i - 1].Close && data[i - 0].Close >= data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishAdvanceBlock(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Close > data[i - 1].Open && data[i - 1].Open > data[i - 2].Open && data[i - 1].Close > data[i - 2].Close && ((100 + _minCandleDifference) / 100) * (data[i - 1].Close - data[i - 1].Open) < data[i - 2].Close - data[i - 2].Open && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close > data[i - 0].Open && data[i - 0].Open > data[i - 1].Open && data[i - 0].Close > data[i - 1].Close && ((100 + _minCandleDifference) / 100) * (data[i - 0].Close - data[i - 0].Open) < data[i - 1].Close - data[i - 1].Open && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBeltHold(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].High < data[i - 0].Low)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open == data[i - 0].High && data[i - 0].Open > data[i - 0].Close && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackClosingMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open > data[i - 0].Close && data[i - 0].High > data[i - 0].Open && data[i - 0].Close == data[i - 0].Low && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open > data[i - 0].Close && data[i - 0].High == data[i - 0].Open && data[i - 0].Close == data[i - 0].Low && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBlackOpeningMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open > data[i - 0].Close && data[i - 0].High == data[i - 0].Open && data[i - 0].Close > data[i - 0].Low && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishBreakaway(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 4].Open < data[i - 4].Close && 100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 3].Open < data[i - 3].Close && data[i - 4].High < data[i - 3].Low && 100 * (data[i - 3].Close - data[i - 3].Open) / data[i - 3].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 3].Close < data[i - 2].Close && Math.Abs((100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open)) < _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Close < data[i - 1].Close && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _minCandleSize)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close > data[i - 4].High && data[i - 0].Close < data[i - 3].Low)
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
        public List<OhlcvObject> BearishDeliberation(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Open < data[i - 1].Open && data[i - 2].Close < data[i - 1].Close && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close <= data[i - 0].Open && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < _minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDarkCloudCover(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open > data[i - 1].High && data[i - 0].Close < data[i - 1].Open + ((data[i - 1].Close - data[i - 1].Open) / 2))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDojiStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].High < data[i - 0].Low && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].High - data[i - 0].Low) / data[i - 0].High) < _maxDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDownsideGap3Methods(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Low > data[i - 1].High && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close < data[i - 0].Open && data[i - 1].Open > data[i - 0].Open && data[i - 2].Close < data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDownsideTasukiGap(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Low > data[i - 1].High && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close < data[i - 0].Open && data[i - 1].Open > data[i - 0].Open && data[i - 2].Low > data[i - 0].Close && data[i - 1].High < data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishDragonflyDoji(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].High - data[i - 0].Low) / data[i - 0].High) > _minCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEngulfing(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open > data[i - 1].Close && data[i - 0].Close < data[i - 1].Open && (-100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEveningDojiStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].High < data[i - 1].Low && Math.Abs(100 * (data[i - 1].Open - data[i - 1].Close) / data[i - 1].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 1].High - data[i - 1].Low) / data[i - 1].High) < _maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close > data[i - 2].Open && data[i - 0].Close < data[i - 2].Close && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishEveningStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].High < data[i - 1].Low && Math.Abs(100 * (data[i - 1].High - data[i - 1].Low) / data[i - 1].High) < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close > data[i - 2].Open && data[i - 0].Close < data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishFalling3Methods(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 4].Open > data[i - 4].Close && (-100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 3].Open < data[i - 3].Close && data[i - 3].Close < data[i - 4].Open && data[i - 4].Close < data[i - 3].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if ((data[i - 2].Open < data[i - 2].Close && data[i - 2].Close < data[i - 4].Open && data[i - 3].Close < data[i - 2].Close) || (data[i - 2].Open > data[i - 2].Close && data[i - 2].Open < data[i - 4].Open && data[i - 3].Close < data[i - 2].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Close < data[i - 4].Open && Math.Max(data[i - 2].Close, data[i - 2].Open) < data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close < data[i - 4].Close && (-100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize))
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
        public List<OhlcvObject> BearishGravestoneDoji(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].High < data[i - 0].Low && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Low == data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishHangingMan(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open < data[i - 0].Close)
                {
                    if (Math.Abs(100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].High) < (_maxDojiBodySize / 2) && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Open - data[i - 0].Low > 2 * (data[i - 0].Close - data[i - 0].Open))
                    {
                        data[i].Signal = true;
                    }
                }
                else
                {
                    if (Math.Abs(100 * (data[i - 0].High - data[i - 0].Open) / data[i - 0].High) < (_maxDojiBodySize / 2) && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Close - data[i - 0].Low > 2 * (data[i - 0].Close - data[i - 0].Open))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishHarami(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open < data[i - 1].Close && data[i - 0].Close > data[i - 1].Open && (-100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < _maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishIdentical3Crows(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && (100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open < -1 * _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Close == data[i - 1].Open && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < -1 * _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 1].Close == data[i - 0].Open && (100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < -1 * _minCandleSize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishHaramiCross(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Open < data[i - 1].Close && data[i - 1].Open < data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishInNeck(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open < data[i - 1].Low && data[i - 0].Close > data[i - 1].Close && 100 * (data[i - 0].Close - data[i - 1].Close) / data[i - 1].Close < _inBarMaxChange)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishKicking(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && 100 * (data[i - 1].Open - data[i - 1].Low) / data[i - 1].Open < _maxShadowSize && 100 * (data[i - 1].High - data[i - 1].Close) / data[i - 1].Close < _maxShadowSize && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 1].Open && data[i - 0].Open > data[i - 0].Close && 100 * (data[i - 0].Close - data[i - 0].Low) / data[i - 0].Close < _maxShadowSize && 100 * (data[i - 0].High - data[i - 0].Open) / data[i - 0].Open < _maxShadowSize && (-100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishLongBlackCandelstick(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open > data[i - 0].Close && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishMeetingLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open > data[i - 1].High && (100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Close > _minCandleSize) && 100 * Math.Abs((data[i - 0].Close - data[i - 1].Close) / data[i - 1].Close) < _maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishOnNeck(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open < data[i - 1].Low && Math.Abs(100 * (data[i - 0].Close - data[i - 1].Low) / data[i - 0].Close) < _maxPriceDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishSeparatingLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && 100 * Math.Abs((data[i - 0].Open - data[i - 1].Open) / data[i - 1].Open) < _maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishShootingStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].High < data[i - 0].Low && 3 * Math.Abs(data[i - 0].Open - data[i - 0].Close) < data[i - 0].High - data[i - 0].Low && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxCandleBodySize && 100 * (Math.Min(data[i - 0].Open, data[i - 0].Close) - data[i - 0].Low) / Math.Min(data[i - 0].Open, data[i - 0].Close) < _maxCandleShadowSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishSideBySideWhiteLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].High < data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 0].High < data[i - 2].Low && Math.Abs(100 * (data[i - 0].Open - data[i - 1].Open) / data[i - 0].Open) < _maxDifference && Math.Abs(100 * (data[i - 0].Close - data[i - 1].Close) / data[i - 0].Close) < _maxDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishThrusting(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open < data[i - 1].Close && data[i - 0].Close > data[i - 1].Close && data[i - 0].Close < data[i - 1].Close + (data[i - 1].Open - data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishTriStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (data[i - 2].Open - data[i - 2].Close) / data[i - 2].Open) < _maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (data[i - 1].Open - data[i - 1].Close) / data[i - 1].Open) < _maxDojiBodySize && data[i - 2].High < data[i - 1].Low && data[i - 0].High < data[i - 1].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if ((Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishTweezerTop(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open < data[i - 1].Close && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open >= data[i - 0].Close && data[i - 0].High == data[i - 1].High && (Math.Abs(100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open) < _maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BearishUpsideGap2Crows(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].High < data[i - 1].Low && data[i - 1].Open > data[i - 1].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open > data[i - 1].Open && data[i - 0].Close < data[i - 1].Close && data[i - 0].Close > data[i - 2].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3InsideUp(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && (-100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Open > data[i - 2].Close && data[i - 1].Close < data[i - 2].Open && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _maxShortCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if ((data[i - 0].Open < data[i - 0].Close && data[i - 0].Close > data[i - 1].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3OutsideUp(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && (-100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open < _maxShortCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Open < data[i - 2].Close && data[i - 1].Close > data[i - 2].Open && (100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close > data[i - 1].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3StarsintheSouth(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && 100 * (data[i - 2].High - data[i - 2].Open) / data[i - 2].Open < _maxShadowChange && 100 * (data[i - 2].Close - data[i - 2].Low) / data[i - 2].Low > _minCandleSize && (-100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && 100 * (data[i - 1].High - data[i - 1].Open) / data[i - 1].Open < _maxShadowChange && data[i - 1].Low > data[i - 2].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open < data[i - 1].High && data[i - 0].Close > data[i - 1].Low && 100 * (data[i - 0].High - data[i - 0].Open) / data[i - 0].Open < _maxShadowChange && 100 * (data[i - 0].Close - data[i - 0].Low) / data[i - 0].Low < _maxShadowChange)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3WhiteSoldiers(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].High - data[i - 2].Close) / data[i - 2].Close < _maxCloseHighChange && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Close > data[i - 1].Open && data[i - 1].Open > data[i - 2].Open && data[i - 1].Close > data[i - 2].Close && 100 * (data[i - 1].High - data[i - 1].Close) / data[i - 1].Close < _maxCloseHighChange && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close > data[i - 0].Open && data[i - 0].Open > data[i - 1].Open && data[i - 0].Close > data[i - 1].Close && 100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].Close < _maxCloseHighChange && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> Bullish3LineStrike(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 3].Open < data[i - 3].Close && 100 * (data[i - 3].High - data[i - 3].Close) / data[i - 3].Close < _maxCloseHighChange && 100 * (data[i - 3].Close - data[i - 3].Open) / data[i - 3].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].Open < data[i - 2].Close && data[i - 3].Close > data[i - 2].Open && data[i - 2].Open > data[i - 3].Open && data[i - 2].Close > data[i - 3].Close && 100 * (data[i - 2].High - data[i - 2].Close) / data[i - 2].Close < _maxCloseHighChange && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Close > data[i - 1].Open && data[i - 1].Open > data[i - 2].Open && data[i - 1].Close > data[i - 2].Close && 100 * (data[i - 1].High - data[i - 1].Close) / data[i - 1].Close < _maxCloseHighChange && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open > data[i - 1].Close && data[i - 0].Close < data[i - 3].Open)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishBeltHold(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Low > data[i - 0].High)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open == data[i - 0].Low && data[i - 0].Open < data[i - 0].Close && 100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].Close < _maxCloseHighChange && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishBreakaway(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 4].Open > data[i - 4].Close && (-100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 3].Open > data[i - 3].Close && data[i - 3].High < data[i - 4].Low && (-100 * (data[i - 3].Close - data[i - 3].Open) / data[i - 3].Open < _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 3].Close > data[i - 2].Close && Math.Abs((100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open)) < _minCandleSize)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Close > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _minCandleSize))
                            {
                                // Check whether the fifth candlestick matches. 
                                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close < data[i - 4].Low && data[i - 0].Close > data[i - 3].High)
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
        public List<OhlcvObject> BullishConcealingBabySwallow(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 3].Open > data[i - 3].Close && data[i - 3].High == data[i - 3].Open && data[i - 3].Low == data[i - 3].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 2].Open > data[i - 2].Close && data[i - 2].High == data[i - 2].Open && data[i - 2].Low == data[i - 2].Close)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Open < data[i - 2].Close && data[i - 1].High > data[i - 2].Close)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open >= data[i - 1].High && data[i - 0].Close < data[i - 1].Low)
                            {
                                data[i].Signal = true;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishDojiStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].High < data[i - 1].Low && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].High - data[i - 0].Low) / data[i - 0].High) < _maxDojiShadowSizes)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishDragonflyDoji(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].High) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 0].High - data[i - 0].Low) / data[i - 0].High) > _minCandleShadowSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishEngulfing(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open < _maxShortCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open < data[i - 1].Close && data[i - 0].Close > data[i - 1].Open && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishGravestoneDoji(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].High < data[i - 1].Open && Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Low == data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHammer(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (maxBodyPrice == data[i - 0].High && minBodyPrice - data[i - 0].Low > 2 * (maxBodyPrice - minBodyPrice) && 100 * Math.Abs((data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open) > _maxCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHarami(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open > data[i - 1].Close && data[i - 0].Close < data[i - 1].Open && (100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < _maxShortCandleSize))
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHaramiCross(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize && data[i - 0].Close < data[i - 1].Open && data[i - 1].Close < data[i - 0].Close && data[i - 0].Open < data[i - 1].Open && data[i - 1].Close < data[i - 0].Open)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishHomingPigeon(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Open < data[i - 1].Open && data[i - 0].Close > data[i - 1].Close && -100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < _maxShortCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishInvertedHammer(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && 100 * (data[i - 1].Close - data[i - 1].Low) / data[i - 1].Close < _maxShadowSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].High < data[i - 1].Open && data[i - 0].High - data[i - 0].Low > 3 * Math.Abs(data[i - 0].Close - data[i - 0].Open) && 100 * (data[i - 0].Close - data[i - 0].Low) / data[i - 0].Close < _maxShadowSize && 100 * Math.Abs((data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open) < _maxCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishKicking(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && 100 * (data[i - 1].High - data[i - 1].Open) / data[i - 1].Open < _maxShadowSize && -100 * (data[i - 1].Low - data[i - 1].Close) / data[i - 1].Close < _maxShadowSize && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 1].Open && data[i - 0].Open < data[i - 0].Close && 100 * (data[i - 0].High - data[i - 0].Close) / data[i - 0].Close < _maxShadowSize && -100 * (data[i - 0].Low - data[i - 0].Open) / data[i - 0].Open < _maxShadowSize && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishLadderBottom(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if ((data[i - 4].Open > data[i - 4].Close && -100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if ((data[i - 3].Open > data[i - 3].Close && data[i - 3].Close < data[i - 4].Close && data[i - 4].Close <= data[i - 3].Open && data[i - 3].Open <= data[i - 4].Open && -100 * (data[i - 3].Close - data[i - 3].Open) / data[i - 3].Open > _minCandleSize))
                    {
                        // Check whether the third candlestick matches. 
                        if ((data[i - 2].Open > data[i - 2].Close && data[i - 3].Close < data[i - 2].Open && data[i - 2].Open < data[i - 3].Open && data[i - 2].Close < data[i - 3].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize))
                        {
                            // Check whether the fourth candlestick matches. 
                            if ((data[i - 1].Open > data[i - 1].Close && data[i - 1].High - data[i - 1].Open > 2 * (data[i - 1].Open - data[i - 1].Close)))
                            {
                                // Check whether the fifth candlestick matches. 
                                if ((data[i - 0].Open < data[i - 0].Close && data[i - 0].Open > data[i - 1].Open))
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
        public List<OhlcvObject> BullishLongWhiteCandlestick(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open < data[i - 0].Close && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMatHold(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 4].Open < data[i - 4].Close && 100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 3].Open > data[i - 3].Close && data[i - 4].Close < data[i - 3].Close && -100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if ((data[i - 2].Open < data[i - 2].Close && data[i - 2].Close < data[i - 3].Open && data[i - 2].Open > data[i - 4].Open) || (data[i - 2].Open > data[i - 2].Close && data[i - 2].Close > data[i - 4].Open && data[i - 2].Open < data[i - 3].Open))
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 1].Open > data[i - 1].Close && data[i - 2].Close > data[i - 4].Open && data[i - 2].Open < data[i - 3].Open)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open > data[i - 1].Open && data[i - 0].Close > data[i - 4].Close && data[i - 0].Close > data[i - 3].Open)
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
        public List<OhlcvObject> BullishMatchingLow(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open > data[i - 0].Close && data[i - 0].Close == data[i - 1].Close)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMeetingLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close < data[i - 1].Close && -100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Close > _minCandleSize && 100 * Math.Abs((data[i - 0].Close - data[i - 1].Close) / data[i - 1].Close) < _maxCloseDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMorningDojiStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].High < data[i - 2].Low && Math.Abs(100 * (data[i - 1].Open - data[i - 1].Close) / data[i - 1].Open) < _maxDojiBodySize && Math.Abs(100 * (data[i - 1].High - data[i - 1].Low) / data[i - 1].High) < _maxDojiShadowSizes)
                    {
                        // Check whether the third candlestick matches.  
                        if ((data[i - 0].Open < data[i - 0].Close && data[i - 1].Close < data[i - 0].Close))
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishMorningStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].High < data[i - 2].Low && Math.Abs(100 * (data[i - 1].Open - data[i - 1].Close) / data[i - 1].Open) < _maxShortCandleSize)
                    {
                        // Check whether the third candlestick matches.  
                        if (data[i - 0].Open < data[i - 0].Close && data[i - 1].Close < data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishPiercingLine(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && (-100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize))
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open < data[i - 1].Close && data[i - 0].Close > data[i - 1].Close + (data[i - 1].Open - data[i - 1].Close) / 2)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishRising3Methods(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 4].Open < data[i - 4].Close && 100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 3].Open > data[i - 3].Close && data[i - 4].High > data[i - 3].Open && data[i - 4].Low < data[i - 3].Close && -100 * (data[i - 4].Close - data[i - 4].Open) / data[i - 4].Open < _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 2].Open < data[i - 3].Open && data[i - 2].Close < data[i - 3].Open && data[i - 4].High > data[i - 2].Open && data[i - 4].High > data[i - 2].Close && data[i - 4].Low < data[i - 2].Close && data[i - 4].Low < data[i - 2].Open)
                        {
                            // Check whether the fourth candlestick matches. 
                            if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Close < Math.Min(data[i - 2].Open, data[i - 2].Close) && data[i - 4].High > data[i - 1].Open && data[i - 4].Low < data[i - 1].Close)
                            {
                                // Check whether the fifth candlestick matches. 
                                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Open > data[i - 1].Close && data[i - 0].Close > data[i - 4].Close)
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
        public List<OhlcvObject> BullishSeparatingLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 1].Open > data[i - 1].Close && -100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 0].Open < data[i - 0].Close && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize && 100 * Math.Abs((data[i - 0].Open - data[i - 1].Open) / data[i - 1].Open) < _maxOpenDifference)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishSideBySideWhiteLines(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Open > data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open < data[i - 0].Close && 100 * Math.Abs((data[i - 0].Open - data[i - 1].Open) / data[i - 1].Open) < _maxDifference && 100 * Math.Abs((data[i - 0].Close - data[i - 1].Close) / data[i - 1].Close) < _maxDifference)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishStickSandwich(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 2].Close < data[i - 1].Close)
                    {
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 2].Close == data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishTriStar(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (Math.Abs(100 * (data[i - 2].Open - data[i - 2].Close) / data[i - 2].Open) < _maxDojiBodySize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (data[i - 1].Open - data[i - 1].Close) / data[i - 1].Open) < _maxDojiBodySize && data[i - 1].High < data[i - 2].Low && data[i - 1].High < data[i - 0].Low)
                    {
                        // Check whether the third candlestick matches. 
                        if (Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxDojiBodySize)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishTweezerBottom(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (100 * Math.Abs((data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open) > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (Math.Abs(100 * (data[i - 0].Open - data[i - 0].Close) / data[i - 0].Open) < _maxShortCandleSize && data[i - 0].Low == data[i - 1].Low)
                    {
                        data[i].Signal = true;
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUnique3RiverBottom(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open > data[i - 2].Close && -100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open > data[i - 1].Close && data[i - 1].Open == data[i - 1].High && 100 * (data[i - 1].Close - data[i - 1].Low) / data[i - 1].Close > _minCandleSize)
                    {
                        // Check whether the third candlestick matches.  
                        if (data[i - 0].Open < data[i - 0].Close && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open < _maxShortCandleSize && data[i - 1].Close > data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUpsideGap3Methods(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close && 100 * (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Low > data[i - 2].High && 100 * (data[i - 1].Close - data[i - 1].Open) / data[i - 1].Open > _minCandleSize)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 1].Open < data[i - 0].Open && data[i - 2].Close > data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishUpsideTasukiGap(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 2].Open < data[i - 2].Close)
                {
                    // Check whether the second candlestick matches. 
                    if (data[i - 1].Open < data[i - 1].Close && data[i - 1].Low > data[i - 2].High)
                    {
                        // Check whether the third candlestick matches. 
                        if (data[i - 0].Open > data[i - 0].Close && data[i - 1].Open < data[i - 0].Open && data[i - 0].Open < data[i - 1].Close && data[i - 1].Open > data[i - 0].Close && data[i - 2].Close < data[i - 0].Close)
                        {
                            data[i].Signal = true;
                        }
                    }
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteClosingMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close == data[i - 0].High && data[i - 0].Open != data[i - 0].Low && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close == data[i - 0].High && data[i - 0].Open == data[i - 0].Low && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
        public List<OhlcvObject> BullishWhiteOpeningMarubozu(List<OhlcvObject> data)
        {
            for (int i = 5; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (data[i - 0].Open < data[i - 0].Close && data[i - 0].Close < data[i - 0].High && data[i - 0].Open == data[i - 0].Low && 100 * (data[i - 0].Close - data[i - 0].Open) / data[i - 0].Open > _minCandleSize)
                {
                    data[i].Signal = true;
                }
            }
            return data;
        }
    }
}
