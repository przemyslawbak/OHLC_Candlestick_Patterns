using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    public class Formations : IFormations
    {
        private readonly List<OhlcvObject> _dataOhlcv;
        private readonly List<ZigZagObject> _data;
        private readonly decimal _percentageMargin;
        private readonly List<int> _formationsLenght;
        private readonly decimal _minShift;
        private readonly List<double> _advance;
        private List<ZigZagObject> _peaksFromZigZag;

        public Formations(List<OhlcvObject> dataOhlcv)
        {
            _dataOhlcv = dataOhlcv;
            _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, 0.002M);
            _percentageMargin = (decimal) 0.035; 
            _formationsLenght = new List<int>() { 4, 7 };
            _minShift = 2;
            _advance = new List<double>() { 0.10, 0.20 };
        }

        public Formations(List<OhlcvObject> dataOhlcv, int zigZagParam)
        {
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
        }

        private List<ZigZagObject> BearishDoubleTops()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 4; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close < points[i - 2].Close && points[i - 2].Close > points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 1].Close - points[i - 3].Close)) / points[i - 3].Close);
                        if (points[i - 3].Close > neck && points[i - 1].Close > neck && change < _percentageMargin)
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count > _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishTripleTops() 
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 6].Close < points[i - 5].Close && points[i - 4].Close < points[i - 3].Close && points[i - 2].Close < points[i - 3].Close && points[i].Close < points[i - 1].Close)
                    {
                        var neck = new List<decimal>() { points[i - 2].Close, points[i - 4].Close };
                        var change = (Math.Abs((points[i - 1].Close - points[i - 2].Close)) / points[i - 2].Close);
                        var change1 = (Math.Abs((points[i - 3].Close - points[i - 4].Close)) / points[i - 4].Close);
                        var diff1 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);
                        var diff2 = Math.Abs((points[i - 5].Close - points[i - 3].Close) / points[i - 5].Close);
                        if (Math.Abs((neck.Min() - neck.Average()) / neck.Min()) < _percentageMargin)
                        {
                            if (points[i - 5].Close > neck.Average() && points[i - 3].Close > neck.Average() && change < (decimal)_advance.Max())
                            {
                                if (points[i - 1].Close > neck.Average() && change1 < _percentageMargin && Math.Abs(diff1 - diff2) < _percentageMargin)
                                {
                                    if (points[i - 6].Close < neck.Min() && points[i].Close < neck.Min())
                                    {
                                        for (int x = -6; x < 1; x++)
                                        {
                                            dateList.Add(points[i + x].Close);
                                        }

                                        if (dateList.Count >= _formationsLenght.Max())
                                        {
                                            points[i].Signal = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishDoubleBottoms()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 4; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close > points[i - 2].Close && points[i - 2].Close < points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 3].Close - points[i - 1].Close)) / points[i - 3].Close);

                        if (Math.Abs((points[i - 3].Close - neck) / points[i - 3].Close) < (decimal)_advance.Max() && Math.Abs((points[i - 1].Close - neck) / neck) < (decimal)_advance.Max() && points[i - 1].Close < neck && change < _percentageMargin)
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count > _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishTripleBottoms()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i].Close > points[i - 5].Close && points[i - 4].Close > points[i - 3].Close && points[i - 2].Close > points[i - 1].Close)
                    {
                        if (points[i - 6].Close > points[i - 4].Close && points[i - 4].Close < points[i - 2].Close)
                        {
                            var neck = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                            var change = (Math.Abs((points[i - 5].Close - points[i - 3].Close)) / points[i - 5].Close);
                            var change2 = (Math.Abs((points[i - 1].Close - points[i - 3].Close)) / points[i - 3].Close);
                            var diff1 = Math.Abs((points[i - 5].Close - neck.Average()) / points[i - 5].Close);
                            var diff2 = Math.Abs((points[i - 1].Close - neck.Average()) / points[i - 1].Close);
                            if (Math.Abs(diff1 - diff2) < _percentageMargin && change < _percentageMargin && change2 < _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }
        private List<ZigZagObject> BearishHeadAndShoulders()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 3].Close > points[i - 4].Close && points[i - 4].Close < points[i - 5].Close && points[i - 6].Close < points[i - 5].Close && points[i - 3].Close > points[i - 2].Close && points[i - 2].Close < points[i - 1].Close && points[i - 1].Close > points[i].Close)
                    {
                        var neckline = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                        var change = Math.Abs((neckline.Max() - neckline.Min()) / neckline.Min());
                        if (change < _percentageMargin && points[i - 6].Close < neckline.Min() && points[i].Close < neckline.Min())
                        {
                            for (int x = -6; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count >= _formationsLenght.Max())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishCupAndHandle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 13; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if(points[i].Close> points[i - 1].Close && points[i - 1].Close< points[i - 3].Close && points[i - 5].Close < points[i - 3].Close && points[i - 4].Close > points[i -2].Close && points[i - 4].Close < points[i].Close && points[i - 5].Close< points[i - 3].Close)
                    {
                        if(points[i - 7].Close < points[i - 5].Close && points[i - 7].Close > points[i - 9].Close && points[i - 6].Close > points[i - 8].Close && points[i - 11].Close > points[i - 9].Close && points[i - 13].Close> points[i - 11].Close)
                        {
                            var diff1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff2 = Math.Abs(points[i - 5].Close - points[i - 4 ].Close);
                            var diff3 = Math.Abs(points[i - 5].Close - points[i - 6].Close);
                            var diff4 = Math.Abs(points[i - 7].Close - points[i - 6].Close);
                            var change1 = Math.Abs((points[i - 8].Close - points[i - 10].Close) / points[i - 10].Close);
                            var change2 = Math.Abs((points[i - 7].Close - points[i - 11].Close) / points[i - 11].Close);

                            if (diff2 >= _minShift * diff1 && diff2 > diff3 && diff3 < diff4 && change1 < _percentageMargin && change2 < _percentageMargin)
                            {
                                for (int x = -13; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _minShift * _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    
                    }
                }
            }

            return points;
        }
        
        private List<ZigZagObject> BearishInverseCupAndHandle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 11; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if(points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 3].Close && points[i - 5].Close < points[i - 3].Close && points[i - 4].Close < points[i -2].Close && points[i - 4].Close > points[i].Close && points[i - 8].Close > points[i - 5].Close && points[i - 10].Close > points[i - 5].Close && points[i - 9].Close > points[i - 10].Close && points[i - 9].Close > points[i - 8].Close)
                    {
                        if(points[i - 7].Close > points[i - 4].Close && points[i - 7].Close < points[i - 9].Close && points[i - 8].Close < points[i - 11].Close && points[i - 11].Close > points[i - 9].Close && points[i].Close < points[i - 11].Close)
                        {
                            var diffHandle1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diffHandle2 = Math.Abs(points[i - 4].Close - points[i - 3].Close);

                            var change1 = Math.Abs(points[i - 8].Close - points[i - 7].Close);
                            var change2 = Math.Abs(points[i - 10].Close - points[i - 11].Close);

                            if (change1 >= diffHandle1 && change2 > diffHandle2 && Math.Abs(change1 - change2)/change2 < _percentageMargin && Math.Abs(diffHandle1 - diffHandle2)/diffHandle2 < _percentageMargin)
                            {
                                for (int x = -11; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _minShift * (decimal) _formationsLenght.Average())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishInverseHeadAndShoulders()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 3].Close < points[i - 4].Close && points[i - 3].Close < points[i - 2].Close && points[i - 1].Close > points[i - 3].Close && points[i - 5].Close > points[i - 3].Close && points[i].Close > points[i - 2].Close && points[i - 6].Close > points[i - 4].Close)
                    {
                        if (points[i - 4].Close > points[i - 5].Close && points[i - 2].Close > points[i - 1].Close)
                        {
                            var neck = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                            var diff1 = Math.Abs((points[i - 2].Close - points[i - 4].Close) / points[i - 4].Close);
                            var diff2 = Math.Abs((points[i - 5].Close - points[i - 1].Close) / points[i - 5].Close);
                            if (diff2 < _percentageMargin && diff1 < _percentageMargin && Math.Abs((points[i - 5].Close - points[i - 3].Close) / points[i - 3].Close) > _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }

                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishAscendingTriangle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (Math.Abs((points[i].Close - points[i - 2].Close) / points[i - 2].Close) < _percentageMargin && points[i + 2].Close > points[i + 1].Close)
                    {
                        if (points[i - 3].Close < points[i - 1].Close && points[i + 1].Close > points[i - 1].Close)
                        {
                            var diff1 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i].Close - points[i + 1].Close);
                            var diff4 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            if (diff2 > diff1 && diff4 > diff3)
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count > _formationsLenght.Min())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> ContinuationSymmetricTriangle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close > points[i - 2].Close && points[i].Close < points[i - 2].Close)
                    {
                        if (points[i - 5].Close < points[i - 3].Close && points[i - 1].Close > points[i - 3].Close && points[i].Close > points[i - 1].Close)
                        {
                            if (Math.Abs(points[i - 2].Close - points[i - 3].Close) < Math.Abs(points[i - 4].Close - points[i - 5].Close) && Math.Abs(points[i - 2].Close - points[i - 1].Close) < Math.Abs(points[i - 4].Close - points[i - 3].Close)) ;
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count > _formationsLenght.Min())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishDescendingTriangle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 3; i < points.Count - 3; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (Math.Abs((points[i].Close - points[i - 2].Close) / points[i - 2].Close) < _percentageMargin)
                    {
                        if (points[i - 3].Close > points[i - 1].Close && points[i + 1].Close < points[i - 1].Close)
                        {
                            var change1 = Math.Abs((points[i].Close - points[i + 1].Close) / points[i].Close);
                            var change2 = Math.Abs((points[i - 2].Close - points[i - 1].Close) / points[i - 2].Close);
                            var diff1 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            if (change2 > change1 && diff2 > diff1)
                            {
                                for (int x = -3; x < 4; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }
        private List<ZigZagObject> BullishFallingWedge()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 7; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close < points[i - 6].Close && points[i - 2].Close < points[i - 4].Close)
                    {
                        if (points[i - 3].Close < points[i - 5].Close && points[i - 1].Close < points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i - 6].Close - points[i - 7].Close);


                            if (diff2 < diff1 && diff3 > (decimal)_advance.Max() * diff2)
                            {
                                for (int x = -7; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count > _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishRisingWedge()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 2].Close > points[i - 4].Close && points[i].Close > points[i - 2].Close)
                    {
                        if (points[i - 5].Close < points[i - 3].Close && points[i - 1].Close > points[i - 3].Close)
                        {
                            if (Math.Abs(points[i - 3].Close - points[i - 5].Close) > Math.Abs(points[i - 4].Close - points[i - 2].Close))
                            {
                                var diff1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                                var diff2 = Math.Abs(points[i - 1].Close - points[i].Close);
                                var diff3 = Math.Abs(points[i - 4].Close - points[i - 5].Close);

                                if (diff2 < diff1 && diff3 > diff1)
                                {
                                    for (int x = -6; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].Close);
                                    }

                                    if (dateList.Count >= _formationsLenght.Max())
                                    {
                                        points[i].Signal = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }
        private List<ZigZagObject> BearishBearFlagsPennants()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 5].Close > points[i - 4].Close && (points[i - 5].Close > points[i - 6].Close && points[i - 4].Close < points[i - 6].Close))
                    {
                        if (points[i - 3].Close > points[i - 4].Close && points[i - 3].Close < points[i - 6].Close && points[i - 1].Close > points[i - 3].Close && points[i - 2].Close > points[i - 4].Close)
                        {
                            var diff1 = Math.Abs(points[i - 4].Close - points[i - 3].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var diff3 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            var diff4 = Math.Abs(points[i - 6].Close - points[i - 5].Close);
                            if (Math.Abs((points[i - 4].Close - points[i - 2].Close) / points[i - 4].Close) < _percentageMargin)
                            {
                                if (_minShift * diff1 <= diff3 && _minShift * diff1 < diff3)
                                {
                                    for (int x = -6; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].Close);
                                    }

                                    if (dateList.Count >= _formationsLenght.Max())
                                    {
                                        points[i].Signal = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishBullFlagsPennants()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close > points[i - 3].Close && (points[i - 4].Close > points[i - 5].Close && points[i - 3].Close > points[i - 5].Close))
                    {
                        if (points[i - 2].Close > points[i - 3].Close && points[i - 2].Close < points[i - 4].Close && points[i - 1].Close < points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff3 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            var diff4 = Math.Abs(points[i - 5].Close - points[i - 4].Close);
                            if (Math.Abs((points[i - 3].Close - points[i - 1].Close) / points[i - 3].Close) < _percentageMargin && Math.Abs((points[i - 3].Close - points[i - 1].Close) / points[i - 3].Close) < _percentageMargin)
                            {
                                if (_minShift * diff1 <= diff4 && _minShift * diff3 < diff4)
                                {
                                    for (int x = -6; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].Close);
                                    }

                                    if (dateList.Count >= _formationsLenght.Max())
                                    {
                                        points[i].Signal = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new List<ZigZagObject>();
        }

        private List<ZigZagObject> BullishAscendingPriceChannel()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 3].Close > points[i - 5].Close && points[i - 1].Close > points[i - 3].Close)
                    {
                        if (points[i - 4].Close < points[i - 2].Close && points[i - 2].Close < points[i].Close && points[i - 4].Close > points[i - 6].Close)
                        {
                            var diff1 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var change1 = Math.Abs((points[i - 2].Close - points[i - 4].Close) / points[i - 4].Close);
                            var change2 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);

                            if (Math.Abs((diff2 - diff1) / diff1) <= _percentageMargin && change1 <= _percentageMargin && change2 <= _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishDescendingPriceChannel()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i].Close < points[i - 1].Close && points[i].Close < points[i - 2].Close && points[i - 4].Close > points[i - 2].Close && points[i - 6].Close > points[i - 4].Close)
                    {
                        if (points[i - 1].Close < points[i - 3].Close && points[i - 5].Close > points[i - 3].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close < points[i - 1].Close)
                        {
                            var diff1 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff3 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            var diff4 = Math.Abs(points[i - 3].Close - points[i - 4].Close);

                            if (Math.Abs((diff1 - diff2) / diff1) <= _percentageMargin && Math.Abs((diff3 - diff4) / diff4) <= _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishRoundingBottomPattern()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 12; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i].Close > points[i - 1].Close && points[i - 2].Close < points[i].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close)
                    {
                        if (points[i - 4].Close > points[i - 6].Close && points[i - 12].Close > points[i - 8].Close && points[i - 8].Close < points[i - 4].Close)
                        {
                            var diff1 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var diff3 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var change1 = Math.Abs(points[i - 5].Close - points[i - 6].Close);
                            var change2 = Math.Abs(points[i - 9].Close - points[i - 10].Close);

                            if (diff1 >= _minShift * diff2 && diff3 >= _minShift * diff2 && Math.Abs((change1 - change2) / change2) <= (decimal)_advance.Average()) ;
                            {
                                for (int x = -12; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _minShift * _formationsLenght.Max() - 1)
                                {
                                    points[i].Signal = true;
                                }
                            }

                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishRoundingTopPattern()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 12; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i].Close < points[i - 1].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close)
                    {
                        if (points[i - 7].Close > points[i - 6].Close && points[i - 12].Close < points[i - 8].Close && points[i - 11].Close < points[i - 9].Close)
                        {
                            //
                            var diff1 = Math.Abs(points[i - 7].Close - points[i - 8].Close);
                            var diff2 = Math.Abs(points[i - 6].Close - points[i - 7].Close);

                            var change1 = Math.Abs(points[i - 5].Close - points[i - 9].Close);
                            var change2 = Math.Abs(points[i - 6].Close - points[i - 8].Close);

                            if (Math.Abs((diff1 - diff2) / diff1) <= (decimal)_advance.Average() && Math.Abs((change1 - change2) / change1) <= (decimal)_advance.Average())
                            {
                                for (int x = -12; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _minShift * _formationsLenght.Max() - 1)
                                {
                                    points[i].Signal = true;
                                }
                            }

                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> ContinuationDiamondFormation()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 10; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close > points[i - 6].Close && points[i - 6].Close > points[i - 8].Close && points[i - 4].Close > points[i - 2].Close && points[i - 2].Close > points[i - 1].Close)
                    {
                        if (points[i - 3].Close > points[i - 5].Close && points[i - 3].Close < points[i - 2].Close && points[i - 5].Close < points[i - 7].Close)
                        {
                            var diff1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff2 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            var diff3 = Math.Abs(points[i - 8].Close - points[i - 9].Close);

                            if (diff2 > diff1 && diff2 > diff3)
                            {
                                for (int x = -10; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Count())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }
        
        public List<string> GetFormationsAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod != null)
            {
                List<ZigZagObject> result = (List<ZigZagObject>)theMethod.Invoke(this, null);
                return result;
            }
            else
            {
                return _data;
            }
        }

        public int GetFormationsSignalsCount(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");
            return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }

        public List<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }

            return methods.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();
        }

        public int GetSignalsCount(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");
            return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }
    }
}
