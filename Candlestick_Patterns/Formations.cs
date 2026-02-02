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
        private readonly List<double> _graphSlope;
        private readonly decimal _channelTolerancePercentage;
        private List<ZigZagObject> _peaksFromZigZag;

        // ─── Precomputed slope boundaries (cached once, never reallocated) ───
        private double _slopeMin;
        private double _slopeMax;       // _graphSlope[3]
        private double _slope2;         // _graphSlope[2]
        private int _formationsLengthMax;
        // ─── Cached points list (reused across every formation call) ───
        private List<ZigZagObject> _cachedPoints;
        private bool _pointsDirty = true;
        private Dictionary<FormationNameEnum, Func<List<ZigZagObject>>> _dispatchTable;
        private List<string> _formationNames;

        public enum FormationNameEnum
        {
            None,
            BearishDoubleTops,
            BearishTripleTops,
            BullishDoubleBottoms,
            BullishTripleBottoms,
            BearishHeadAndShoulders,
            BullishCupAndHandle,
            BearishInverseCupAndHandle,
            BullishInverseHeadAndShoulders,
            BullishAscendingTriangle,
            ContinuationSymmetricTriangle,
            BearishDescendingTriangle,
            BullishFallingWedge, // ok
            BearishRisingWedge,
            BearishBearFlagsPennants,
            BullishBullFlagsPennants,
            BullishAscendingPriceChannel,
            BearishDescendingPriceChannel,
            BullishRoundingBottomPattern,
            BearishRoundingTopPattern,
            ContinuationDiamondFormation

        }
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

            CacheScalars();
            BuildDispatchTable();
        }

        public Formations(List<OhlcvObject> dataOhlcv, int zigZagParam) : this(dataOhlcv) // call full constructor first
        {
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
            _pointsDirty = true;
        }
        private void CacheScalars()
        {
            _slopeMin = _graphSlope.Min();
            _slopeMax = _graphSlope[3];          // 45
            _slope2 = _graphSlope[2];            // 30
            _formationsLengthMax = _formationsLenght.Max();
        }
        private List<ZigZagObject> GetPoints()
        {
            if (_pointsDirty || _cachedPoints == null)
            {
                _cachedPoints = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
                _pointsDirty = false;
            }
            return _cachedPoints;
        }

        [ThreadStatic]
        private static HashSet<decimal> _reusableDateSet;

        private static HashSet<decimal> RentDateSet()
        {
            _reusableDateSet ??= new HashSet<decimal>();
            _reusableDateSet.Clear();
            return _reusableDateSet;
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
        
        private List<ZigZagObject> BearishInverseCupAndHandle()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 16; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 14].IndexOHLCV))
                {
                    var highestPoint = new List<decimal>() { points[i - 11].Close, points[i - 10].Close, points[i - 9].Close };
                    var cupPoints = new List<decimal>() {points[i - 15].Close, points[i - 13].Close, points[i - 13].Close, points[i - 12].Close, points[i - 8].Close, points[i - 7].Close, points[i - 6].Close};
                    var handlePoints = new List<decimal>() {points[i - 2].Close, points[i - 3].Close, points[i - 4].Close};

                    if (highestPoint.Max() > cupPoints.Max() && highestPoint.Max() > handlePoints.Max() && handlePoints.Min() > cupPoints.Min() && points[i].Close < handlePoints.Min())
                    {
                        if (points[i - 10].Close > points[i - 12].Close && points[i - 8].Close > points[i - 6].Close && (handlePoints.Max() - handlePoints.Min()) / handlePoints.Min() < _percentageMargin)

                        {
                            for (int x = -16; x < 1; x++)
                            {
                                dateList.Add(points[i + x].IndexOHLCV);
                            }

                            points[i].Signal = true;
                            points[i - 16].Initiation = true;
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
            var points = GetPoints();
            var seen = RentDateSet();
            int count = points.Count;
            for (int i = 6; i < count; i++)
            {
                if (seen.Contains(points[i - 6].IndexOHLCV)) continue;
                decimal c0 = points[i - 6].Close;  // peak 0 (oldest)
                decimal c1 = points[i - 5].Close;  // valley 0
                decimal c2 = points[i - 4].Close;  // peak 1
                decimal c3 = points[i - 3].Close;  // valley 1
                decimal c4 = points[i - 2].Close;  // peak 2
                decimal c5 = points[i - 1].Close;  // valley 2

                if (c2 >= c0 || c4 >= c2) continue;
                if (c3 >= c1 || c5 >= c3) continue;
                var diff1 = Math.Abs(c2 - c1);
                var diff2 = Math.Abs(c3 - c4);
                var diff3 = Math.Abs(c0 - c1);
                var diff4 = Math.Abs(c2 - c3);
                var diff5 = Math.Abs(c5 - c4);
                var change1 = c1 - c3;
                var change2 = c2 - c4;
                var change3 = c3 - c5;
                if (diff2 >= diff1 || diff3 <= diff4 || diff4 <= diff5 || change2 <= change1 || change2 <= change3) continue;
                decimal slope53 = (c1 - c3) / 1 * 100;
                decimal slope31 = (c3 - c5) / 1 * 100;
                decimal slope42 = (c2 - c4) / 1 * 100;
                decimal slopeMinD = (decimal)_slopeMin;
                decimal slopeMaxD = (decimal)_slopeMax;
                decimal slope2D = (decimal)_slope2;

                if (slope53 < slopeMinD || slope53 >= slopeMaxD) continue;
                if (slope31 < slopeMinD || slope31 >= slopeMaxD) continue;
                if (slope42 <= slope2D) continue;
                // --- Formation confirmed — mark indices as seen ---
                for (int x = i - 6; x <= i; x++)
                    seen.Add(points[x].IndexOHLCV);

                // Use precomputed max instead of .Max() every iteration
                if (seen.Count > _formationsLengthMax)
                {
                    points[i].Signal = true;
                    points[i - 6].Initiation = true;
                }
            }

            return points;
        

            /*var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 4].Close < points[i - 6].Close && points[i - 2].Close < points[i - 4].Close)
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

            return points;*/
        }

        private List<ZigZagObject> BearishRisingWedge()
        {
            var dateList = new List<decimal>();
            var points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 6].IndexOHLCV))
                {
                    if (points[i - 4].Close > points[i - 6].Close && points[i - 2].Close > points[i - 4].Close)
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
                                    dateList.Add(points[i + x].IndexOHLCV);
                                }

                                if (dateList.Count >= _formationsLenght.Count())
                                {
                                    points[i].Signal = true;
                                    points[i - 10].Initiation = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        public Dictionary<FormationNameEnum, List<ZigZagObject>> GetAllFormations()
        {
            Dictionary<FormationNameEnum, List<ZigZagObject>> dict = new();

            dict.Add(FormationNameEnum.BearishDoubleTops, BearishDoubleTops());
            dict.Add(FormationNameEnum.BearishTripleTops, BearishTripleTops());
            dict.Add(FormationNameEnum.BullishDoubleBottoms, BullishDoubleBottoms());
            dict.Add(FormationNameEnum.BullishTripleBottoms, BullishTripleBottoms());
            dict.Add(FormationNameEnum.BearishHeadAndShoulders, BearishHeadAndShoulders());
            dict.Add(FormationNameEnum.BullishCupAndHandle, BullishCupAndHandle());
            dict.Add(FormationNameEnum.BearishInverseCupAndHandle, BearishInverseCupAndHandle());
            dict.Add(FormationNameEnum.BullishInverseHeadAndShoulders, BullishInverseHeadAndShoulders());
            dict.Add(FormationNameEnum.BullishAscendingTriangle, BullishAscendingTriangle());
            dict.Add(FormationNameEnum.ContinuationSymmetricTriangle, ContinuationSymmetricTriangle());
            dict.Add(FormationNameEnum.BearishDescendingTriangle, BearishDescendingTriangle());
            dict.Add(FormationNameEnum.BullishFallingWedge, BullishFallingWedge());
            dict.Add(FormationNameEnum.BearishRisingWedge, BearishRisingWedge());
            dict.Add(FormationNameEnum.BearishBearFlagsPennants, BearishBearFlagsPennants());
            dict.Add(FormationNameEnum.BullishBullFlagsPennants, BullishBullFlagsPennants());
            dict.Add(FormationNameEnum.BullishAscendingPriceChannel, BullishAscendingPriceChannel());
            dict.Add(FormationNameEnum.BearishDescendingPriceChannel, BearishDescendingPriceChannel());
            dict.Add(FormationNameEnum.BullishRoundingBottomPattern, BullishRoundingBottomPattern());
            dict.Add(FormationNameEnum.BearishRoundingTopPattern, BearishRoundingTopPattern());
            dict.Add(FormationNameEnum.ContinuationDiamondFormation, ContinuationDiamondFormation());

            return dict;
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
        public static int MinimumOhlcvCount(FormationNameEnum formationName) => formationName switch
        {
            FormationNameEnum.BearishDoubleTops => 4,
            FormationNameEnum.BearishTripleTops => 6,
            FormationNameEnum.BullishDoubleBottoms => 4,
            FormationNameEnum.BullishTripleBottoms => 6,
            FormationNameEnum.BearishHeadAndShoulders => 6,
            FormationNameEnum.BullishCupAndHandle => 13,
            FormationNameEnum.BearishInverseCupAndHandle => 16,
            FormationNameEnum.BullishInverseHeadAndShoulders => 6,
            FormationNameEnum.BullishAscendingTriangle => 5,
            FormationNameEnum.ContinuationSymmetricTriangle => 5,
            FormationNameEnum.BearishDescendingTriangle => 5,
            FormationNameEnum.BullishFallingWedge => 6,
            FormationNameEnum.BearishRisingWedge => 6,
            FormationNameEnum.BearishBearFlagsPennants => 5,
            FormationNameEnum.BullishBullFlagsPennants => 5,
            FormationNameEnum.BullishAscendingPriceChannel => 6,
            FormationNameEnum.BearishDescendingPriceChannel => 6,
            FormationNameEnum.BullishRoundingBottomPattern => 12,
            FormationNameEnum.BearishRoundingTopPattern => 10,
            FormationNameEnum.ContinuationDiamondFormation => 12
        };

        // replace switch GetFormationsSignalsList
        private void BuildDispatchTable()
        {
            _dispatchTable = new Dictionary<FormationNameEnum, Func<List<ZigZagObject>>>
            {
                { FormationNameEnum.BearishDoubleTops,              BearishDoubleTops },
                { FormationNameEnum.BearishTripleTops,              BearishTripleTops },
                { FormationNameEnum.BullishDoubleBottoms,           BullishDoubleBottoms },
                { FormationNameEnum.BullishTripleBottoms,           BullishTripleBottoms },
                { FormationNameEnum.BearishHeadAndShoulders,        BearishHeadAndShoulders },
                { FormationNameEnum.BullishCupAndHandle,            BullishCupAndHandle },
                { FormationNameEnum.BearishInverseCupAndHandle,     BearishInverseCupAndHandle },
                { FormationNameEnum.BullishInverseHeadAndShoulders, BullishInverseHeadAndShoulders },
                { FormationNameEnum.BullishAscendingTriangle,       BullishAscendingTriangle },
                { FormationNameEnum.ContinuationSymmetricTriangle,  ContinuationSymmetricTriangle },
                { FormationNameEnum.BearishDescendingTriangle,      BearishDescendingTriangle },
                { FormationNameEnum.BullishFallingWedge,            BullishFallingWedge },
                { FormationNameEnum.BearishRisingWedge,             BearishRisingWedge },
                { FormationNameEnum.BearishBearFlagsPennants,       BearishBearFlagsPennants },
                { FormationNameEnum.BullishBullFlagsPennants,       BullishBullFlagsPennants },
                { FormationNameEnum.BullishAscendingPriceChannel,   BullishAscendingPriceChannel },
                { FormationNameEnum.BearishDescendingPriceChannel,  BearishDescendingPriceChannel },
                { FormationNameEnum.BullishRoundingBottomPattern,   BullishRoundingBottomPattern },
                { FormationNameEnum.BearishRoundingTopPattern,      BearishRoundingTopPattern },
                { FormationNameEnum.ContinuationDiamondFormation,   ContinuationDiamondFormation },
            };

            // Pre-build the name list so GetAllFormationNames() never allocates again.
            _formationNames = Enum.GetValues(typeof(FormationNameEnum))
                .Cast<FormationNameEnum>()
                .Select(e => e.ToString())
                .Where(name => name.Contains("Bullish")
                            || name.Contains("Bearish")
                            || name.Contains("Continuation")).ToList();
        }

        /*public List<ZigZagObject> GetFormationsSignalsList(FormationNameEnum formation)
        {
            switch (formation)
            {
                case FormationNameEnum.BearishDoubleTops: return BearishDoubleTops();
                case FormationNameEnum.BearishTripleTops: return BearishTripleTops();
                case FormationNameEnum.BullishDoubleBottoms: return BullishDoubleBottoms();
                case FormationNameEnum.BullishTripleBottoms: return BullishTripleBottoms();
                case FormationNameEnum.BearishHeadAndShoulders: return BearishHeadAndShoulders();
                case FormationNameEnum.BullishCupAndHandle: return BullishCupAndHandle();
                case FormationNameEnum.BearishInverseCupAndHandle: return BearishInverseCupAndHandle();
                case FormationNameEnum.BullishInverseHeadAndShoulders: return BullishInverseHeadAndShoulders();
                case FormationNameEnum.BullishAscendingTriangle: return BullishAscendingTriangle();
                case FormationNameEnum.ContinuationSymmetricTriangle: return ContinuationSymmetricTriangle();
                case FormationNameEnum.BearishDescendingTriangle: return BearishDescendingTriangle();
                case FormationNameEnum.BullishFallingWedge: return BullishFallingWedge();
                case FormationNameEnum.BearishRisingWedge: return BearishRisingWedge();
                case FormationNameEnum.BearishBearFlagsPennants: return BearishBearFlagsPennants();
                case FormationNameEnum.BullishBullFlagsPennants: return BullishBullFlagsPennants();
                case FormationNameEnum.BullishAscendingPriceChannel: return BullishAscendingPriceChannel();
                case FormationNameEnum.BearishDescendingPriceChannel: return BearishDescendingPriceChannel();
                case FormationNameEnum.BullishRoundingBottomPattern: return BullishRoundingBottomPattern();
                case FormationNameEnum.BearishRoundingTopPattern: return BearishRoundingTopPattern();
                case FormationNameEnum.ContinuationDiamondFormation :return ContinuationDiamondFormation();
            }
            return null;
        }*/
        public List<ZigZagObject> GetFormationsSignalsList(FormationNameEnum formation)
        {
            return _dispatchTable.TryGetValue(formation, out var handler) ? handler() : null;
        }
        public List<string> GetAllFormationNames()
        {
            return _formationNames;
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");

            if (Enum.TryParse(methodName, ignoreCase: true, out FormationNameEnum formation))
            {
                return GetFormationsSignalsList(formation);
            }

            return _data;

            /*Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod != null)
            {
                List<ZigZagObject> result = (List<ZigZagObject>)theMethod.Invoke(this, null);
                return result;
            }
            else
            {
                return _data;
            }*/
        }

        public int GetFormationsSignalsCount(string formationName)
        {
            var signals = GetFormationsSignalsList(formationName);
            return signals?.Count(x => x.Signal) ?? 0;

            //var methodName = formationName.Trim().Replace(" ", "");
            //return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }

        public List<string> GetAllMethodNames()
        {
            return GetAllFormationNames();

            /*List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }

            return methods.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();*/
        }

        public int GetSignalsCount(string formationName)
        {
            return GetFormationsSignalsCount(formationName);

            //var methodName = formationName.Trim().Replace(" ", "");
            //return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }
    }
}
