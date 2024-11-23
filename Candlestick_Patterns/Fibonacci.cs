using OHLC_Candlestick_Patterns;
using System.Reflection;

namespace Candlestick_Patterns
{
    internal abstract class AbstractFibonnaci
    {
        internal abstract bool FirstCheck(List<ZigZagObject> points, int i, string pattern, string fibbPattern);
        internal abstract bool SecondCheck(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList, string fibbPattern);
    }

    internal class Fibonacci : AbstractFibonnaci, IFibonacci
    {
        private static List<ZigZagObject> _data { get; set; }

        private static decimal _fibError { get; set; }

        private static Fibonacci3DrivePattern _drivePattern { get; set; }
        private static SupportClass _support { get; set; }

        private static List<decimal> _peaksFromZigZag { get; set; }

        private static List<ZigZagObject> _points { get; set; }

        public List<OhlcvObject> _dataOhlcv { get; }

        public static bool isDataLoaded { get; set; } = false;

        internal Fibonacci(List<OhlcvObject> dataOhlcv)
        {
            if (isDataLoaded == false)
            {
                isDataLoaded = true;
                _dataOhlcv = dataOhlcv;
                _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
                _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data);
                _points = SetPeaksVallyes.GetPoints(_peaksFromZigZag);
                _fibError = 0.1M; // in all Fibonacci ratios, errors of no more than 10% of the ideal value are allowed.  
                _drivePattern = new Fibonacci3DrivePattern();
                _support = new SupportClass();
            }
        }

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
                            return _support.AddPointsToList(points, i, dateList, -startNumber);
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

        private bool FirstCheckButterflyPattern(List<ZigZagObject> points, int i, string pattern)
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
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 2].Close > points[i - 1].Close && points[i - 2].Close > points[i - 3].Close)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SecondCheckForButterflyPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var retracementBaXa = _support.GetRetracement(points, i, 2, 3, 4); // 78,6%
            var retracementDaXa = _support.GetRetracement(points, i, 0, 3, 4); // 127,2% lub 161,8%
            var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 38,2%-88,6%
            var retracementCdBc = _support.GetRetracement(points, i, 0, 1, 2); // 161,8%

            if (_support.PointsRange(78.6M, _fibError).Contains(retracementBaXa) && _support.PointsRange(161.8M, _fibError).Contains(retracementCdBc) &&
               (_support.PointsRange(127.2M, _fibError).Contains(retracementDaXa) || _support.PointsRange(161.8M, _fibError).Contains(retracementDaXa)) &&
               (_support.PointsRange(38.2M, _fibError).Min() < retracementBcBa && _support.PointsRange(88.6M, _fibError).Max() > retracementBcBa))
            {
                return true;
            }
            return false;
        }

        private bool FirstCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern)
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
        
        private bool SecondCheckForABCDPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            var lenghtba = GetLenght(points, i, 2, 3);
            var lenghtdc = GetLenght(points, i, 0, 1);
            var diffLenght = Math.Abs(lenghtdc - lenghtba);

            if (diffLenght / ((lenghtba + lenghtdc)/2) <= _fibError)  // ustalić poziom różnicy, gdy ma być zbliżonae do zera
            {
                var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
                var retracementCdCb = _support.GetRetracement(points, i, 0, 1, 2); // 127,2% lub 161,8%
                if ((_support.PointsRange(61.8M, _fibError).Contains(retracementBcBa) || _support.PointsRange(78.6M, _fibError).Contains(retracementBcBa)) &&
                   (_support.PointsRange(127.2M, _fibError).Contains(retracementCdCb) || _support.PointsRange(161.8M, _fibError).Contains(retracementCdCb)))
                {
                    return true;
                } 

            }
            if (lenghtdc > lenghtba && pattern == "bullish")
            {
                var retracementBullishExtensionDcBa = _support.GetRetracement(points, i, 1, 2, 3); // 161,8% lub 127,2%
                if (_support.PointsRange(161.8M, _fibError).Contains(retracementBullishExtensionDcBa) || _support.PointsRange(127.2M, _fibError).Contains(retracementBullishExtensionDcBa))
                {
                    return true;
                }
            }

            return false;
        }

        private decimal GetLenght(List<ZigZagObject> points, int i, int number1, int number2)
        {
            var lenght = Math.Abs(points[i - number1].Close - points[i - number2].Close);
            return lenght;
        }

        private bool FirstCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern)
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

        private bool SecondCheckFor3RetracementPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            if (_support.PointsRange(61.8M, _fibError).Contains(_support.GetRetracement(points, i, 0, 1, 2)))
            {
                return true;
            }
            return false;
        }

        private bool SecondCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            if (pattern == "bullish")
            {
                if (_support.PointsRange(61.8M, _fibError).Contains(_support.GetRetracement(points, i, 0, 1, 2)))
                {
                    return true; 
                }
            }

            if (pattern == "bearish")
            {
                if (_support.PointsRange(161.8M, _fibError).Contains(_support.GetRetracement(points, i, 0, 1, 2)))
                {
                    return true;  
                    
                }
            }
            return false;
        }

        private bool FirstCheckFor3ExtensionPattern(List<ZigZagObject> points, int i, string pattern)
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

        public List<ZigZagObject> GetFibonacciSignalsQuantities(string patternName)
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
            return GetFibonacciSignalsQuantities(methodName).Where(x => x.Signal == true).Count();
        }
    }
}
