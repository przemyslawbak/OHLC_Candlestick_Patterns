using OHLC_Candlestick_Patterns;
using System.Linq;
using System.Reflection;

namespace Candlestick_Patterns
{
    public class Fibonacci : IFibonacci
    {
        private static List<ZigZagObject> _data { get; set; }

        private static decimal _priceMovement1 { get; set; }

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
                _priceMovement1 = 0.02M; // change
                _drivePattern = new Fibonacci3DrivePattern();
                _support = new SupportClass();
            }
        }

        private List<ZigZagObject> BearishABCD() => abcdPattern("bearish", _points, _priceMovement1);
        private List<ZigZagObject> BullishABCD() => abcdPattern("bullish", _points, _priceMovement1);
        private List<ZigZagObject> Bearish3Extension() => ThreeExtensionPattern("bearish", _points, _priceMovement1);

        private List<ZigZagObject> Bullish3Extension() => ThreeExtensionPattern("bullish", _points, _priceMovement1);

        private List<ZigZagObject> Bearish3Retracement() => ThreeRetracementPattern("bearish", _points, _priceMovement1);

        private List<ZigZagObject> Bullish3Retracement() => ThreeRetracementPattern("bullish", _points, _priceMovement1);

        private List<ZigZagObject> Bearish3Drive() => _drivePattern.ThreeDrivePattern("bearish", _points, _priceMovement1);

        private List<ZigZagObject> Bullish3Drive() => _drivePattern.ThreeDrivePattern("bullish", _points, _priceMovement1);

        private List<ZigZagObject> abcdPattern(string pattern, List<ZigZagObject> points, decimal priceMovement1)
        {
            var dateList = new List<decimal>();

            for (int i = 3; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (FirstCheckForABCDPattern(points, i, pattern))
                    {
                        if (SecondCheckForABCDPattern(points, i, pattern, dateList))
                        {
                            return _support.AddPointsToList(points, i, dateList, -3);
                        }
                    }
                }
            }
            return points;
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

            if (diffLenght / ((lenghtba + lenghtdc)/2) <= _priceMovement1)  // ustalić poziom różnicy, gdy ma być zbliżonae do zera
            {
                var retracementBcBa = _support.GetRetracement(points, i, 1, 2, 3); // 61,8% lub 78,6%
                var retracementCdCb = _support.GetRetracement(points, i, 0, 1, 2); // 127,2% lub 161,8%
                if ((_support.PointsRange(61.8M, _priceMovement1).Contains(retracementBcBa) || _support.PointsRange(78.6M, _priceMovement1).Contains(retracementBcBa)) &&
                   (_support.PointsRange(127.2M, _priceMovement1).Contains(retracementCdCb) || _support.PointsRange(161.8M, _priceMovement1).Contains(retracementCdCb)))
                {
                    return true;
                } 

            }
            if (lenghtdc > lenghtba && pattern == "bullish")
            {
                var retracementBullishExtensionDcBa = _support.GetRetracement(points, i, 1, 2, 3); // 161,8% lub 127,2%
                if (_support.PointsRange(161.8M, _priceMovement1).Contains(retracementBullishExtensionDcBa) || _support.PointsRange(127.2M, _priceMovement1).Contains(retracementBullishExtensionDcBa))
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

        private List<ZigZagObject> ThreeRetracementPattern(string pattern, List<ZigZagObject> points, decimal priceMovement1)
        {
            var dateList = new List<decimal>();

            for (int i = 2; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (FirstCheckFor3RetracementPattern(points, i, pattern))
                    {
                        if (_support.PointsRange(61.8M, _priceMovement1).Contains(_support.GetRetracement(points, i, 0, 1, 2))) // dla obu 
                        {
                            return _support.AddPointsToList(points, i, dateList, -2);
                        }
                    }
                }
            }
            return points;
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

        private List<ZigZagObject> ThreeExtensionPattern(string pattern, List<ZigZagObject> points, decimal priceMovement1)
        {
            var dateList = new List<decimal>();

            for (int i = 2; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (FirstCheckFor3ExtensionPattern(points, i, pattern))
                    {
                        if ( SecondCheckFor2ExtensionPattern(points,i, pattern, dateList))
                        {
                            return _support.AddPointsToList(points, i, dateList, -2);
                        }
                    }
                }
            }
            return points;
        }

        private bool SecondCheckFor2ExtensionPattern(List<ZigZagObject> points, int i, string pattern, List<decimal> dateList)
        {
            if (pattern == "bullish")
            {
                if (_support.PointsRange(61.8M, _priceMovement1).Contains(_support.GetRetracement(points, i, 0, 1, 2)))
                {
                    return true; 
                }
            }

            if (pattern == "bearish")
            {
                if (_support.PointsRange(161.8M, _priceMovement1).Contains(_support.GetRetracement(points, i, 0, 1, 2)))
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
