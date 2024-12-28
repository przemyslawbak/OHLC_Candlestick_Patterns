using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    public abstract class AbstractFibonnaci
    {
        internal abstract bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
        internal abstract bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
    }

    public class Fibonacci : AbstractFibonnaci, IFibonacci
    { 
        private static List<ZigZagObject> _data { get; set; }

        private static decimal _fibError { get; set; }

        private static Fibonacci3DrivePattern _drivePattern { get; set; }
        private static SupportClass _support { get; set; }

        private static List<ZigZagObject> _peaksFromZigZag { get; set; }

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
            _fibError = 0.1M; // in all Fibonacci ratios, errors of no more than 10% of the ideal value are allowed.  
            _drivePattern = new Fibonacci3DrivePattern();
            _support = new SupportClass();
        }

        public Fibonacci(List<OhlcvObject> dataOhlcv, decimal zigZagParam)
        {
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data, zigZagParam);
        }

        private List<ZigZagObject> BearishGartley() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "gartleyPattern", 4);
        private List<ZigZagObject> BullishGartley() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "gartleyPattern", 4);
        private List<ZigZagObject> BearishButterfly() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "butterflyPattern", 4);
        private List<ZigZagObject> BullishButterfly() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "butterflyPattern", 4);
        private List<ZigZagObject> BearishABCD() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "abcdPattern", 3);
        private List<ZigZagObject> BullishABCD() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "abcdPattern", 3);
        private List<ZigZagObject> Bearish3Extension() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeExtensionPattern", 3);
        private List<ZigZagObject> Bullish3Extension() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeExtensionPattern", 3);
        private List<ZigZagObject> Bearish3Retracement() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bullish3Retracement() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeRetracementPattern", 2);
        private List<ZigZagObject> Bearish3Drive() => Pattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeDrivePattern", 5);
        private List<ZigZagObject> Bullish3Drive() => Pattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError, "threeDrivePattern", 5);

        //not in use at the time
        //private List<ZigZagObject> Bearish3Drive() => _drivePattern.ThreeDrivePattern("bearish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError);
        //private List<ZigZagObject> Bullish3Drive() => _drivePattern.ThreeDrivePattern("bullish", SetPeaksVallyes.GetPoints(_peaksFromZigZag), _fibError);

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
             if (fibbPattern == "threeDrivePattern")
            {
                return FirstCheckFor3DrivePattern(points, i, pattern);
            }

            return false;
        }

        internal override bool SecondCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern)
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
        }

        private bool FirstCheckFor3DrivePattern(List<ZigZagObject> points, int i, string pattern)
        {
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
            var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
            var retracementX0XA = _support.GetRetracement(points, i, 3, 4, 5); // 61,8% lub 78,6%
            var extensionCdCb = _support.GetRetracement(points, i, 0, 1, 2);  // 127% 
            var extensionAxAc = _support.GetRetracement(points, i, 2, 3, 4);  // 127% 

            var check618retracementBcBa = _support.CheckIfRetracemntIsInRange(range618, range618, retracementBcBa);
            var check786retracementBcBa = _support.CheckIfRetracemntIsInRange(range786, range786, retracementBcBa);
            var check618retracementX0XA = _support.CheckIfRetracemntIsInRange(range618, range618, retracementX0XA);
            var check786retracementX0XA = _support.CheckIfRetracemntIsInRange(range786, range786, retracementX0XA);
            var check127extensionCdCb = _support.CheckIfRetracemntIsInRange(range127, range127, extensionCdCb);
            var check127extensionAxAc = _support.CheckIfRetracemntIsInRange(range127, range127, extensionAxAc);


            if ((check618retracementBcBa || check786retracementBcBa) && (check786retracementX0XA || check618retracementX0XA) && check127extensionCdCb && check127extensionAxAc)
            {
                return true;
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

        private static bool SecondCheckForGartleyPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var retracementAbAx = _support.GetRetracement(points, i, 2, 3, 4); // 61,8% - 78,6%
            var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 38.2% – 88.6% 
            var retracementCbBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% - 78,6%
            var retracementCdCb = _support.GetRetracement(points, i, 0, 1, 2); // between 127,2% and 161,8%
            var retracementAdAX = _support.GetRetracement(points, i, 0, 3, 4); // between 78.6% and 88.6% 
            //var retracementCdAb = (Math.Abs(points[i - 1].Close - points[i].Close) * 100) / (Math.Abs(points[i - 2].Close - points[i - 3].Close)); // 127,2% and 161,8%

            var check618_786retracement1 = _support.CheckIfRetracemntIsInRange(range618, range786, retracementAbAx);
            var check381_886retracement = _support.CheckIfRetracemntIsInRange(range382, range886, retracementBcBa);
            var check786_886retracement = _support.CheckIfRetracemntIsInRange(range786, range886, retracementAdAX);
            var check127_161retracement1 = _support.CheckIfRetracemntIsInRange(range127, range161, retracementCdCb);
            var check618_786retracement2 = _support.CheckIfRetracemntIsInRange(range618, range786, retracementCbBa);
            //var check127_161retracement2 = _support.CheckIfRetracemntIsInRange(range127, range161, retracementCdAb);

            if (check618_786retracement1 && (check381_886retracement || check618_786retracement2) && check786_886retracement && check127_161retracement1 /*&& check127_161retracement2*/)
            {
                return true; 
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

        private static bool SecondCheckForButterflyPattern(List<ZigZagObject> points, int i, string pattern)
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
                return true; 
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
        
        private static bool SecondCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern)
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
                    return true; 
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

        private static bool SecondCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern)
        {
            var retracement = _support.GetRetracement(points, i, 0, 1, 2);//
            if (range618.Min() < retracement && range618.Max() > retracement)
            {
                return true;
            }
            return false;
        }

        private static bool SecondCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern)
        {

            var extension = _support.GetRetracement(points, i, 0, 2, 3); //signal for level 161,8% 
            var retracement = _support.GetRetracement(points, i, 1, 2, 3);
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
            if (pattern == "bullish") 
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close)
                {
                    return true;
                }
            }
            if (pattern == "bearish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetFibonacciAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.IgnoreCase |BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFibonacciSignalsList(string patternName)
        {
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
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance))
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
    }
}
