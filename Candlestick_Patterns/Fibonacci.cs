using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    public abstract class AbstractFibonnaci
    {
        internal abstract bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
        internal abstract bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
    }

    public class Fibonacci : AbstractFibonnaci, IFibonacci, ISignalEngine
    { 
        private static List<ZigZagObject> Data { get; set; }
        private static decimal FibError { get; set; }
        private static SupportClass Support { get; set; }
        private static List<ZigZagObject> PeaksFromZigZag { get; set; }
        private static List<ZigZagObject> Points { get; set; }
        public List<OhlcvObject> DataOhlcv { get; }
        public static bool IsDataLoaded { get; set; } = false;

        internal static List<decimal> range382 { get { return Support.PointsRange(38.2M, FibError); } }
        internal static List<decimal> range786 { get { return Support.PointsRange(78.6M, FibError); } }
        internal static List<decimal> range618 { get { return Support.PointsRange(61.8M, FibError); } }
        internal static List<decimal> range886 { get { return Support.PointsRange(88.6M, FibError); } }
        internal static List<decimal> range127 { get { return Support.PointsRange(127.2M, FibError); } }
        internal static List<decimal> range161 { get { return Support.PointsRange(161.8M, FibError); } }
        internal static List<decimal> range224 { get { return Support.PointsRange(224M, FibError); } }

        private List<ZigZagObject> _cachedPoints;
        private Dictionary<PatternType, Func<List<ZigZagObject>>> _patternMethods;
        private List<ZigZagObject> GetCachedPoints()
        {
            if (_cachedPoints == null)
            {
                _cachedPoints = PeaksFromZigZag; 
                ResetSignals(_cachedPoints);
            }
            return _cachedPoints;
        }

        internal static void ResetSignals(List<ZigZagObject> _peaksFromZigZag)
        {
            for (int i = 0; i < _peaksFromZigZag.Count; i++)
            {
                _peaksFromZigZag[i].Signal = false;
            }
        }

        private void InitializePatternMethods()
        {
            _patternMethods = new Dictionary<PatternType, Func<List<ZigZagObject>>>
            {
                { PatternType.BullishGartley, BullishGartley },
                { PatternType.BearishGartley, BearishGartley },
                { PatternType.Bullish3Drive, Bullish3Drive },
                { PatternType.Bearish3Drive, Bearish3Drive },
                { PatternType.BearishButterfly, BearishButterfly },
                { PatternType.BullishButterfly, BullishButterfly },
                { PatternType.BearishABCD, BearishABCD },
                { PatternType.BullishABCD, BullishABCD },
                { PatternType.Bearish3Extension, Bearish3Extension },
                { PatternType.Bullish3Extension, Bullish3Extension },
                { PatternType.Bullish3Retracement, Bullish3Retracement },
                { PatternType.Bearish3Retracement, Bearish3Retracement },
            };
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv)
        {
            DataOhlcv = dataOhlcv;
            Data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            PeaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(Data, 0.002M);
            FibError = 0.1M; // in all Fibonacci ratios, errors of no more than 10% of the ideal value are allowed.  
            Support = new SupportClass();
            InitializePatternMethods();
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv, decimal zigZagParam)
        {
            DataOhlcv = dataOhlcv;
            Data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            PeaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(Data, zigZagParam);
            FibError = 0.1M;
            Support = new SupportClass();
            InitializePatternMethods();
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

        private List<ZigZagObject> BearishGartley() => Pattern("bearish", GetCachedPoints(), FibError, "gartleyPattern", 4);
        private List<ZigZagObject> BullishGartley() => Pattern("bullish", GetCachedPoints(), FibError, "gartleyPattern", 4);
        private List<ZigZagObject> BearishButterfly() => Pattern("bearish", GetCachedPoints(), FibError, "butterflyPattern", 4);
        private List<ZigZagObject> BullishButterfly() => Pattern("bullish", GetCachedPoints(), FibError, "butterflyPattern", 4);
        private List<ZigZagObject> BearishABCD() => Pattern("bearish", GetCachedPoints(), FibError, "abcdPattern", 3);
        private List<ZigZagObject> BullishABCD() => Pattern("bullish", GetCachedPoints(), FibError, "abcdPattern", 3);
        private List<ZigZagObject> Bearish3Extension() => Pattern("bearish", GetCachedPoints(), FibError, "threeExtensionPattern", 3);
        private List<ZigZagObject> Bullish3Extension() => Pattern("bullish", GetCachedPoints(), FibError, "threeExtensionPattern", 3);
        private List<ZigZagObject> Bearish3Retracement() => Pattern("bearish", GetCachedPoints(), FibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bullish3Retracement() => Pattern("bullish", GetCachedPoints(), FibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bearish3Drive() => Pattern("bearish", GetCachedPoints(), FibError, "threeDrivePattern", 5);
        private List<ZigZagObject> Bullish3Drive() => Pattern("bullish", GetCachedPoints(), FibError, "threeDrivePattern", 5);

        internal virtual List<ZigZagObject> Pattern(string pattern, List<ZigZagObject> points, decimal priceMovement1, string fibbPattern, int startNumber)
        {
            var dateList = new List<decimal>();

            for (int i = startNumber; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - startNumber].IndexOHLCV))
                {
                    if (FirstCheck(points, i, pattern, fibbPattern))
                    {
                        if (SecondCheck(points, i, pattern, fibbPattern))
                        {
                            Support.AddPointsToList(points, i, dateList, startNumber);
                        }
                    }
                }
            }

            return points;
        }

        /*internal override bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
        {
            if (fibbPattern == "abcdPattern")
            {
                return FirstCheckForABCDPattern(points, i, pattern);
            }
            if (fibbPattern == "gartleyPattern")
            {
                return FirstCheckForGartleyPattern(points, i, pattern);
            }
            if (fibbPattern == "butterflyPattern")
            {
                return FirstCheckButterflyPattern(points, i, pattern);
            }
            if (fibbPattern == "threeExtensionPattern")
            {
                return FirstCheckFor3ExtensionPattern(points, i, pattern);
            }
            if (fibbPattern == "threeRetracementPattern")
            {
                return FirstCheckFor3RetracementPattern(points, i, pattern);
            }
            if (fibbPattern == "threeDrivePattern")
            {
                return FirstCheckFor3DrivePattern(points, i, pattern);
            }

            return false;
        }*/

        internal override bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
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

        internal override bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
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

        /*internal override bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
        {
            if (fibbPattern == "abcdPattern")
            {
                return SecondCheckForABCDPattern(points, i, pattern);
            }
            if (fibbPattern == "gartleyPattern")
            {
                return SecondCheckForGartleyPattern(points, i, pattern);
            }
            if (fibbPattern == "butterflyPattern")
            {
                return SecondCheckForButterflyPattern(points, i, pattern);
            }
            if (fibbPattern == "threeExtensionPattern")
            {
                return SecondCheckFor3ExtensionPattern(points, i, pattern);
            } 
            if (fibbPattern == "threeRetracementPattern")
            {
                return SecondCheckFor3RetracementPattern(points, i, pattern);
            }
            if (fibbPattern == "threeDrivePattern")
            {
                return SecondCheckFor3DrivePattern(points, i, pattern);
            }
            return false;
        }*/

        private readonly struct PriceWindow
        {
            public readonly decimal C0, C1, C2, C3, C4, C5;

            public PriceWindow(List<ZigZagObject> points, int i)
            {
                C0 = i >= 0 ? points[i].Close : 0;
                C1 = i >= 1 ? points[i - 1].Close : 0;
                C2 = i >= 2 ? points[i - 2].Close : 0;
                C3 = i >= 3 ? points[i - 3].Close : 0;
                C4 = i >= 4 ? points[i - 4].Close : 0;
                C5 = i >= 5 ? points[i - 5].Close : 0;
            }
        }

        private bool FirstCheckFor3DrivePattern(List<ZigZagObject> points, int i, string pattern)
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


            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close && points[i - 4].Close < points[i - 5].Close)
                {
                    if (points[i - 1].Close < points[i - 3].Close && points[i - 3].Close < points[i - 5].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close < points[i - 4].Close)
                    {
                        return true;
                    }
                }
            }

            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 4].Close > points[i - 5].Close)
                {
                    if (points[i - 1].Close > points[i - 3].Close && points[i - 3].Close > points[i - 5].Close && points[i - 2].Close < points[i].Close && points[i - 2].Close > points[i - 4].Close)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SecondCheckFor3DrivePattern(List<ZigZagObject> points, int i, string pattern) 
        {
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
            var retracementX0XA = Support.GetRetracement(points, i, 3, 4, 5); // 61,8% lub 78,6%
            var extensionCdCb = Support.GetRetracement(points, i, 0, 1, 2);  // 127% 
            var extensionAxAc = Support.GetRetracement(points, i, 2, 3, 4);  // 127% 

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

            if ((check618retracementBcBa || check786retracementBcBa) && (check786retracementX0XA || check618retracementX0XA) && check127extensionCdCb && check127extensionAxAc)
            {
                return true;
            }

            return false;
        }

        private static bool FirstCheckForGartleyPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (i < 4) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4 && p.C2 > p.C0 && p.C2 > p.C4,
                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4 && p.C2 < p.C0 && p.C2 < p.C4,
                _ => false
            };


            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close > points[i - 4].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 2].Close < points[i].Close && points[i - 2].Close < points[i - 4].Close)
                {
                    return true;
                }
            }
            return false; 

        }

        private static bool SecondCheckForGartleyPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var retracementAbAx = Support.GetRetracement(points, i, 2, 3, 4); // 61,8% - 78,6%
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 38.2% – 88.6% 
            var retracementCbBa = Support.GetRetracement(points, i, 1, 2, 3); // 61,8% - 78,6%
            var retracementCdCb = Support.GetRetracement(points, i, 0, 1, 2); // between 127,2% and 161,8%
            var retracementAdAX = Support.GetRetracement(points, i, 0, 3, 4); // between 78.6% and 88.6% 

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

            if (check618_786retracement1 && (check381_886retracement || check618_786retracement2) && check786_886retracement && check127_161retracement1)
            {
                return true;
            }

            return false;
        }

        private static bool FirstCheckButterflyPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (i < 4) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4 && p.C2 > p.C0 && p.C2 > p.C4,

                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4 && p.C2 < p.C0 && p.C2 < p.C4,
                _ => false
            };

            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close && points[i - 2].Close > points[i].Close && points[i - 2].Close > points[i - 4].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 2].Close < points[i].Close && points[i - 2].Close < points[i - 4].Close)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool SecondCheckForButterflyPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var retracementBaXa = Support.GetRetracement(points, i, 2, 3, 4); // 78,6%
            //var retracementDaXa = _support.GetRetracement(points, i, 0, 3, 4); // 127,2% lub 161,8%
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3); // 38,2%-88,6%
            //var retracementCdBc = _support.GetRetracement(points, i, 0, 1, 2); // 161,8%
            var retracementCdAb = (Math.Abs(points[i - 1].Close - points[i].Close) * 100) / (Math.Abs(points[i - 2].Close - points[i - 3].Close)); // 161,8% - 224%

            var check786retracement = Support.CheckIfRetracemntIsInRange(range786, range786, retracementBaXa);
            if (!check786retracement) return false;
            var check382_886retracement = Support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            if (!check382_886retracement) return false;
            var check161_224retracement = Support.CheckIfRetracemntIsInRange(range161, range224, retracementCdAb);
            return check161_224retracement;

            if (check786retracement && check382_886retracement && check161_224retracement)
            {
                return true;
            }
            return false;
        }

        private static bool FirstCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (i < 3) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3,
                "bearish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3,
                _ => false
            };

            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool SecondCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var lenghtba = GetLenght(points, i, 2, 3);
            var lenghtdc = GetLenght(points, i, 0, 1);
            var diffLenght = Math.Abs(lenghtdc - lenghtba);

            if (diffLenght / ((lenghtba + lenghtdc) / 2) <= FibError)  // ustalić poziom różnicy, gdy ma być zbliżonae do zera
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

        private static decimal GetLenght(List<ZigZagObject> points, int i, int number1, int number2)
        {
            var lenght = Math.Abs(points[i - number1].Close - points[i - number2].Close);
            return lenght;
        }

        private static bool FirstCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (i < 2) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C1 < p.C2 && p.C1 < p.C0,
                "bearish" => p.C2 < p.C1 && p.C1 > p.C2,
                _ => false
            };

            if (pattern == "bearish")
            {
                if (points[i - 1].Close < points[i - 2].Close && points[i - 1].Close < points[i].Close)
                {
                    return true;
                }
            }
            if (pattern == "bullish")
            {
                if (points[i - 2].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool SecondCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var retracement = Support.GetRetracement(points, i, 0, 1, 2);//
            if (range618.Min() < retracement && range618.Max() > retracement)
            {
                return true;
            }
            return false;
        }

        private static bool SecondCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var extension = Support.GetRetracement(points, i, 0, 2, 3); //signal for level 161,8% 
            var retracement = Support.GetRetracement(points, i, 1, 2, 3);
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

        private static bool FirstCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (i < 3) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3,
                "bearish" => p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3,
                _ => false
            };

            if (pattern == "bullish") 
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close <points[i - 2].Close &&points[i - 2].Close > points[i - 3].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close >points[i - 2].Close &&points[i - 2].Close < points[i - 3].Close)
                {
                    return true;
                }
            }
            return false;
        }

        

        public List<string> GetFibonacciAllMethodNames()
        {
            return _patternNameMapping.Keys.ToList();

            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.IgnoreCase |BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFibonacciSignalsList(string patternName)
        {
            if (string.IsNullOrWhiteSpace(patternName)) return Data;
            string normalizedName = patternName.Trim().Replace(" ", "");
            if (_patternNameMapping.TryGetValue(normalizedName, out PatternType patternType))
            {
                return GetPatternSignals(patternType);
            }

            return Data;

            var methodName = patternName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod != null)
            {
                List<ZigZagObject> result = (List<ZigZagObject>)theMethod.Invoke(this, null);
                return result;
            }
            else
            {
                return Data;
            }
        }
        public List<ZigZagObject> GetPatternSignals(PatternType pattern)
        {
            if (_patternMethods.TryGetValue(pattern, out var method))
            {
                return method();
            }
            return Data;
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

            var methodName = patternName.Trim().Replace(" ", "");
            return GetFibonacciSignalsList(methodName).Where(x => x.Signal == true).Count();
        }
        public int GetPatternSignalsCount(PatternType pattern)
        {
            if (!_patternMethods.TryGetValue(pattern, out var method)) return 0;
            var result = method();
            int count = 0;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Signal)
                    count++;
            }
            return count;
        }

        public List<string> GetAllMethodNames()
        {
            return _patternNameMapping.Keys.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();

            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }

            return methods.Where(x => x.Contains("Bullish") || x.Contains("Bearish") || x.Contains("Continuation")).ToList();
        }
        public List<ZigZagObject> GetSignals(string patternName)
        {
            if (TryParsePattern(patternName, out PatternType patternType))
            {
                return GetPatternSignals(patternType);
            }
            return Data;
        }

        public int GetSignalsCount(string formationName)
        {
            return GetFibonacciSignalsCount(formationName);

            var methodName = formationName.Trim().Replace(" ", "");
            return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
        }

        public List<ZigZagObject> GetFormationsSignalsList(string formationName)
        {
            return GetFibonacciSignalsList(formationName);

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
                return Data;
            }
        }
        public Dictionary<PatternType, int> GetAllPatternCounts()
        {
            var results = new Dictionary<PatternType, int>(_patternMethods.Count);
            foreach (var kvp in _patternMethods)
            {
                var signals = kvp.Value();
                int count = 0;
                for (int i = 0; i < signals.Count; i++)
                {
                    if (signals[i].Signal)
                        count++;
                }
                results[kvp.Key] = count;
            }

            return results;
        }

        public Dictionary<PatternType, List<ZigZagObject>> GetAllPatternSignals()
        {
            var results = new Dictionary<PatternType, List<ZigZagObject>>(_patternMethods.Count);
            foreach (var kvp in _patternMethods)
            {
                results[kvp.Key] = kvp.Value();
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
