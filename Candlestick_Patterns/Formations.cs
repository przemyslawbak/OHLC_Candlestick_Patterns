using Enumerators;
using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    public class Formations : IFormations, ISignalEngine
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

        // Precomputed slope boundaries (cached once and never reallocated).
        private double _slopeMin;
        private double _slopeMax;
        private double _slope2;
        private int _formationsLengthMax;

        private Lazy<List<ZigZagObject>> _cachedPoints;
        private bool _pointsDirty = true;
        private Dictionary<FormationNameEnum, Func<List<ZigZagObject>>> _dispatchTable;
        private List<string> _formationNames;

        public Formations(List<OhlcvObject> dataOhlcv)
        {
            var deepCopy = dataOhlcv.Select(x => new OhlcvObject
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
                Signal = false
            }).ToList();

            _dataOhlcv = deepCopy;
            _data = SetPeaksVallyes.GetCloseAndSignalsData(deepCopy);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, 0.002M);
            _percentageMargin = 0.0025M;
            _formationsLenght = new List<int>() { 4, 7 };
            _minShift = 2;
            _advance = new List<double>() { 0.10, 0.20 };
            _graphSlope = new List<double>() { 5, 20, 30, 45, 60 };
            _channelTolerancePercentage = 0.3M;

            CacheScalars();
            InitializePointsCache();
            BuildDispatchTable();
        }

        // The legacy integer overload represents a percentage: 1 means 1%.
        public Formations(List<OhlcvObject> dataOhlcv, int zigZagParam)
            : this(dataOhlcv, zigZagParam / 100M)
        {
        }

        public Formations(List<OhlcvObject> dataOhlcv, decimal zigZagParam) : this(dataOhlcv)
        {
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
            _pointsDirty = true;
            InitializePointsCache();
        }

        private void InitializePointsCache()
        {
            _cachedPoints = new Lazy<List<ZigZagObject>>(() => SetPeaksVallyes.GetPoints(_peaksFromZigZag), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private void CacheScalars()
        {
            _slopeMin = _graphSlope.Min();
            _slopeMax = _graphSlope[3];
            _slope2 = _graphSlope[2];
            _formationsLengthMax = _formationsLenght.Max();
        }

        private List<ZigZagObject> GetPoints()
        {
            return SetPeaksVallyes.GetPoints(_cachedPoints.Value);
        }

        [ThreadStatic]
        private static HashSet<decimal> _reusableDateSet;
        [ThreadStatic]
        private static bool _dateSetInUse;

        private static HashSet<decimal> RentDateSet()
        {
            if (_dateSetInUse)
                throw new InvalidOperationException(
                    "RentDateSet detected nested call on same thread. " +
                    "Do not call multiple formation methods concurrently on the same thread. " +
                    "For parallel scanning, create separate Formations instances.");

            _dateSetInUse = true;
            _reusableDateSet ??= new HashSet<decimal>();
            _reusableDateSet.Clear();
            return _reusableDateSet;
        }

        private static void ReturnDateSet()
        {
            _dateSetInUse = false;
        }

        private (List<ZigZagObject> points, HashSet<decimal> seen, int count, int minNumber) SetupFormationScan(FormationNameEnum formation)
        {
            var points = GetPoints();
            var seen = RentDateSet();
            int count = points.Count;
            var minNumber = MinimumOhlcvCount(formation);
            return (points, seen, count, minNumber);
        }

        private void ConfirmFormation(List<ZigZagObject> points, HashSet<decimal> seen, int i, int minNumber, int? addRangeStart = null, int? addRangeEnd = null, int? initiationOffset = null, bool checkSeenCount = false)
        {
            int start = addRangeStart ?? (i - minNumber);
            int end = addRangeEnd ?? i;

            for (int x = start; x <= end; x++)
            {
                seen.Add(points[x].IndexOHLCV);
            }

            if (checkSeenCount && seen.Count < minNumber + 1)
                return;

            points[i].Signal = true;
            points[i - (initiationOffset ?? minNumber)].Initiation = true;
        }

        private List<ZigZagObject> BearishDoubleTops()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishDoubleTops);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    var c0 = points[i - 4].Close;
                    var c1 = points[i - 3].Close;
                    var neck = points[i - 2].Close;
                    var c3 = points[i - 1].Close;
                    var c4 = points[i].Close;
                    if (c0 >= neck || neck <= c4) continue;
                    var change = (Math.Abs((c3 - c1))) / c1;

                    if (c1 <= neck || c3 <= neck || change >= _percentageMargin || neck - c4 <= (decimal)_advance.Min() || neck - c0 <= (decimal)_advance.Min()) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishTripleTops()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishTripleTops);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;

                    if (c0 >= c1 || c2 >= c3 || c4 >= c3 || c6 >= c5) continue;
                    var neck = new List<decimal>() { c4, c2 };
                    var change = (Math.Abs((c5 - c4)) / c4);
                    var change1 = (Math.Abs((c3 - c2)) / c2);
                    var diff1 = Math.Abs((c5 - c3) / c3);
                    var diff2 = Math.Abs((c1 - c3) / c1);
                    var diff3 = Math.Abs((c1 - c5) / c1);

                    if (Math.Abs((neck.Min() - neck.Max()) / neck.Min()) >= _percentageMargin) continue;
                    if (c1 <= neck.Average() || c3 <= neck.Average() || change >= (decimal)_advance.Max()) continue;
                    if (c5 <= neck.Average() || change1 >= _percentageMargin || Math.Abs(diff1 - diff2) >= _percentageMargin || Math.Abs(diff1 - diff3) >= _percentageMargin) continue;
                    if (c0 >= neck.Min() || c6 >= neck.Min() || neck.Min() - c6 <= (decimal)_advance.Min() || neck.Min() - c0 <= (decimal)_advance.Min()) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishDoubleBottoms()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishDoubleBottoms);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 4].Close;
                    decimal c1 = points[i - 3].Close;
                    decimal c2 = points[i - 2].Close;
                    decimal c3 = points[i - 1].Close;
                    decimal c4 = points[i].Close;
                    if (c0 <= c2 || c2 >= c4) continue;
                    if (c1 >= c2 || c3 >= c2) continue;
                    decimal change = Math.Abs(c1 - c3) / c1;
                    if (change >= _percentageMargin) continue;
                    if (c4 - c2 <= (decimal)_advance.Min() || c0 - c2 <= (decimal)_advance.Min()) continue;
                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;

                }
                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishTripleBottoms()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishTripleBottoms);
            var _advanceMin = (decimal)_advance.Min();

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;
                    if (c0 <= c2 || c2 <= c3 || c4 <= c3 || c6 <= c4) continue;
                    if (c0 <= c4 || c2 >= c6) continue;

                    decimal neckMin = Math.Min(c2, c4);
                    decimal neckMax = Math.Max(c2, c4);
                    if ((neckMax - neckMin) / neckMin >= _percentageMargin) continue;
                    decimal change = Math.Abs(c1 - c3) / c1;
                    decimal change2 = Math.Abs(c5 - c3) / c3;
                    decimal change3 = Math.Abs(c5 - c1) / c1;
                    if (change >= _percentageMargin || change2 >= _percentageMargin || change3 >= _percentageMargin) continue;
                    if (c6 - neckMin <= _advanceMin || c0 - neckMin <= _advanceMin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    if (seen.Count >= _formationsLengthMax)
                    {
                        points[i].Signal = true;
                        points[i - minNumber].Initiation = true;
                    }
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishHeadAndShoulders()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishHeadAndShoulders);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;

                    if (c3 <= c1 || c3 <= c5) continue;
                    if (c2 >= c1 || c0 >= c1) continue;
                    if (c3 <= c4 || c4 >= c5) continue;
                    if (c5 <= c6) continue;
                    decimal neckMin = Math.Min(c2, c4);
                    decimal neckMax = Math.Max(c2, c4);
                    decimal changeNeckline = (neckMax - neckMin) / neckMin;
                    if (changeNeckline >= _percentageMargin) continue;
                    decimal shoulderMin = Math.Min(c1, c5);
                    decimal shoulderMax = Math.Max(c1, c5);
                    decimal changeShoulders = (shoulderMax - shoulderMin) / shoulderMin;
                    if (changeShoulders >= _percentageMargin) continue;
                    if (neckMax >= shoulderMax) continue;
                    if (shoulderMax >= c3) continue;
                    if (c3 - shoulderMax <= (decimal)_advance.Min()) continue;
                    if (c6 >= neckMin || c0 >= neckMin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishCupAndHandle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishCupAndHandle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 13].Close;
                    decimal c1 = points[i - 12].Close;
                    decimal c2 = points[i - 11].Close;
                    decimal c3 = points[i - 10].Close;
                    decimal c4 = points[i - 9].Close;
                    decimal c5 = points[i - 8].Close;
                    decimal c6 = points[i - 7].Close;
                    decimal c7 = points[i - 6].Close;
                    decimal c8 = points[i - 5].Close;
                    decimal c9 = points[i - 4].Close;
                    decimal c10 = points[i - 3].Close;
                    decimal c11 = points[i - 2].Close;
                    decimal c12 = points[i - 1].Close;
                    decimal c13 = points[i].Close;

                    if (c13 <= c12 || c12 >= c10 || c8 >= c10 || c9 <= c11 || c9 >= c13) continue;
                    if (c6 >= c8 || c6 <= c4 || c7 <= c5 || c2 <= c4 || c0 <= c2) continue;

                    decimal diff1 = Math.Abs(c11 - c10);
                    decimal diff2 = Math.Abs(c8 - c9);
                    decimal diff3 = Math.Abs(c8 - c7);
                    decimal diff4 = Math.Abs(c6 - c7);
                    decimal change1 = Math.Abs((c5 - c3) / c3);
                    decimal change2 = Math.Abs((c6 - c2) / c2);
                    if (diff2 < (decimal)_minShift * diff1) continue;
                    if (diff2 <= diff3 || diff3 >= diff4) continue;
                    if (change1 >= _percentageMargin || change2 >= _percentageMargin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishInverseCupAndHandle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishInverseCupAndHandle);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal c0 = points[i - 16].Close;
                    decimal c1 = points[i - 15].Close;
                    decimal c2 = points[i - 14].Close;
                    decimal c3 = points[i - 13].Close;
                    decimal c4 = points[i - 12].Close;
                    decimal c5 = points[i - 11].Close;
                    decimal c6 = points[i - 10].Close;
                    decimal c7 = points[i - 9].Close;
                    decimal c8 = points[i - 8].Close;
                    decimal c9 = points[i - 7].Close;
                    decimal c10 = points[i - 6].Close;
                    decimal c11 = points[i - 5].Close;
                    decimal c12 = points[i - 4].Close;
                    decimal c13 = points[i - 3].Close;
                    decimal c14 = points[i - 2].Close;
                    decimal c15 = points[i - 1].Close;
                    decimal c16 = points[i].Close;

                    decimal highMax = Math.Max(c5, Math.Max(c6, c7));
                    decimal cupMin = Math.Min(Math.Min(c1, Math.Min(c3, c4)), Math.Min(c8, Math.Min(c9, c10)));
                    decimal cupMax = Math.Max(Math.Max(c1, Math.Max(c3, c4)), Math.Max(c8, Math.Max(c9, c10)));
                    decimal handleMin = Math.Min(c12, Math.Min(c13, c14));
                    decimal handleMax = Math.Max(c12, Math.Max(c13, c14));

                    if (highMax <= cupMax) continue;
                    if (highMax <= handleMax) continue;
                    if (handleMin <= cupMin) continue;
                    if (c16 >= handleMin) continue;
                    if (c6 <= c4 || c8 <= c10) continue;
                    if ((handleMax - handleMin) / handleMin >= _percentageMargin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishInverseHeadAndShoulders()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishInverseHeadAndShoulders);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;
                    if (c3 >= c1 || c3 >= c5) continue;
                    if (c2 <= c1 || c0 <= c1) continue;
                    if (c3 >= c4 || c4 <= c5) continue;
                    if (c5 >= c6) continue;
                    decimal neckMin = Math.Min(c2, c4);
                    decimal neckMax = Math.Max(c2, c4);
                    decimal changeNeckline = (neckMax - neckMin) / neckMin;
                    if (changeNeckline >= _percentageMargin) continue;
                    decimal shoulderMin = Math.Min(c1, c5);
                    decimal shoulderMax = Math.Max(c1, c5);
                    decimal changeShoulders = (shoulderMax - shoulderMin) / shoulderMin;
                    if (changeShoulders >= _percentageMargin) continue;
                    if (neckMax <= shoulderMax) continue;
                    if (shoulderMax <= c3) continue;
                    if (shoulderMin - c3 <= (decimal)_advance.Min()) continue;

                    if (c6 <= neckMax || c0 <= neckMax) continue;
                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishAscendingTriangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishAscendingTriangle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 5].Close;
                    decimal c1 = points[i - 4].Close;
                    decimal c2 = points[i - 3].Close;
                    decimal c3 = points[i - 2].Close;
                    decimal c4 = points[i - 1].Close;
                    decimal c5 = points[i].Close;
                    decimal topMin = Math.Min(c1, c3);
                    decimal topMax = Math.Max(c1, c3);
                    decimal changeTop = (topMax - topMin) / topMin;
                    if (changeTop >= _percentageMargin) continue;
                    if (c5 - topMax <= (decimal)_advance.Min()) continue;
                    if (c5 <= c4) continue;
                    if (c2 >= c4 || c0 >= c2 || c0 >= c4) continue;
                    if (c2 - c0 <= (decimal)_advance.Min()) continue;
                    decimal diff1 = Math.Abs(c5 - c4);
                    decimal diff2 = Math.Abs(c2 - c3);
                    decimal diff3 = Math.Abs(c2 - c1);
                    decimal diff4 = Math.Abs(c4 - c3);
                    if (diff2 >= diff1 || diff4 >= diff3) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> ContinuationSymmetricTriangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.ContinuationSymmetricTriangle);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 5].Close;
                    decimal c1 = points[i - 4].Close;
                    decimal c2 = points[i - 3].Close;
                    decimal c3 = points[i - 2].Close;
                    decimal c4 = points[i - 1].Close;
                    decimal c5 = points[i].Close;

                    bool bullishLeg = false;
                    bool bearishLeg = false;

                    if (c1 > c3 && c5 > c3)
                    {
                        if (c0 < c2 && c4 > c2 && c5 > c4)
                            bullishLeg = true;
                    }
                    else if (c1 < c3 && c5 < c3)
                    {
                        if (c0 > c2 && c4 < c2 && c5 < c4)
                            bearishLeg = true;
                    }

                    if (!bullishLeg && !bearishLeg) continue;

                    decimal innerRange = Math.Abs(c3 - c2);
                    decimal outerRange = Math.Abs(c1 - c0);
                    decimal innerRange2 = Math.Abs(c3 - c4);
                    decimal outerRange2 = Math.Abs(c1 - c2);
                    if (innerRange >= outerRange || innerRange2 >= outerRange2) continue;

                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishDescendingTriangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishDescendingTriangle);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 5].Close;
                    decimal c1 = points[i - 4].Close;
                    decimal c2 = points[i - 3].Close;
                    decimal c3 = points[i - 2].Close;
                    decimal c4 = points[i - 1].Close;
                    decimal c5 = points[i].Close;
                    decimal topMin = Math.Min(c1, c3);
                    decimal topMax = Math.Max(c1, c3);
                    decimal changeTop = (topMax - topMin) / topMin;
                    if (changeTop >= _percentageMargin) continue;
                    if (topMin - c5 <= (decimal)_advance.Min()) continue;
                    if (c5 >= c4) continue;
                    if (c2 <= c4 || c0 <= c2 || c0 <= c4) continue;
                    decimal diff1 = Math.Abs(c0 - c1);
                    decimal diff2 = Math.Abs(c2 - c3);
                    decimal diff3 = Math.Abs(c2 - c1);
                    decimal diff4 = Math.Abs(c4 - c3);
                    if (diff2 >= diff1 || diff4 >= diff3) continue;
                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishFallingWedge()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishFallingWedge);
            try
            {

                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - 6].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;

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

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }

            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishRisingWedge()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishRisingWedge);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - 6].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;

                    if (c2 <= c0 || c4 <= c2) continue;
                    if (c3 <= c1 || c5 <= c3) continue;
                    decimal diff1 = Math.Abs(c2 - c1);
                    decimal diff2 = Math.Abs(c3 - c4);
                    decimal diff3 = Math.Abs(c0 - c1);
                    decimal diff4 = Math.Abs(c2 - c3);
                    decimal diff5 = Math.Abs(c5 - c4);
                    decimal change1 = Math.Abs(c1 - c3);
                    decimal change2 = Math.Abs(c2 - c4);
                    decimal change3 = Math.Abs(c3 - c5);

                    if (diff2 >= diff1 || diff3 <= diff4 || diff4 <= diff5 || change2 <= change1 || change2 <= change3) continue;
                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishBearFlagsPennants()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishBearFlagsPennants);
            try
            {

                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 5].Close;
                    decimal c1 = points[i - 4].Close;
                    decimal c2 = points[i - 3].Close;
                    decimal c3 = points[i - 2].Close;
                    decimal c4 = points[i - 1].Close;
                    decimal c5 = points[i].Close;

                    decimal smallerMax = Math.Max(c1, Math.Max(c2, Math.Max(c3, Math.Max(c4, c5))));
                    decimal largerMin = Math.Min(c0, Math.Min(c1, Math.Min(c2, Math.Min(c3, c4))));
                    if (c0 <= smallerMax) continue;
                    if (c5 >= largerMin) continue;
                    if (c1 >= c3) continue;
                    if (c2 <= c1 || c4 <= c2) continue;
                    decimal diff1 = Math.Abs(c1 - c2);
                    decimal diff2 = Math.Abs(c3 - c4);
                    decimal diff3 = Math.Abs(c1 - c0);
                    if (Math.Abs((c1 - c3) / c1) >= _percentageMargin) continue;
                    if ((decimal)_minShift * diff1 >= diff3 || (decimal)_minShift * diff2 >= diff3) continue;
                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishBullFlagsPennants()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishBullFlagsPennants);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 5].Close;
                    decimal c1 = points[i - 4].Close;
                    decimal c2 = points[i - 3].Close;
                    decimal c3 = points[i - 2].Close;
                    decimal c4 = points[i - 1].Close;
                    decimal c5 = points[i].Close;

                    decimal largerMin = Math.Min(c1, Math.Min(c2, Math.Min(c3, Math.Min(c4, c5))));
                    decimal smallerMax = Math.Max(c0, Math.Max(c1, Math.Max(c2, Math.Max(c3, c4))));

                    if (c1 <= c2) continue;
                    if (c0 >= largerMin) continue;
                    if (c5 <= smallerMax) continue;
                    if (c3 <= c2 || c3 >= c1 || c4 >= c2) continue;
                    decimal diff1 = Math.Abs(c2 - c3);
                    decimal diff2 = Math.Abs(c2 - c1);
                    decimal diff3 = Math.Abs(c4 - c3);
                    decimal diff4 = Math.Abs(c0 - c1);
                    decimal changePennant = Math.Abs((c2 - c4) / c2);
                    if (changePennant >= _percentageMargin) continue;
                    if ((decimal)_minShift * diff1 > diff4 || (decimal)_minShift * diff3 >= diff4) continue;

                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishAscendingPriceChannel()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishAscendingPriceChannel);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;

                    if (c3 <= c1 || c5 <= c3) continue;
                    if (c2 >= c4 || c4 >= c6 || c2 <= c0) continue;
                    decimal diff1 = Math.Abs(c3 - c2);
                    decimal diff2 = Math.Abs(c4 - c5);
                    decimal diff3 = Math.Abs(c1 - c0);
                    decimal diff4 = Math.Abs(c1 - c2);
                    decimal diff5 = Math.Abs(c4 - c3);
                    decimal diff6 = Math.Abs(c6 - c5);
                    decimal change1 = Math.Abs((c4 - c2) / c2);
                    decimal change2 = Math.Abs((c5 - c3) / c3);

                    if (Math.Abs(diff3 - diff1) / diff3 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff2 - diff1) / diff1 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff4 - diff5) / diff4 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff3 - diff2) / diff3 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff5 - diff6) / diff5 > _channelTolerancePercentage) continue;
                    if (Math.Abs(change1 - change2) / change1 > _channelTolerancePercentage) continue;
                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishDescendingPriceChannel()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishDescendingPriceChannel);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 6].Close;
                    decimal c1 = points[i - 5].Close;
                    decimal c2 = points[i - 4].Close;
                    decimal c3 = points[i - 3].Close;
                    decimal c4 = points[i - 2].Close;
                    decimal c5 = points[i - 1].Close;
                    decimal c6 = points[i].Close;
                    if (c6 >= c5 || c6 >= c4) continue;
                    if (c2 <= c4 || c0 <= c2) continue;
                    if (c5 >= c3 || c1 <= c3) continue;
                    if (c4 <= c6 || c4 >= c5) continue;

                    decimal diff1 = Math.Abs(c3 - c2);
                    decimal diff2 = Math.Abs(c4 - c5);
                    decimal diff3 = Math.Abs(c1 - c0);
                    decimal diff4 = Math.Abs(c1 - c2);
                    decimal diff5 = Math.Abs(c4 - c3);
                    decimal diff6 = Math.Abs(c6 - c5);
                    decimal change1 = Math.Abs((c4 - c2) / c2);
                    decimal change2 = Math.Abs((c5 - c3) / c3);

                    if (Math.Abs(diff3 - diff1) / diff3 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff2 - diff1) / diff1 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff4 - diff5) / diff4 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff3 - diff2) / diff3 > _channelTolerancePercentage) continue;
                    if (Math.Abs(diff5 - diff6) / diff5 > _channelTolerancePercentage) continue;
                    if (Math.Abs(change1 - change2) / change1 > _channelTolerancePercentage) continue;
                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishRoundingBottomPattern()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishRoundingBottomPattern);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;
                    decimal c0 = points[i - 12].Close;
                    decimal c1 = points[i - 11].Close;
                    decimal c2 = points[i - 10].Close;
                    decimal c3 = points[i - 9].Close;
                    decimal c4 = points[i - 8].Close;
                    decimal c5 = points[i - 7].Close;
                    decimal c6 = points[i - 6].Close;
                    decimal c7 = points[i - 5].Close;
                    decimal c8 = points[i - 4].Close;
                    decimal c9 = points[i - 3].Close;
                    decimal c10 = points[i - 2].Close;
                    decimal c11 = points[i - 1].Close;
                    decimal c12 = points[i].Close;

                    decimal resistanceMin = Math.Min(c4, c6);
                    decimal resistanceMax = Math.Max(c4, c6);
                    decimal roundedMin = Math.Min(c1, Math.Min(c2, Math.Min(c3, Math.Min(c7, Math.Min(c8, Math.Min(c9, c10))))));
                    decimal roundedMax = Math.Max(c1, Math.Max(c2, Math.Max(c3, Math.Max(c7, Math.Max(c8, Math.Max(c9, c10))))));

                    if (c5 >= roundedMin) continue;
                    if (roundedMax >= resistanceMin) continue;
                    decimal changeSupportLevel = (resistanceMax - resistanceMin) / resistanceMin;
                    if (changeSupportLevel >= _percentageMargin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                    {
                        seen.Add(points[x].IndexOHLCV);
                    }

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishRoundingTopPattern()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishRoundingTopPattern);

            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal c0 = points[i - 10].Close;
                    decimal c1 = points[i - 9].Close;
                    decimal c2 = points[i - 8].Close;
                    decimal c3 = points[i - 7].Close;
                    decimal c4 = points[i - 6].Close;
                    decimal c5 = points[i - 5].Close;
                    decimal c6 = points[i - 4].Close;
                    decimal c7 = points[i - 3].Close;
                    decimal c8 = points[i - 2].Close;
                    decimal c9 = points[i - 1].Close;
                    decimal c10 = points[i].Close;

                    decimal supportMin = Math.Min(c4, c6);
                    decimal supportMax = Math.Max(c4, c6);
                    decimal roundedMin = Math.Min(c1, Math.Min(c2, Math.Min(c3, Math.Min(c7, Math.Min(c8, c9)))));
                    decimal roundedMax = Math.Max(c1, Math.Max(c2, Math.Max(c3, Math.Max(c7, Math.Max(c8, c9)))));

                    if (c5 <= roundedMax) continue;
                    if (roundedMin <= supportMin) continue;
                    decimal changeSupportLevel = (supportMax - supportMin) / supportMin;
                    if (changeSupportLevel >= _percentageMargin) continue;

                    for (int x = i - minNumber; x <= i; x++)
                        seen.Add(points[x].IndexOHLCV);

                    points[i].Signal = true;
                    points[i - minNumber].Initiation = true;
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private bool ApproximatelyEqual(decimal left, decimal right, decimal toleranceMultiplier = 1M)
        {
            if (left == 0M || right == 0M)
                return false;

            decimal baseValue = Math.Min(Math.Abs(left), Math.Abs(right));
            if (baseValue == 0M)
                return false;

            return Math.Abs(left - right) / baseValue <= _percentageMargin * toleranceMultiplier;
        }

        private static decimal RelativeDifference(decimal left, decimal right)
        {
            decimal avg = (Math.Abs(left) + Math.Abs(right)) / 2M;
            return avg == 0M ? 0M : Math.Abs(left - right) / avg;
        }

        private List<ZigZagObject> BullishRectangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishRectangle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal r1 = points[i - 5].Close;
                    decimal s1 = points[i - 4].Close;
                    decimal r2 = points[i - 3].Close;
                    decimal s2 = points[i - 2].Close;
                    decimal r3 = points[i - 1].Close;
                    decimal breakout = points[i].Close;

                    if (!ApproximatelyEqual(r1, r2, 2M) || !ApproximatelyEqual(r2, r3, 2M)) continue;
                    if (!ApproximatelyEqual(s1, s2, 2M)) continue;
                    if (!(r1 > s1 && r2 > s2 && r3 > s2)) continue;
                    if (breakout <= Math.Max(r1, Math.Max(r2, r3))) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishRectangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishRectangle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal s1 = points[i - 5].Close;
                    decimal r1 = points[i - 4].Close;
                    decimal s2 = points[i - 3].Close;
                    decimal r2 = points[i - 2].Close;
                    decimal s3 = points[i - 1].Close;
                    decimal breakdown = points[i].Close;

                    if (!ApproximatelyEqual(s1, s2, 2M) || !ApproximatelyEqual(s2, s3, 2M)) continue;
                    if (!ApproximatelyEqual(r1, r2, 2M)) continue;
                    if (!(r1 > s1 && r2 > s2 && r2 > s3)) continue;
                    if (breakdown >= Math.Min(s1, Math.Min(s2, s3))) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishBroadeningTop()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishBroadeningTop);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal l1 = points[i - 4].Close;
                    decimal h1 = points[i - 3].Close;
                    decimal l2 = points[i - 2].Close;
                    decimal h2 = points[i - 1].Close;
                    decimal l3 = points[i].Close;

                    if (!(h2 > h1 && l2 < l1 && l3 < l2)) continue;
                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishBroadeningBottom()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishBroadeningBottom);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal h1 = points[i - 4].Close;
                    decimal l1 = points[i - 3].Close;
                    decimal h2 = points[i - 2].Close;
                    decimal l2 = points[i - 1].Close;
                    decimal h3 = points[i].Close;

                    if (!(h2 > h1 && l2 < l1 && h3 > h2)) continue;
                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishDescendingTriangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishDescendingTriangle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal h1 = points[i - 5].Close;
                    decimal l1 = points[i - 4].Close;
                    decimal h2 = points[i - 3].Close;
                    decimal l2 = points[i - 2].Close;
                    decimal h3 = points[i - 1].Close;
                    decimal breakout = points[i].Close;

                    if (!(h1 > h2 && h2 > h3)) continue;
                    if (!ApproximatelyEqual(l1, l2, 2M)) continue;
                    if (breakout <= h3) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishAscendingTriangle()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishAscendingTriangle);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal l1 = points[i - 5].Close;
                    decimal h1 = points[i - 4].Close;
                    decimal l2 = points[i - 3].Close;
                    decimal h2 = points[i - 2].Close;
                    decimal l3 = points[i - 1].Close;
                    decimal breakdown = points[i].Close;

                    if (!(l1 < l2 && l2 < l3)) continue;
                    if (!ApproximatelyEqual(h1, h2, 2M)) continue;
                    if (breakdown >= l3) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BullishMeasuredMove()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BullishMeasuredMove);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal start = points[i - 3].Close;
                    decimal firstLegEnd = points[i - 2].Close;
                    decimal correction = points[i - 1].Close;
                    decimal completion = points[i].Close;

                    decimal firstLeg = firstLegEnd - start;
                    decimal secondLeg = completion - correction;

                    if (!(firstLeg > 0M && secondLeg > 0M && correction > start && correction < firstLegEnd)) continue;
                    if (RelativeDifference(firstLeg, secondLeg) > 0.20M) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> BearishMeasuredMove()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.BearishMeasuredMove);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal start = points[i - 3].Close;
                    decimal firstLegEnd = points[i - 2].Close;
                    decimal correction = points[i - 1].Close;
                    decimal completion = points[i].Close;

                    decimal firstLeg = start - firstLegEnd;
                    decimal secondLeg = correction - completion;

                    if (!(firstLeg > 0M && secondLeg > 0M && correction < start && correction > firstLegEnd)) continue;
                    if (RelativeDifference(firstLeg, secondLeg) > 0.20M) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
        }

        private List<ZigZagObject> ContinuationDiamondFormation()
        {
            var (points, seen, count, minNumber) = SetupFormationScan(FormationNameEnum.ContinuationDiamondFormation);
            try
            {
                for (int i = minNumber; i < count; i++)
                {
                    if (seen.Contains(points[i - minNumber].IndexOHLCV)) continue;

                    decimal c0 = points[i - 12].Close;
                    decimal c1 = points[i - 11].Close;
                    decimal c2 = points[i - 10].Close;
                    decimal c3 = points[i - 9].Close;
                    decimal c4 = points[i - 8].Close;
                    decimal c5 = points[i - 7].Close;
                    decimal c6 = points[i - 6].Close;
                    decimal c7 = points[i - 5].Close;
                    decimal c8 = points[i - 4].Close;
                    decimal c9 = points[i - 3].Close;
                    decimal c10 = points[i - 2].Close;
                    decimal c11 = points[i - 1].Close;
                    decimal c12 = points[i].Close;

                    if (c8 <= c6 || c6 <= c4 || c8 <= c10 || c10 <= c11) continue;
                    if (c9 <= c7 || c9 >= c10 || c7 >= c5) continue;
                    decimal diff1 = Math.Abs(c10 - c9);
                    decimal diff2 = Math.Abs(c8 - c7);
                    decimal diff3 = Math.Abs(c4 - c3);
                    if (diff2 <= diff1 || diff2 <= diff3) continue;

                    ConfirmFormation(points, seen, i, minNumber);
                }

                return points;
            }
            finally
            {
                ReturnDateSet();
            }
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
            dict.Add(FormationNameEnum.BullishRectangle, BullishRectangle());
            dict.Add(FormationNameEnum.BearishRectangle, BearishRectangle());
            dict.Add(FormationNameEnum.BearishBroadeningTop, BearishBroadeningTop());
            dict.Add(FormationNameEnum.BullishBroadeningBottom, BullishBroadeningBottom());
            dict.Add(FormationNameEnum.BullishDescendingTriangle, BullishDescendingTriangle());
            dict.Add(FormationNameEnum.BearishAscendingTriangle, BearishAscendingTriangle());
            dict.Add(FormationNameEnum.BullishMeasuredMove, BullishMeasuredMove());
            dict.Add(FormationNameEnum.BearishMeasuredMove, BearishMeasuredMove());

            return dict;
        }

        public List<string> GetFormationsAllMethodNames()
        {
            return GetAllFormationNames();
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
            FormationNameEnum.ContinuationDiamondFormation => 12,
            FormationNameEnum.BullishRectangle => 5,
            FormationNameEnum.BearishRectangle => 5,
            FormationNameEnum.BearishBroadeningTop => 4,
            FormationNameEnum.BullishBroadeningBottom => 4,
            FormationNameEnum.BullishDescendingTriangle => 5,
            FormationNameEnum.BearishAscendingTriangle => 5,
            FormationNameEnum.BullishMeasuredMove => 3,
            FormationNameEnum.BearishMeasuredMove => 3
        };

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
                { FormationNameEnum.BullishRectangle,               BullishRectangle },
                { FormationNameEnum.BearishRectangle,               BearishRectangle },
                { FormationNameEnum.BearishBroadeningTop,           BearishBroadeningTop },
                { FormationNameEnum.BullishBroadeningBottom,        BullishBroadeningBottom },
                { FormationNameEnum.BullishDescendingTriangle,      BullishDescendingTriangle },
                { FormationNameEnum.BearishAscendingTriangle,       BearishAscendingTriangle },
                { FormationNameEnum.BullishMeasuredMove,            BullishMeasuredMove },
                { FormationNameEnum.BearishMeasuredMove,            BearishMeasuredMove },
            };

            _formationNames = Enum.GetValues(typeof(FormationNameEnum))
                .Cast<FormationNameEnum>()
                .Select(e => e.ToString())
                .Where(name => name.Contains("Bullish") || name.Contains("Bearish") || name.Contains("Continuation")).ToList();
        }

        public List<ZigZagObject> GetFormationsSignalsList(FormationNameEnum formation)
        {
            return _dispatchTable.TryGetValue(formation, out var handler) ? handler() : GetPoints();
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            var methodName = formationName?.Trim().Replace(" ", "") ?? string.Empty;

            if (Enum.TryParse(methodName, ignoreCase: true, out FormationNameEnum formation))
            {
                return GetFormationsSignalsList(formation);
            }

            return GetPoints();
        }

        public int GetFormationsSignalsCount(string formationName)
        {
            var signals = GetFormationsSignalsList(formationName);
            return signals?.Count(x => x.Signal) ?? 0;
        }

        public List<string> GetAllMethodNames()
        {
            return GetAllFormationNames();
        }

        public List<string> GetAllFormationNames()
        {
            return new List<string>(_formationNames);
        }

        public int GetSignalsCount(string formationName)
        {
            return GetFormationsSignalsCount(formationName);
        }
    }
}
