using OHLC_Candlestick_Patterns;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Candlestick_Patterns
{
    //public abstract class AbstractFibonnaci
    //{
    //    internal abstract bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
    //    internal abstract bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
    //}

    public class Fibonacci :/* AbstractFibonnaci,*/ IFibonacci, ISignalEngine
    {
        private readonly List<ZigZagObject> _data;
        private readonly decimal _fibError;
        private readonly List<ZigZagObject> _peaksFromZigZag;
        //private  List<ZigZagObject> Points { get; set; }
        private readonly ImmutableList<OhlcvObject> DataOhlcv;

        private static readonly SupportClass Support = new();
        //public  bool IsDataLoaded { get; set; } = false;

        internal  List<decimal> GetRange382() => Support.PointsRange(38.2M, _fibError);
        internal  List<decimal> GetRange786() => Support.PointsRange(78.6M, _fibError);
        internal  List<decimal> GetRange618() => Support.PointsRange(61.8M, _fibError);
        internal  List<decimal> GetRange886() => Support.PointsRange(88.6M, _fibError);
        internal  List<decimal> GetRange127() => Support.PointsRange(127.2M, _fibError);
        internal  List<decimal> GetRange161() => Support.PointsRange(161.8M, _fibError);
        internal  List<decimal> GetRange224() => Support.PointsRange(224M, _fibError);

        private readonly ImmutableList<ZigZagObject> _cachedPoints; 
        private readonly Dictionary<PatternType, Func<IReadOnlyList<ZigZagObject>, ImmutableList<ZigZagObject>>> _patternMethods;


        private readonly ConcurrentDictionary<PatternType, ImmutableList<ZigZagObject>> _patternResultsCache;
        private ImmutableList<ZigZagObject> GetCachedPoints()
        {
            return _cachedPoints;
        }

        //private List<ZigZagObject> GetCachedPoints()
        //{
        //    return _cachedPoints.Value;

        //    //if (_cachedPoints == null)
        //    //{
        //    //    _cachedPoints = PeaksFromZigZag; 
        //    //    ResetSignals(_cachedPoints);
        //    //}
        //    //return _cachedPoints;
        //}
        private List<ZigZagObject> InitializeCachedPoints()
        {
            var cachedCopy = new List<ZigZagObject>(_peaksFromZigZag.Count);

            for (int i = 0; i < _peaksFromZigZag.Count; i++)
            {
                var point = _peaksFromZigZag[i];
                cachedCopy.Add(new ZigZagObject
                {
                    Close = point.Close,
                    IndexOHLCV = point.IndexOHLCV,
                    Signal = false, 
                    Initiation = point.Initiation
                });
            }

            return cachedCopy;
        }

        //internal static void ResetSignals(List<ZigZagObject> _peaksFromZigZag)
        //{
        //    for (int i = 0; i < _peaksFromZigZag.Count; i++)
        //    {
        //        _peaksFromZigZag[i].Signal = false;
        //    }
        //}

        //private void InitializePatternMethods()
        //{
        //    _patternMethods = new Dictionary<PatternType, Func<List<ZigZagObject>>>
        //    {
        //        { PatternType.BullishGartley, BullishGartley },
        //        { PatternType.BearishGartley, BearishGartley },
        //        { PatternType.Bullish3Drive, Bullish3Drive },
        //        { PatternType.Bearish3Drive, Bearish3Drive },
        //        { PatternType.BearishButterfly, BearishButterfly },
        //        { PatternType.BullishButterfly, BullishButterfly },
        //        { PatternType.BearishABCD, BearishABCD },
        //        { PatternType.BullishABCD, BullishABCD },
        //        { PatternType.Bearish3Extension, Bearish3Extension },
        //        { PatternType.Bullish3Extension, Bullish3Extension },
        //        { PatternType.Bullish3Retracement, Bullish3Retracement },
        //        { PatternType.Bearish3Retracement, Bearish3Retracement },
        //    };
        //}

        public Fibonacci(List<OhlcvObject> dataOhlcv) : this(dataOhlcv, 0.002M)
        {
            //DataOhlcv = dataOhlcv;
            //Data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            //PeaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(Data, 0.002M);
            //_fibError = 0.1M; // in all Fibonacci ratios, errors of no more than 10% of the ideal value are allowed.  
            //Support = new SupportClass();
            //InitializePatternMethods();
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv, decimal zigZagParam)
        {
            DataOhlcv = dataOhlcv.ToImmutableList();
            _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
            _fibError = 0.1M;
            _cachedPoints = InitializeCachedPoints().ToImmutableList();
            _patternMethods = new Dictionary<PatternType, Func<IReadOnlyList<ZigZagObject>, ImmutableList<ZigZagObject>>>
            {
                { PatternType.BullishGartley, pts => BullishGartley(pts) },
                { PatternType.BearishGartley, pts => BearishGartley(pts) },
                { PatternType.BullishButterfly, pts => BullishButterfly(pts) },
                { PatternType.BearishButterfly, pts => BearishButterfly(pts) },
                { PatternType.BullishABCD, pts => BullishABCD(pts) },
                { PatternType.BearishABCD, pts => BearishABCD(pts) },
                { PatternType.Bullish3Extension, pts => Bullish3Extension(pts) },
                { PatternType.Bearish3Extension, pts => Bearish3Extension(pts) },
                { PatternType.Bullish3Retracement, pts => Bullish3Retracement(pts) },
                { PatternType.Bearish3Retracement, pts => Bearish3Retracement(pts) },
                { PatternType.Bullish3Drive, pts => Bullish3Drive(pts) },
                { PatternType.Bearish3Drive, pts => Bearish3Drive(pts) },
            };

            _patternResultsCache = new ConcurrentDictionary<PatternType, ImmutableList<ZigZagObject>>();

            //DataOhlcv = dataOhlcv;
            //Data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            //PeaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(Data, zigZagParam);
            //_fibError = 0.1M;
            //Support = new SupportClass();
            //InitializePatternMethods();
        }

        private static readonly Dictionary<string, PatternType> _patternNameMapping = new Dictionary<string, PatternType>(StringComparer.OrdinalIgnoreCase)
        {
            { "BullishGartley", PatternType.BullishGartley },
            { "BearishGartley", PatternType.BearishGartley },
            { "Bullish3Drive", PatternType.Bullish3Drive },
            { "Bearish3Drive", PatternType.Bearish3Drive },
            { "BearishButterfly", PatternType.BearishButterfly },
            { "BullishButterfly", PatternType.BullishButterfly },
            { "BearishABCD", PatternType.BearishABCD },
            { "BullishABCD", PatternType.BullishABCD },
            { "Bearish3Extension", PatternType.Bearish3Extension },
            { "Bullish3Extension", PatternType.Bullish3Extension },
            { "Bullish3Retracement", PatternType.Bullish3Retracement },
            { "Bearish3Retracement", PatternType.Bearish3Retracement },
        };

        public enum PatternType
        {
            BullishGartley,
            BearishGartley,
            Bullish3Drive,
            Bearish3Drive,
            BearishButterfly,
            BullishButterfly,
            BearishABCD,
            BullishABCD,
            Bearish3Extension,
            Bullish3Extension,
            Bullish3Retracement,
            Bearish3Retracement,
        }

        private ImmutableList<ZigZagObject> BearishGartley(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "gartleyPattern", 4);
        private ImmutableList<ZigZagObject> BullishGartley(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "gartleyPattern", 4);
        private ImmutableList<ZigZagObject> BearishButterfly(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "butterflyPattern", 4);
        private ImmutableList<ZigZagObject> BullishButterfly(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "butterflyPattern", 4);
        private ImmutableList<ZigZagObject> BearishABCD(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "abcdPattern", 3);
        private ImmutableList<ZigZagObject> BullishABCD(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "abcdPattern", 3);
        private ImmutableList<ZigZagObject> Bearish3Extension(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "threeExtensionPattern", 3);
        private ImmutableList<ZigZagObject> Bullish3Extension(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "threeExtensionPattern", 3);
        private ImmutableList<ZigZagObject> Bearish3Retracement(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "threeRetracementPattern", 2);
        private ImmutableList<ZigZagObject> Bullish3Retracement(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "threeRetracementPattern", 2);
        private ImmutableList<ZigZagObject> Bearish3Drive(IReadOnlyList<ZigZagObject> points) => Pattern("bearish", points, _fibError, "threeDrivePattern", 5);
        private ImmutableList<ZigZagObject> Bullish3Drive(IReadOnlyList<ZigZagObject> points) => Pattern("bullish", points, _fibError, "threeDrivePattern", 5);

        private static void AddPointsToList(IReadOnlyList<ZigZagObject> points, int i, int number, HashSet<decimal> usedIndexes, List<ZigZagObject> result)
        {
            for (int x = -number; x <= 0; x++)
            {
                int index = i + x;
                if (index < 0 || index >= points.Count)
                    continue;

                var original = points[index];

                if (!usedIndexes.Add(original.IndexOHLCV))
                    continue;

                result.Add(new ZigZagObject
                {
                    IndexOHLCV = original.IndexOHLCV,
                    Close = original.Close,
                    Signal = index == i,
                    Initiation = index == i - number
                });
            }

            //for (int x = -number; x <= 0; x++)
            //{
            //    int index = i + x;
            //    if (index >= 0 && index < points.Count)  // Extra safety
            //    {
            //        dateSet.Add(points[index].IndexOHLCV);
            //    }
            //}

            //// Mark pattern points
            //points[i].Signal = true;
            //points[i - number].Initiation = true;

            //return points;

            /*
            for (int x = -number; x < 1; x++)
            {
                dateList.Add(points[i + x].IndexOHLCV);
            }
            points[i].Signal = true;
            points[i - number].Initiation = true;
            return points;*/
        }

        internal virtual ImmutableList<ZigZagObject> Pattern(string pattern, IReadOnlyList<ZigZagObject> points, decimal priceMovement1, string fibbPattern, int startNumber)
        {
            var usedIndexes = new HashSet<decimal>();
            var result = new List<ZigZagObject>();

            for (int i = startNumber; i < points.Count; i++)
            {
                var baseIndex = points[i - startNumber].IndexOHLCV;

                if (usedIndexes.Contains(baseIndex))
                    continue;

                if (!FirstCheck(points, i, pattern, fibbPattern))
                    continue;

                if (!SecondCheck(points, i, pattern, fibbPattern))
                    continue;

                AddPointsToList(points, i, startNumber, usedIndexes, result);
            }

            return result.ToImmutableList(); 
            ////var dateList = new List<decimal>();
            //var dateList = new HashSet<decimal>();

            //for (int i = startNumber; i < points.Count; i++)
            //{
            //    if (!dateList.Contains(points[i - startNumber].IndexOHLCV))
            //    {
            //        if (FirstCheck(points, i, pattern, fibbPattern))
            //        {
            //            if (SecondCheck(points, i, pattern, fibbPattern))
            //            {
            //                Support.AddPointsToList(points, i, dateList, startNumber);
            //            }
            //        }
            //    }
            //}

            //return points;
        }
       
        internal bool FirstCheck(IReadOnlyList<ZigZagObject> points, int i, string pattern, string fibbPattern)
        {
            return fibbPattern switch
            {
                "abcdPattern" => FirstCheckForABCDPattern(points, i, pattern),
                "gartleyPattern" => FirstCheckForGartleyPattern(points, i, pattern),
                "butterflyPattern" => FirstCheckButterflyPattern(points, i, pattern),
                "threeExtensionPattern"=> FirstCheckFor3ExtensionPattern(points, i, pattern),
                "threeRetracementPattern" => FirstCheckFor3RetracementPattern(points, i, pattern),
                "threeDrivePattern" => FirstCheckFor3DrivePattern(points, i, pattern),
                 _ => false
            };
        }

        internal bool SecondCheck(IReadOnlyList<ZigZagObject> points, int i, string pattern, string fibbPattern)
        {
            return fibbPattern switch
            {
                "abcdPattern" => SecondCheckForABCDPattern(points, i, pattern),
                "gartleyPattern" => SecondCheckForGartleyPattern(points, i, pattern),
                "butterflyPattern" => SecondCheckForButterflyPattern(points, i, pattern),
                "threeExtensionPattern" => SecondCheckFor3ExtensionPattern(points, i, pattern),
                "threeRetracementPattern" => SecondCheckFor3RetracementPattern(points, i, pattern),
                "threeDrivePattern" => SecondCheckFor3DrivePattern(points, i, pattern),
                _ => false
            };
        }
       
        private readonly struct PriceWindow
        {
            public readonly decimal C0, C1, C2, C3, C4, C5;

            public PriceWindow(IReadOnlyList<ZigZagObject> points, int i)
            {
                C0 = i >= 0 ? points[i].Close : 0;
                C1 = i >= 1 ? points[i - 1].Close : 0;
                C2 = i >= 2 ? points[i - 2].Close : 0;
                C3 = i >= 3 ? points[i - 3].Close : 0;
                C4 = i >= 4 ? points[i - 4].Close : 0;
                C5 = i >= 5 ? points[i - 5].Close : 0;
            }
        }

        private bool FirstCheckFor3DrivePattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 5) return false;
            var p = new PriceWindow(points, i);

            if (pattern == "bullish")
            {
                return p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4 && p.C4 < p.C5
                    && p.C1 < p.C3 && p.C3 < p.C5 && p.C2 > p.C0 && p.C2 < p.C4;
            }

            if (pattern == "bearish")
            {
                return (p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4 && p.C4 > p.C5)
                    && (p.C1 > p.C3 && p.C3 > p.C5 && p.C2 < p.C0 && p.C2 > p.C4);
            }

            return false;
        }

        private bool SecondCheckFor3DrivePattern(IReadOnlyList<ZigZagObject> points, int i, string pattern) 
        {
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
            var retracementX0XA = Support.GetRetracement(points, i, 3, 4, 5); // 61,8% lub 78,6%
            var extensionCdCb = Support.GetRetracement(points, i, 0, 1, 2);  // 127% 
            var extensionAxAc = Support.GetRetracement(points, i, 2, 3, 4);  // 127% 

            var range618 = GetRange618();
            var range786 = GetRange786();
            var range127 = GetRange127();

            var check618retracementBcBa = Support.CheckIfRetracemntIsInRange(range618, range618, retracementBcBa);
            var check786retracementBcBa = Support.CheckIfRetracemntIsInRange(range786, range786, retracementBcBa);
            if (!(check618retracementBcBa || check786retracementBcBa)) return false;
            var check618retracementX0XA = Support.CheckIfRetracemntIsInRange(range618, range618, retracementX0XA);
            var check786retracementX0XA = Support.CheckIfRetracemntIsInRange(range786, range786, retracementX0XA);
            if (!(check786retracementX0XA || check618retracementX0XA)) return false;
            var check127extensionCdCb = Support.CheckIfRetracemntIsInRange(range127, range127, extensionCdCb);
            if (!check127extensionCdCb) return false;
            var check127extensionAxAc = Support.CheckIfRetracemntIsInRange(range127, range127, extensionAxAc);

            return check127extensionAxAc;
        }

        private static bool FirstCheckForGartleyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 4) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4 && p.C2 > p.C0 && p.C2 > p.C4,
                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4 && p.C2 < p.C0 && p.C2 < p.C4,
                _ => false
            };
        }

        private bool SecondCheckForGartleyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            var retracementAbAx = Support.GetRetracement(points, i, 2, 3, 4); // 61,8% - 78,6%
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 38.2% – 88.6% 
            var retracementCbBa = Support.GetRetracement(points, i, 1, 2, 3); // 61,8% - 78,6%
            var retracementCdCb = Support.GetRetracement(points, i, 0, 1, 2); // between 127,2% and 161,8%
            var retracementAdAX = Support.GetRetracement(points, i, 0, 3, 4); // between 78.6% and 88.6% 

            var range618 = GetRange618();
            var range786 = GetRange786();
            var range127 = GetRange127();
            var range886 = GetRange886();
            var range161 = GetRange161();
            var range382 = GetRange382();
            var check618_786retracement1 = Support.CheckIfRetracemntIsInRange(range618, range786, retracementAbAx);
            if (!check618_786retracement1) return false;
            var check381_886retracement = Support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            var check618_786retracement2 = Support.CheckIfRetracemntIsInRange(range618, range786, retracementCbBa);
            if (!(check381_886retracement || check618_786retracement2)) return false;
            var check786_886retracement = Support.CheckIfRetracemntIsInRange(range786, range886, retracementAdAX);
            if (!check786_886retracement) return false;
            var check127_161retracement1 = Support.CheckIfRetracemntIsInRange(range127, range161, retracementCdCb);
            if (!check127_161retracement1) return false;

            return true;
        }

        private static bool FirstCheckButterflyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 4) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4 && p.C2 > p.C0 && p.C2 > p.C4,

                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4 && p.C2 < p.C0 && p.C2 < p.C4,
                _ => false
            };
        }

        private bool SecondCheckForButterflyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            var retracementBaXa = Support.GetRetracement(points, i, 2, 3, 4); // 78,6%
            //var retracementDaXa = _support.GetRetracement(points, i, 0, 3, 4); // 127,2% lub 161,8%
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 38,2%-88,6%
            //var retracementCdBc = _support.GetRetracement(points, i, 0, 1, 2); // 161,8%
            var retracementCdAb = (Math.Abs(points[i - 1].Close - points[i].Close) * 100) / (Math.Abs(points[i - 2].Close - points[i - 3].Close)); // 161,8% - 224%
            var range786 = GetRange786();
            var range886 = GetRange886();
            var range161 = GetRange161();
            var range224 = GetRange224();
            var range382 = GetRange382();
            var check786retracement = Support.CheckIfRetracemntIsInRange(range786, range786, retracementBaXa);
            if (!check786retracement) return false;
            var check382_886retracement = Support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            if (!check382_886retracement) return false;
            var check161_224retracement = Support.CheckIfRetracemntIsInRange(range161, range224, retracementCdAb);

            return check161_224retracement;
        }

        private static bool FirstCheckForABCDPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 3) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3,
                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3,
                _ => false
            };
        }

        private bool SecondCheckForABCDPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            var lenghtba = GetLenght(points, i, 2, 3);
            var lenghtdc = GetLenght(points, i, 0, 1);
            var diffLenght = Math.Abs(lenghtdc - lenghtba);

            var range618 = GetRange618();
            var range786 = GetRange786();
            var range127 = GetRange127();
            var range161 = GetRange161();

            if (diffLenght / ((lenghtba + lenghtdc) / 2) <= _fibError)  // ustalić poziom różnicy, gdy ma być zbliżonae do zera
            {
                var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
                var retracementCdCb = Support.GetRetracement(points, i, 0, 1, 2); // 127,2% lub 161,8%
                var check618retracement = Support.CheckIfRetracemntIsInRange(range618, range618, retracementBcBa);
                var check786retracement = Support.CheckIfRetracemntIsInRange(range786, range786, retracementBcBa);
                var check127retracement = Support.CheckIfRetracemntIsInRange(range127, range127, retracementCdCb);
                var check161retracement = Support.CheckIfRetracemntIsInRange(range161, range161, retracementCdCb);

                if ((check618retracement || check786retracement) && (check127retracement || check161retracement))
                {
                    return true;
                }

            }
            if (lenghtdc > lenghtba && pattern == "bullish")
            {
                var retracementBullishExtensionDcBa = Support.GetRetracement(points, i, 1, 2, 3); // 161,8% lub 127,2%
                if ((range161.Min() < retracementBullishExtensionDcBa && (range161.Max() > retracementBullishExtensionDcBa) || (range127.Min() < retracementBullishExtensionDcBa && range127.Max() > retracementBullishExtensionDcBa)))
                {
                    return true;
                }
            }

            return false;
        }

        private static decimal GetLenght(IReadOnlyList<ZigZagObject> points, int i, int number1, int number2)
        {
            var lenght = Math.Abs(points[i - number1].Close - points[i - number2].Close);
            return lenght;
        }

        private static bool FirstCheckFor3RetracementPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 2) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C1 < p.C2 && p.C1 < p.C0,
                "bearish" => p.C2 < p.C1 && p.C1 > p.C2,
                _ => false
            };
        }

        private bool SecondCheckFor3RetracementPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            var retracement = Support.GetRetracement(points, i, 0, 1, 2);
            var range618 = GetRange618();
            if (range618.Min() < retracement && range618.Max() > retracement)
            {
                return true;
            }
            return false;
        }

        private bool SecondCheckFor3ExtensionPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            var extension = Support.GetRetracement(points, i, 0, 2, 3); //signal for level 161,8% 
            var retracement = Support.GetRetracement(points, i, 1, 2, 3);
            var range618 = GetRange618();
            var range161 = GetRange161();

            if (pattern == "bullish")
            {
                if ((range618.Min() < retracement && range618.Max() > retracement) && (range161.Min() < extension /*&& range618.Max() > extension*/))
                {
                    return true;
                }
            }

            if (pattern == "bearish")
            {
                if ((range161.Min() < retracement && range161.Max() > retracement) && (range161.Min() < extension /*&& range618.Max() > extension*/))
                {
                    return true;

                }
            }
            return false;
        }

        private static bool FirstCheckFor3ExtensionPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 3) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3,
                "bearish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3,
                _ => false
            };
        }

        

        public List<string> GetFibonacciAllMethodNames()
        {
            return _patternNameMapping.Keys.ToList();
        }

        public List<ZigZagObject> GetFibonacciSignalsList(string patternName)
        {
            //if (string.IsNullOrWhiteSpace(patternName)) return Data;
            if (string.IsNullOrWhiteSpace(patternName)) return _data.ToList();

            string normalizedName = patternName.Trim().Replace(" ", "");
            if (_patternNameMapping.TryGetValue(normalizedName, out PatternType patternType))
            {
                return GetPatternSignals(patternType).ToList();
            }

            //return Data;
            return _data.ToList();
        }

        public ImmutableList<ZigZagObject> GetPatternSignals(PatternType pattern)
        {
            return _patternResultsCache.GetOrAdd(pattern, pt =>
            {
                if (_patternMethods.TryGetValue(pt, out var method))
                {
                    return method(GetCachedPoints()); // Pass points
                }

                return GetCachedPoints().ToImmutableList();
            });

            //if (_patternMethods.TryGetValue(pattern, out var method))
            //{
            //    return method();
            //}
            //return Data;
        }

        public int GetFibonacciSignalsCount(string patternName)
        {
            if (string.IsNullOrWhiteSpace(patternName)) return 0;
            string normalizedName = patternName.Trim().Replace(" ", "");
            if (_patternNameMapping.TryGetValue(normalizedName, out PatternType patternType))
            {
                return GetPatternSignalsCount(patternType);
            }

            return 0;
        }

        public int GetPatternSignalsCount(PatternType pattern)
        {
            var result = GetPatternSignals(pattern);
            int count = 0;
            foreach (var z in result)
            {
                if (z.Signal)
                    count++;
            }
            return count;

            //if (!_patternMethods.TryGetValue(pattern, out var method)) return 0;
            //var result = method();
            //int count = 0;
            //for (int i = 0; i < result.Count; i++)
            //{
            //    if (result[i].Signal)
            //        count++;
            //}
            //return count;
        }

        public List<string> GetAllMethodNames()
        {
            return _patternNameMapping.Keys.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();
        }

        public List<ZigZagObject> GetSignals(string patternName)
        {
            if (TryParsePattern(patternName, out PatternType patternType))
            {
                return GetPatternSignals(patternType).ToList(); 
            }

            return _data.ToList();
        }

        public int GetSignalsCount(string formationName)
        {
            return GetFibonacciSignalsCount(formationName);
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            return GetFibonacciSignalsList(formationName);
        }

        public Dictionary<PatternType, int> GetAllPatternCounts()
        {
            var results = new Dictionary<PatternType, int>(_patternMethods.Count);
            foreach (var kvp in _patternMethods)
            {
                int count = GetPatternSignalsCount(kvp.Key);
                results[kvp.Key] = count;
            }
            return results;

            //var results = new Dictionary<PatternType, int>(_patternMethods.Count);
            //foreach (var kvp in _patternMethods)
            //{
            //    //var signals = kvp.Value();
            //    var signals = GetPatternSignals(kvp.Key);
            //    int count = 0;
            //    for (int i = 0; i < signals.Count; i++)
            //    {
            //        if (signals[i].Signal)
            //            count++;
            //    }
            //    results[kvp.Key] = count;
            //}

            //return results;
        }

        public Dictionary<PatternType, List<ZigZagObject>> GetAllPatternSignals()
        {
            var results = new Dictionary<PatternType, List<ZigZagObject>>(_patternMethods.Count);
            foreach (var kvp in _patternMethods)
            {
                //results[kvp.Key] = kvp.Value();
                results[kvp.Key] = GetPatternSignals(kvp.Key).ToList();
            }

            return results;
        }

        public static bool TryParsePattern(string patternName, out PatternType patternType)
        {
            if (string.IsNullOrWhiteSpace(patternName))
            {
                patternType = default;
                return false;
            }

            string normalizedName = patternName.Trim().Replace(" ", "");
            return _patternNameMapping.TryGetValue(normalizedName, out patternType);
        }
    }
}
