using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    public abstract class AbstractFibonnaci
    {
        internal abstract bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
        internal abstract bool SecondCheck(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList, string fibbPattern);
    }

    public class Fibonacci : AbstractFibonnaci, IFibonacci
    { 
        private static List<ZigZagObject> _data { get; set; }

        private static decimal _fibError { get; set; }

        private static Fibonacci3DrivePattern _drivePattern { get; set; }
        private static SupportClass _support { get; set; }

        private static List<decimal> _peaksFromZigZag { get; set; }

        private static List<ZigZagObject> _points { get; set; }

        public List<OhlcvObject> _dataOhlcv { get; }

        public static bool isDataLoaded { get; set; } = false;

        internal static List<decimal> range382 { get { return _support.PointsRange(38.2M, _fibError); } }
        internal static List<decimal> range786 { get { return _support.PointsRange(78.6M, _fibError); } }
        internal static List<decimal> range618 { get { return _support.PointsRange(61.8M, _fibError); } }
        internal static List<decimal> range886 { get { return _support.PointsRange(88.6M, _fibError); } }
        internal static List<decimal> range127 { get { return _support.PointsRange(127.2M, _fibError); } }
        internal static List<decimal> range161 { get { return _support.PointsRange(161.8M, _fibError); } }
        internal static List<decimal> range224 { get { return _support.PointsRange(224M, _fibError); } }

        public Fibonacci(List<OhlcvObject> dataOhlcv)
        {
            _dataOhlcv = dataOhlcv;
            _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, 0.002M);
            _points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
            _fibError = 0.1M; // in all Fibonacci ratios, errors of no more than 10% of the ideal value are allowed.  
            _drivePattern = new Fibonacci3DrivePattern();
            _support = new SupportClass();
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv, decimal zigZagParam)
        {
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
        }

        private List<ZigZagObject> BearishGartley() => Pattern("bearish", _points, _fibError, "gartleyPattern", 4);
        private List<ZigZagObject> BullishGartley() => Pattern("bullish", _points, _fibError, "gartleyPattern", 4);
        private List<ZigZagObject> BearishButterfly() => Pattern("bearish", _points, _fibError, "butterflyPattern", 4);
        private List<ZigZagObject> BullishButterfly() => Pattern("bullish", _points, _fibError, "butterflyPattern", 4);
        private List<ZigZagObject> BearishABCD() => Pattern("bearish", _points, _fibError, "abcdPattern", 3);
        private List<ZigZagObject> BullishABCD() => Pattern("bullish", _points, _fibError, "abcdPattern", 3);
        private List<ZigZagObject> Bearish3Extension() => Pattern("bearish", _points, _fibError, "threeExtensionPattern", 2);
        private List<ZigZagObject> Bullish3Extension() => Pattern("bullish", _points, _fibError, "threeExtensionPattern", 2);
        private List<ZigZagObject> Bearish3Retracement() => Pattern("bearish", _points, _fibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bullish3Retracement() => Pattern("bullish", _points, _fibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bearish3Drive() => _drivePattern.ThreeDrivePattern("bearish", _points, _fibError);
        private List<ZigZagObject> Bullish3Drive() => _drivePattern.ThreeDrivePattern("bullish", _points, _fibError);

        internal virtual List<ZigZagObject> Pattern(string pattern, List<ZigZagObject> points, decimal priceMovement1, string fibbPattern, int startNumber)
        {
            var dateList = new List<decimal>();

            for (int i = startNumber; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (FirstCheck(points, i, pattern, fibbPattern))
                    {
                        if (SecondCheck(points, i, pattern, dateList, fibbPattern))
                        {
                            _support.AddPointsToList(points, i, dateList, startNumber);
                        }
                    }
                }
            }
            return points;
        }

        internal override bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
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

            return false;
        }

        internal override bool SecondCheck(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList, string fibbPattern)
        {
            if (fibbPattern == "abcdPattern")
            {
                return SecondCheckForABCDPattern(points, i, pattern, dateList);
            }
            if (fibbPattern == "gartleyPattern")
            {
                return SecondCheckForGartleyPattern(points, i, pattern, dateList);
            }
            if (fibbPattern == "butterflyPattern")
            {
                return SecondCheckForButterflyPattern(points, i, pattern, dateList);
            }
            if (fibbPattern == "threeExtensionPattern")
            {
                return SecondCheckFor3ExtensionPattern(points, i, pattern, dateList);
            } if (fibbPattern == "threeRetracementPattern")
            {
                return SecondCheckFor3RetracementPattern(points, i, pattern, dateList);
            }

            return false;
        }

        private static bool FirstCheckForGartleyPattern(List<ZigZagObject> points, int i, string pattern)
        {
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

        private static bool SecondCheckForGartleyPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var retracementAbAx = _support.GetRetracement(points, i, 2, 3, 4); // 61,8% - 78,6%
            var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 38.2% – 88.6% 
            var retracementAdAX = _support.GetRetracement(points, i, 0, 3, 4); // between 78.6% and 88.6% 
            var retracementCdCb = _support.GetRetracement(points, i, 0, 1, 2); // between 127,2% and 161,8%
            var retracementCbBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% - 78,6%
            var retracementCdAb = (Math.Abs(points[i - 1].Close - points[i].Close) * 100) / (Math.Abs(points[i - 2].Close - points[i - 3].Close)); // 127,2% and 161,8%

            var check618_786retracement1 = _support.CheckIfRetracemntIsInRange(range618, range786, retracementAbAx);
            var check381_886retracement = _support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            var check786_886retracement = _support.CheckIfRetracemntIsInRange(range786, range886, retracementAdAX);
            var check127_161retracement1 = _support.CheckIfRetracemntIsInRange(range127, range161, retracementCdCb);
            var check618_786retracement2 = _support.CheckIfRetracemntIsInRange(range618, range786, retracementCbBa);
            var check127_161retracement2 = _support.CheckIfRetracemntIsInRange(range127, range161, retracementCdAb);

            if (check618_786retracement1 && check381_886retracement && check786_886retracement && check127_161retracement1 && check618_786retracement2 && check127_161retracement2)
            {
                return true; // i = 1788(bearish) and i = 1223(bullish)
            }

            return false;
        }

        private static bool FirstCheckButterflyPattern(List<ZigZagObject> points, int i, string pattern)
        {
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

        private static bool SecondCheckForButterflyPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var retracementBaXa = _support.GetRetracement(points, i, 2, 3, 4); // 78,6%
            //var retracementDaXa = _support.GetRetracement(points, i, 0, 3, 4); // 127,2% lub 161,8%
            var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 38,2%-88,6%
            //var retracementCdBc = _support.GetRetracement(points, i, 0, 1, 2); // 161,8%
            var retracementCdAb = (Math.Abs(points[i - 1].Close - points[i].Close) * 100) / (Math.Abs(points[i - 2].Close - points[i - 3].Close)); // 161,8% - 224%

            var check786retracement = _support.CheckIfRetracemntIsInRange(range786, range786, retracementBaXa);
            var check382_886retracement = _support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            var check161_224retracement = _support.CheckIfRetracemntIsInRange(range161, range224, retracementCdAb);

            if (check786retracement && check382_886retracement && check161_224retracement)
            {
                return true; // i=444(bearish) and  i= 627(bullish)
            }
            return false;
        }

        private static bool FirstCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern)
        {
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
        
        private static bool SecondCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var lenghtba = GetLenght(points, i, 2, 3);
            var lenghtdc = GetLenght(points, i, 0, 1);
            var diffLenght = Math.Abs(lenghtdc - lenghtba);

            if (diffLenght / ((lenghtba + lenghtdc)/2) <= _fibError)  // ustalić poziom różnicy, gdy ma być zbliżonae do zera
            {
                var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
                var retracementCdCb = _support.GetRetracement(points, i, 0, 1, 2); // 127,2% lub 161,8%

                var check618retracement = _support.CheckIfRetracemntIsInRange(range618, range618, retracementBcBa);
                var check786retracement = _support.CheckIfRetracemntIsInRange(range786, range786, retracementBcBa);
                var check127retracement = _support.CheckIfRetracemntIsInRange(range127, range127, retracementCdCb);
                var check161retracement = _support.CheckIfRetracemntIsInRange(range161, range161, retracementCdCb);


                if ((check618retracement || check786retracement) && (check127retracement || check161retracement))
                {
                    return true; //550(bearish) and i=55(bullish)
                } 

            }
            if (lenghtdc > lenghtba && pattern == "bullish")
            {
                var retracementBullishExtensionDcBa = _support.GetRetracement(points, i, 1, 2, 3); // 161,8% lub 127,2%
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

        private static bool SecondCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var retracement = _support.GetRetracement(points, i, 0, 1, 2);
            if (range618.Min() < retracement && range618.Max() > retracement)
            {
                return true;//i=27(bullish) i =14(bearish)
            }
            return false;
        }

        private static bool SecondCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var retracement = _support.GetRetracement(points, i, 0, 1, 2);
            if (pattern == "bullish")
            {
                if (range618.Min() < retracement && range618.Max() > retracement)
                {
                    return true; //i=27(bullish)
                }
            }

            if (pattern == "bearish")
            {
                if (range161.Min() < retracement && range161.Max() > retracement)
                {
                    return true;   //i=38(bearish)
                    
                }
            }
            return false;
        }

        private static bool FirstCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetFibonacciAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFibonacciSignalsList(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
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

        public int GetFibonacciSignalsCount(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            return GetFibonacciSignalsList(methodName).Where(x => x.Signal == true).Count();
        }

        public List<string> GetAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public int GetSignalsCount(string formationName)
        {
            var methodName = formationName.Trim().Replace(" ", "");
            return GetFormationsSignalsList(methodName).Where(x => x.Signal == true).Count();
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
    }
}
