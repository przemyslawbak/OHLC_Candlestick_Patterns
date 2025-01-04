using OHLC_Candlestick_Patterns;
using System.ComponentModel.Design;
using System.Reflection;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly List<double> _graphSlope;
        private readonly decimal _channelTolerancePercentage;
        private List<ZigZagObject> _peaksFromZigZag;

        public Formations(List<OhlcvObject> dataOhlcv)
        {
            _dataOhlcv = dataOhlcv;
            _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, 0.002M);
            _percentageMargin = 0.0025M; 
            _formationsLenght = new List<int>() { 4, 7 };
            _minShift = 2;
            _advance = new List<double>() { 0.10, 0.20 };
            _graphSlope = new List<double>() { 5, 20, 30, 45, 60};
            _channelTolerancePercentage = 0.3M;
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
                if (!dateList.Contains(points[i - 4].IndexOHLCV))
                {
                    if (points[i - 4].Close < points[i - 2].Close && points[i - 2].Close > points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 1].Close - points[i - 3].Close))) / points[i - 3].Close;
                        if (points[i - 3].Close > neck && points[i - 1].Close > neck && change < _percentageMargin && neck - points[i].Close > (decimal) _advance.Min() &&  neck - points[i - 4].Close > (decimal)_advance.Min())
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 4].Initiation = true;
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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 6].Close < points[i - 5].Close && points[i - 4].Close < points[i - 3].Close && points[i - 2].Close < points[i - 3].Close && points[i].Close < points[i - 1].Close)
                    {
                        var neck = new List<decimal>() { points[i - 2].Close, points[i - 4].Close };
                        var change = (Math.Abs((points[i - 1].Close - points[i - 2].Close)) / points[i - 2].Close);
                        var change1 = (Math.Abs((points[i - 3].Close - points[i - 4].Close)) /points[i - 4].Close);
                        var diff1 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);
                        var diff2 = Math.Abs((points[i - 5].Close - points[i - 3].Close) / points[i - 5].Close);
                        var diff3 = Math.Abs((points[i - 5].Close - points[i - 1].Close) / points[i - 5].Close);
                        if (Math.Abs((neck.Min() - neck.Max()) / neck.Min()) < _percentageMargin)
                        {
                            if (points[i - 5].Close > neck.Average() && points[i - 3].Close > neck.Average() && change < (decimal)_advance.Max())
                            {
                                if (points[i - 1].Close > neck.Average() && change1 < _percentageMargin && Math.Abs(diff1 - diff2) < _percentageMargin && Math.Abs(diff1 - diff3) < _percentageMargin)
                                {
                                    if (points[i - 6].Close < neck.Min() && points[i].Close < neck.Min() && neck.Min() - points[i].Close > (decimal)_advance.Min() && neck.Min() - points[i - 6].Close > (decimal)_advance.Min())
                                    {
                                        for (int x = -6; x < 1; x++)
                                        {
                                            dateList.Add(points[i + x].IndexOHLCV);
                                        }

                                        points[i].Signal = true;
                                        points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 4].IndexOHLCV))
                {
                    if (points[i - 4].Close > points[i - 2].Close && points[i - 2].Close < points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 3].Close - points[i - 1].Close)) / points[i - 3].Close);
                       
                        if (points[i - 1].Close < neck && points[i - 3].Close < neck && change < _percentageMargin &&  points[i].Close - neck > (decimal)_advance.Min() && points[i - 4].Close - neck > (decimal)_advance.Min())
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 4].Initiation = true;
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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 6].Close > points[i - 4].Close && points[i - 4].Close > points[i - 3].Close && points[i - 2].Close > points[i - 1].Close && points[i].Close > points[i - 2].Close)
                    {
                        if (points[i - 6].Close > points[i - 2].Close && points[i - 4].Close < points[i].Close)
                        {
                            var neck = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                            var change = (Math.Abs((points[i - 5].Close - points[i - 3].Close)) / points[i - 5].Close);
                            var change2 = (Math.Abs((points[i - 1].Close - points[i - 3].Close)) / points[i - 3].Close);
                            var change3 = (Math.Abs((points[i - 1].Close - points[i - 5].Close)) / points[i - 5].Close);
                            var diff1 = (neck.Max() - neck.Min()) / neck.Min();
                            if (diff1 < _percentageMargin && change < _percentageMargin && change2 < _percentageMargin && change3 < _percentageMargin && points[i].Close - neck.Min() > (decimal)_advance.Min() && points[i - 6].Close - neck.Min() > (decimal)_advance.Min())
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                    points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 3].Close > points[i - 5].Close && points[i - 3].Close > points[i - 1].Close && points[i - 4].Close < points[i - 5].Close && points[i - 6].Close < points[i - 5].Close && points[i - 3].Close > points[i - 2].Close && points[i - 2].Close < points[i - 1].Close && points[i - 1].Close > points[i].Close)
                    {
                        var neckline = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                        var changeNeckline = Math.Abs((neckline.Max() - neckline.Min()) / neckline.Min());
                        var shoulders = new List<decimal>() { points[i - 1].Close, points[i - 5].Close };
                        var changeShoulders = Math.Abs((shoulders.Max() - shoulders.Min()) / shoulders.Min());
                        var head = points[i - 3].Close;
                        if (changeNeckline < _percentageMargin && changeShoulders < _percentageMargin && neckline.Max() < shoulders.Max() && shoulders.Max() < head && (head - shoulders.Max()) > (decimal)_advance.Min() && points[i].Close < neckline.Min() && points[i - 6].Close < neckline.Min())
                        {
                            for (int x = -6; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 13].IndexOHLCV))
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
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 13].Initiation = true;
                            }
                        }
                    
                    }
                }
            }

            return points;
        }
        
        private List<ZigZagObject> BearishInverseCupAndHandle() //
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 11; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 11].IndexOHLCV))
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
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 11].Initiation = true;
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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 3].Close < points[i - 5].Close && points[i - 3].Close < points[i - 1].Close && points[i - 4].Close > points[i - 5].Close && points[i - 6].Close > points[i - 5].Close && points[i - 3].Close < points[i - 2].Close && points[i - 2].Close > points[i - 1].Close && points[i - 1].Close < points[i].Close)
                    {
                        var neckline = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                        var changeNeckline = Math.Abs((neckline.Max() - neckline.Min()) / neckline.Min());
                        var shoulders = new List<decimal>() { points[i - 1].Close, points[i - 5].Close };
                        var changeShoulders = Math.Abs((shoulders.Max() - shoulders.Min()) / shoulders.Min());
                        var head = points[i - 3].Close;
                        if (changeNeckline < _percentageMargin && changeShoulders < _percentageMargin && neckline.Max() > shoulders.Max() && shoulders.Max() > head && (shoulders.Min() - head) > (decimal)_advance.Min() && points[i].Close > neckline.Max() && points[i - 6].Close > neckline.Max())
                        {
                            for (int x = -6; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {
                    var top = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                    var changeTop = (top.Max() - top.Min()) / top.Min();
                    if (changeTop < _percentageMargin && points[i].Close - top.Max() > (decimal) _advance.Min() && points[i].Close > points[i - 1].Close)
                    {
                        if (points[i - 3].Close < points[i - 1].Close && points[i - 5].Close < points[i - 1].Close && points[i - 5].Close <  points[i - 3].Close && points[i - 3].Close - points[i - 5].Close > (decimal) _advance.Min())
                        {
                            var diff1 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff4 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            if (diff2 < diff1 && diff4 < diff3)
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 5].Initiation = true;
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
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {
                    if (points[i - 4].Close > points[i - 2].Close && points[i].Close > points[i - 2].Close)
                    {
                        if (points[i - 5].Close < points[i - 3].Close && points[i - 1].Close > points[i - 3].Close && points[i].Close > points[i - 1].Close)
                        {
                            if (Math.Abs(points[i - 2].Close - points[i - 3].Close) < Math.Abs(points[i - 4].Close - points[i - 5].Close) && Math.Abs(points[i - 2].Close - points[i - 1].Close) < Math.Abs(points[i - 4].Close - points[i - 3].Close)) 
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 5].Initiation = true;
                            }
                        }
                    }
                    else if (points[i - 4].Close < points[i - 2].Close && points[i].Close < points[i - 2].Close)
                    {
                        if (points[i - 5].Close > points[i - 3].Close && points[i - 1].Close < points[i - 3].Close && points[i].Close < points[i - 1].Close)
                        {
                            if (Math.Abs(points[i - 2].Close - points[i - 3].Close) < Math.Abs(points[i - 4].Close - points[i - 5].Close) && Math.Abs(points[i - 2].Close - points[i - 1].Close) < Math.Abs(points[i - 4].Close - points[i - 3].Close)) 
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 5].Initiation = true;
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
            for (int i = 5; i < points.Count - 3; i++)
            {
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {
                    var top = new List<decimal>() { points[i - 2].Close, points[i - 4].Close };
                    var changeTop = (top.Max() - top.Min()) / top.Min();
                    if (changeTop < _percentageMargin && top.Min() - points[i].Close > (decimal)_advance.Min() && points[i].Close < points[i - 1].Close)
                    {
                        if (points[i - 3].Close > points[i - 1].Close && points[i - 5].Close > points[i - 1].Close && points[i - 5].Close > points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 5].Close - points[i - 4].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff4 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            if (diff2 < diff1 && diff4 < diff3)
                            {
                                for (int x = -5; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 5].Initiation = true;
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
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 4].Close < points[i - 6].Close && points[i - 2].Close < points[i - 4].Close /*&& points[i].Close > points[i - 2].Close*/)
                    {
                        if (points[i - 3].Close < points[i - 5].Close && points[i - 1].Close < points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i - 6].Close - points[i - 5].Close);
                            var diff4 = Math.Abs(points[i - 4].Close - points[i - 3].Close);
                            var diff5 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            var change1 = points[i - 5].Close - points[i - 3].Close;
                            var change2 = points[i - 4].Close - points[i - 2].Close;
                            var change3 = points[i - 3].Close - points[i - 1].Close;

                            if (diff2 < diff1 && diff3 > diff4 && diff4 > diff5 && change2 > change1 && change2 > change3)
                            {
                                var slope53 =  (points[i - 5].Close - points[i - 3].Close) / 1 * 100;
                                var slope31 = (points[i - 3].Close - points[i - 1].Close) / 1 * 100;
                                var slope531 = (points[i - 5].Close - points[i - 1].Close) / 2 * 100;
                                var slope42 = (points[i - 4].Close - points[i - 2].Close) / 1 * 100;
                                if(slope53 >= (decimal)  _graphSlope.Min() && slope53 < (decimal)_graphSlope[3] && slope31 >= (decimal)_graphSlope.Min() && slope31 < (decimal)_graphSlope[3] && slope42 > (decimal) _graphSlope[2])
                                {
                                    for (int x = -6; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].IndexOHLCV);
                                    }

                                    if (dateList.Count > _formationsLenght.Max())
                                    {
                                        points[i].Signal = true;
                                        points[i - 6].Initiation = true;
                                    }

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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 4].Close > points[i - 6].Close && points[i - 2].Close > points[i - 4].Close /*&& points[i].Close < points[i - 2].Close*/)
                    {
                        if (points[i - 3].Close > points[i - 5].Close && points[i - 1].Close > points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            var diff2 = Math.Abs(points[i - 3].Close - points[i - 2].Close);
                            var diff3 = Math.Abs(points[i - 6].Close - points[i - 5].Close);
                            var diff4 = Math.Abs(points[i - 4].Close - points[i - 3].Close);
                            var diff5 = Math.Abs(points[i - 1].Close - points[i - 2].Close);
                            var change1 = Math.Abs(points[i - 5].Close - points[i - 3].Close);
                            var change2 = Math.Abs(points[i - 4].Close - points[i - 2].Close);
                            var change3 = Math.Abs(points[i - 3].Close - points[i - 1].Close);

                            if (diff2 < diff1 && diff3 > diff4 && diff4 > diff5 && change2 > change1 && change2 > change3)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 6].Initiation = true;

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
            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {
                    var smallerPointsList = new List<decimal>() { points[i - 4].Close, points[i - 3].Close,  points[i - 2].Close, points[i - 1].Close, points[i].Close};
                    var largerPointsList = new List<decimal>() { points[i - 5].Close, points[i - 4].Close, points[i - 3].Close,  points[i - 2].Close, points[i - 1].Close};
                    if (points[i - 5].Close  > smallerPointsList.Max() && points[i].Close < largerPointsList.Min() && points[i - 4].Close < points[i - 2].Close)
                    {
                        if (points[i - 3].Close > points[i - 4].Close && points[i - 1].Close > points[i - 3].Close)
                        {
                            var diff1 = Math.Abs(points[i - 4].Close - points[i - 3].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var diff3 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                            
                            if (Math.Abs((points[i - 4].Close - points[i - 2].Close) / points[i - 4].Close) < _percentageMargin)
                            {
                                if (_minShift * diff1 <= diff3 && _minShift * diff1 < diff3)
                                {
                                    for (int x = -5; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].IndexOHLCV);
                                    }

                                    points[i].Signal = true;
                                    points[i - 5].Initiation = true;
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
            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {

                    var largerPointsList = new List<decimal>() { points[i - 4].Close, points[i - 3].Close, points[i - 2].Close, points[i - 1].Close, points[i].Close };
                    var smallerPointsList = new List<decimal>() { points[i - 5].Close, points[i - 4].Close, points[i - 3].Close, points[i - 2].Close, points[i - 1].Close };
                    if (points[i - 4].Close > points[i - 3].Close && points[i - 5].Close < largerPointsList.Min() && points[i].Close > smallerPointsList.Max())
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
                                    for (int x = -5; x < 1; x++)
                                    {
                                        dateList.Add(points[i + x].IndexOHLCV);
                                    }

                                    points[i].Signal = true;
                                    points[i - 5].Initiation = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishAscendingPriceChannel()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 3].Close > points[i - 5].Close && points[i - 1].Close > points[i - 3].Close)
                    {
                        if (points[i - 4].Close < points[i - 2].Close && points[i - 2].Close < points[i].Close && points[i - 4].Close > points[i - 6].Close)
                        {
                            var diff1 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var diff3 = Math.Abs(points[i - 5].Close - points[i - 6].Close);
                            var diff4 = Math.Abs(points[i - 5].Close - points[i - 4].Close);
                            var diff5 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff6 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var change1 = Math.Abs((points[i - 2].Close - points[i - 4].Close) / points[i - 4].Close);
                            var change2 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);

                            if (Math.Abs(diff3 - diff1) / diff3 <= _channelTolerancePercentage && Math.Abs(diff2 - diff1) / diff1 <= _channelTolerancePercentage && Math.Abs(diff4 - diff5) / diff4 <= _channelTolerancePercentage && Math.Abs(diff3 - diff2) / diff3 <= _channelTolerancePercentage && Math.Abs(diff5 - diff6) / diff5 <= _channelTolerancePercentage && Math.Abs(change1 - change2)/change1 <= _channelTolerancePercentage)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i].Close < points[i - 1].Close && points[i].Close < points[i - 2].Close && points[i - 4].Close > points[i - 2].Close && points[i - 6].Close > points[i - 4].Close)
                    {
                        if (points[i - 1].Close < points[i - 3].Close && points[i - 5].Close > points[i - 3].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close < points[i - 1].Close)
                        {
                            var diff1 = Math.Abs(points[i - 3].Close - points[i - 4].Close);
                            var diff2 = Math.Abs(points[i - 2].Close - points[i - 1].Close);
                            var diff3 = Math.Abs(points[i - 5].Close - points[i - 6].Close);
                            var diff4 = Math.Abs(points[i - 5].Close - points[i - 4].Close);
                            var diff5 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff6 = Math.Abs(points[i].Close - points[i - 1].Close);
                            var change1 = Math.Abs((points[i - 2].Close - points[i - 4].Close) / points[i - 4].Close);
                            var change2 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);

                            if (Math.Abs(diff3 - diff1) / diff3 <= _channelTolerancePercentage && Math.Abs(diff2 - diff1) / diff1 <= _channelTolerancePercentage && Math.Abs(diff4 - diff5) / diff4 <= _channelTolerancePercentage && Math.Abs(diff3 - diff2) / diff3 <= _channelTolerancePercentage && Math.Abs(diff5 - diff6) / diff5 <= _channelTolerancePercentage && Math.Abs(change1 - change2) / change1 <= _channelTolerancePercentage)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                points[i].Signal = true;
                                points[i - 6].Initiation = true;
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
                if (!dateList.Contains(points[i - 12].IndexOHLCV))
                {
                    var lowesttPoint = points[i - 7].Close;
                    var resistanceLevel = new List<decimal>() { points[i - 8].Close, points[i - 6].Close };
                    var changeSupportLevel = Math.Abs((resistanceLevel.Max() - resistanceLevel.Min()) / resistanceLevel.Min());
                    var roundedPoints = new List<decimal>() { points[i - 11].Close, points[i - 10].Close, points[i - 9].Close, points[i - 5].Close, points[i - 4].Close, points[i - 3].Close, points[i - 2].Close};

                    if (lowesttPoint < roundedPoints.Min() && roundedPoints.Max() < resistanceLevel.Min() && changeSupportLevel < _percentageMargin)
                    {
                        // first and last higher than resistance level
                            
                        for (int x = -12; x < 1; x++)
                        {
                            dateList.Add(points[i + x].IndexOHLCV);
                        }

                        points[i].Signal = true;
                        points[i - 12].Initiation = true;
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishRoundingTopPattern()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 10; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 10].IndexOHLCV))
                {
                    var highestPoint = points[i - 5].Close;
                    var supportLevel = new List<decimal>() { points[i - 6].Close, points[i - 4].Close };
                    var changeSupportLevel = Math.Abs((supportLevel.Max() - supportLevel.Min()) / supportLevel.Min());
                    var roundedPoints = new List<decimal>() { points[i - 9].Close, points[i - 8].Close, points[i - 7].Close, points[i - 3].Close, points[i - 2].Close, points[i - 1].Close };

                    if (highestPoint > roundedPoints.Max() && roundedPoints.Min() > supportLevel.Min() && changeSupportLevel < _percentageMargin)
                    {
                        // first and last smallest than support level
                        {
                            for (int x = -10; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 10].Initiation = true;
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
            for (int i = 12; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 12].IndexOHLCV))
                {
                    var highest = points[i - 7].Close;
                    var lowest = new List<decimal>() { points[i - 8].Close, points[i - 6].Close };
                    var changeLowest = Math.Abs((lowest.Max() - lowest.Min()) / lowest.Min());
                    var diamondPoints = new List<decimal>() { points[i - 12].Close, points[i - 11].Close, points[i - 10].Close, points[i - 9].Close, points[i - 5].Close, points[i - 4].Close, points[i - 3].Close, points[i - 2].Close, points[i - 1].Close };
                    
                    if (highest > diamondPoints.Max() && lowest.Min() < diamondPoints.Min() &&  changeLowest < _percentageMargin)
                    {
                        // first and last smallest than support level

                        for (int x = -12; x < 1; x++)
                        {
                            dateList.Add(points[i + x].IndexOHLCV);
                        }

                        points[i].Signal = true;
                        points[i - 12].Initiation = true;
                    }
                    else
                    {
                        var newLowest = points[i - 7].Close;
                        var newHighest = new List<decimal>() { points[i - 8].Close, points[i - 6].Close };
                        diamondPoints = new List<decimal>() { points[i - 10].Close, points[i - 9].Close, points[i - 5].Close, points[i - 4].Close, points[i - 3].Close, points[i - 2].Close, points[i - 1].Close };
                        var changeNewHighest = Math.Abs((lowest.Max() - lowest.Min()) / lowest.Min());
                        
                        if (newHighest.Max() > diamondPoints.Max() && newLowest < diamondPoints.Min() && changeNewHighest < _percentageMargin)
                        {
                            //first and last larger than support level

                            for (int x = -12; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 12].Initiation = true;
                        }

                    }

                    //if (points[i - 4].Close > points[i - 6].Close && points[i - 6].Close > points[i - 8].Close && points[i - 4].Close > points[i - 2].Close && points[i - 2].Close > points[i - 1].Close)
                    //{
                    //    if (points[i - 3].Close > points[i - 5].Close && points[i - 3].Close < points[i - 2].Close && points[i - 5].Close < points[i - 7].Close)
                    //    {
                    //        var diff1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                    //        var diff2 = Math.Abs(points[i - 4].Close - points[i - 5].Close);
                    //        var diff3 = Math.Abs(points[i - 8].Close - points[i - 9].Close);

                    //        if (diff2 > diff1 && diff2 > diff3)
                    //        {
                    //            for (int x = -10; x < 1; x++)
                    //            {
                    //                dateList.Add(points[i + x].IndexOHLCV);
                    //            }

                    //            if (dateList.Count >= _formationsLenght.Count())
                    //            {
                    //                points[i].Signal = true;
                    //                points[i - 10].Initiation = true;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            return points;
        }
        
        public List<string> GetFormationsAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
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
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
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
