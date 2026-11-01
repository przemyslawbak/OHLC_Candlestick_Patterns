using OHLC_Candlestick_Patterns;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Candlestick_Patterns
{

    public class Fibonacci : IFibonacci, ISignalEngine
    {
        private readonly List<ZigZagObject> _data;
        private readonly decimal _fibError;
        private readonly List<ZigZagObject> _peaksFromZigZag;
        private readonly ImmutableList<OhlcvObject> DataOhlcv;

        private static readonly SupportClass Support = new();

        internal List<decimal> GetRange382() => Support.PointsRange(38.2M, _fibError);
        internal List<decimal> GetRange786() => Support.PointsRange(78.6M, _fibError);
        internal List<decimal> GetRange618() => Support.PointsRange(61.8M, _fibError);
        internal List<decimal> GetRange886() => Support.PointsRange(88.6M, _fibError);
        internal List<decimal> GetRange127() => Support.PointsRange(127.2M, _fibError);
        internal List<decimal> GetRange161() => Support.PointsRange(161.8M, _fibError);
        internal List<decimal> GetRange224() => Support.PointsRange(224M, _fibError);

        private readonly ImmutableList<ZigZagObject> _cachedPoints;
        private readonly Dictionary<PatternType, Func<IReadOnlyList<ZigZagObject>, ImmutableList<ZigZagObject>>> _patternMethods;


        private readonly ConcurrentDictionary<PatternType, ImmutableList<ZigZagObject>> _patternResultsCache;
        private ImmutableList<ZigZagObject> GetCachedPoints()
        {
            return _cachedPoints;
        }


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
                    ConfirmationIndexOHLCV = point.ConfirmationIndexOHLCV,
                    Signal = false,
                    Initiation = point.Initiation
                });
            }

            return cachedCopy;
        }



        public Fibonacci(List<OhlcvObject> dataOhlcv) : this(dataOhlcv, 0.002M)
        {
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv, decimal zigZagParam)
        {
            DataOhlcv = dataOhlcv.ToImmutableList();
            _data = SetPeaksVallyes.GetCloseAndSignalsData(new List<OhlcvObject>(dataOhlcv));
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

        private static void AddPointsToList(IReadOnlyList<ZigZagObject> points, int i,
            int number, Dictionary<int, ZigZagObject> selected)
        {
            for (int index = i - number; index <= i; index++)
            {
                var source = points[index];
                if (!selected.TryGetValue(source.IndexOHLCV, out var existing))
                {
                    existing = new ZigZagObject
                    {
                        Close = source.Close,
                        IndexOHLCV = source.IndexOHLCV,
                        ConfirmationIndexOHLCV = source.ConfirmationIndexOHLCV
                    };
                    selected.Add(source.IndexOHLCV, existing);
                }

                // An overlapping pivot may be the end of one pattern and the start of another.
                existing.Signal |= index == i;
                existing.Initiation |= index == i - number;
            }
        }

        internal virtual ImmutableList<ZigZagObject> Pattern(string pattern,
            IReadOnlyList<ZigZagObject> points, decimal priceMovement1,
            string fibbPattern, int startNumber)
        {
            var selected = new Dictionary<int, ZigZagObject>();
            for (int i = startNumber; i < points.Count; i++)
            {
                if (!FirstCheck(points, i, pattern, fibbPattern))
                    continue;
                if (!SecondCheck(points, i, pattern, fibbPattern))
                    continue;
                AddPointsToList(points, i, startNumber, selected);
            }

            return selected.Values.OrderBy(p => p.IndexOHLCV).ToImmutableList();
        }

        internal bool FirstCheck(IReadOnlyList<ZigZagObject> points, int i, string pattern, string fibbPattern)
        {
            return fibbPattern switch
            {
                "abcdPattern" => FirstCheckForABCDPattern(points, i, pattern),
                "gartleyPattern" => FirstCheckForGartleyPattern(points, i, pattern),
                "butterflyPattern" => FirstCheckButterflyPattern(points, i, pattern),
                "threeExtensionPattern" => FirstCheckFor3ExtensionPattern(points, i, pattern),
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
            var retracementBcBa = Support.GetRetracement(points, i, 1, 2, 3);
            var retracementX0XA = Support.GetRetracement(points, i, 3, 4, 5);
            var extensionCdCb = Support.GetRetracement(points, i, 0, 1, 2);
            var extensionAxAc = Support.GetRetracement(points, i, 2, 3, 4);

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
            decimal abXa = Support.GetRetracement(points, i, 2, 3, 4);
            decimal bcAb = Support.GetRetracement(points, i, 1, 2, 3);
            decimal cdBc = Support.GetRetracement(points, i, 0, 1, 2);
            decimal adXa = Support.GetRetracement(points, i, 0, 3, 4);

            // Gartley: AB/XA = 0.618, BC/AB = 0.382..0.886,
            // CD/BC = 1.272..1.618 and AD/XA = 0.786.
            return InRange(abXa, 61.8M)
                && InRange(bcAb, 38.2M, 88.6M)
                && InRange(cdBc, 127.2M, 161.8M)
                && InRange(adXa, 78.6M);
        }

        private static bool FirstCheckButterflyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            if (i < 4) return false;
            var p = new PriceWindow(points, i);

            return pattern switch
            {
                "bullish" => p.C0 < p.C4 && p.C0 < p.C1 && p.C1 > p.C2 && p.C2 < p.C3 && p.C3 > p.C4,

                "bearish" => p.C0 > p.C4 && p.C0 > p.C1 && p.C1 < p.C2 && p.C2 > p.C3 && p.C3 < p.C4,
                _ => false
            };
        }

        private bool SecondCheckForButterflyPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            decimal abXa = Support.GetRetracement(points, i, 2, 3, 4);
            decimal bcAb = Support.GetRetracement(points, i, 1, 2, 3);
            decimal cdBc = Support.GetRetracement(points, i, 0, 1, 2);
            decimal adXa = Support.GetRetracement(points, i, 0, 3, 4);

            // Butterfly: AB/XA = 0.786, BC/AB = 0.382..0.886,
            // CD/BC = 1.618..2.618, and AD/XA = 1.272..1.618.
            return InRange(abXa, 78.6M)
                && InRange(bcAb, 38.2M, 88.6M)
                && InRange(cdBc, 161.8M, 261.8M)
                && InRange(adXa, 127.2M, 161.8M);
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
            decimal ab = GetLenght(points, i, 2, 3);
            decimal cd = GetLenght(points, i, 0, 1);
            if (ab == 0M || cd == 0M)
                return false;

            decimal bcAb = Support.GetRetracement(points, i, 1, 2, 3);
            decimal cdBc = Support.GetRetracement(points, i, 0, 1, 2);
            decimal relativeDifference = Math.Abs(cd - ab) / ((cd + ab) / 2M);

            // The classic AB=CD completion requires equal impulse legs
            // and reciprocal Fibonacci ratios for the intermediate correction.
            return relativeDifference <= _fibError
                && (InRange(bcAb, 61.8M) || InRange(bcAb, 78.6M))
                && (InRange(cdBc, 127.2M) || InRange(cdBc, 161.8M));
        }

        private bool InRange(decimal actual, decimal lower, decimal? upper = null)
        {
            if (actual <= 0M)
                return false;
            decimal high = upper ?? lower;
            return Support.CheckIfRetracemntIsInRange(
                Support.PointsRange(lower, _fibError),
                Support.PointsRange(high, _fibError), actual);
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
                "bullish" => p.C1 < p.C0 && p.C0 < p.C2,
                "bearish" => p.C1 > p.C0 && p.C0 > p.C2,
                _ => false
            };
        }

        private bool SecondCheckFor3RetracementPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            decimal retracement = Support.GetRetracement(points, i, 0, 1, 2);
            return InRange(retracement, 61.8M);
        }

        private bool SecondCheckFor3ExtensionPattern(IReadOnlyList<ZigZagObject> points, int i, string pattern)
        {
            decimal extension = Support.GetRetracement(points, i, 0, 2, 3);
            decimal retracement = Support.GetRetracement(points, i, 1, 2, 3);

            // Both directions use the same magnitudes; only their geometry differs.
            return InRange(retracement, 61.8M) && InRange(extension, 161.8M);
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
            if (string.IsNullOrWhiteSpace(patternName)) return _data.ToList();

            string normalizedName = patternName.Trim().Replace(" ", "");
            if (_patternNameMapping.TryGetValue(normalizedName, out PatternType patternType))
            {
                return GetPatternSignals(patternType).ToList();
            }

            return _data.ToList();
        }

        public ImmutableList<ZigZagObject> GetPatternSignals(PatternType pattern)
        {
            return _patternResultsCache.GetOrAdd(pattern, pt =>
            {
                if (_patternMethods.TryGetValue(pt, out var method))
                {
                    return method(GetCachedPoints());
                }

                return GetCachedPoints().ToImmutableList();
            });

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


        }

        public Dictionary<PatternType, List<ZigZagObject>> GetAllPatternSignals()
        {
            var results = new Dictionary<PatternType, List<ZigZagObject>>(_patternMethods.Count);
            foreach (var kvp in _patternMethods)
            {
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
