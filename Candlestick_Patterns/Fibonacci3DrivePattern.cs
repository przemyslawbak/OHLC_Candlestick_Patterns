using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal class Fibonacci3DrivePattern: SupportClass
    {
        private decimal CalculateFibonacciPoint(List<ZigZagObject> points, int one, int two, int three, decimal fibonacci, int i)
        {
            var point = (Math.Abs(points[i - one].Close - points[i - two].Close) * fibonacci) + points[i - three].Close;
            return point;
        }

        internal List<ZigZagObject> ThreeDrivePattern(string pattern, List<ZigZagObject> points, decimal _priceMovement)
        {
            bool checkD, checkC, checkB, checkA;
            decimal pointD, pointC, pointB, pointA;
            var dateList = new List<decimal>();

            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (FirstCheckFor3DrivePattern(points, i, pattern))
                    {
                        (pointD, pointC, pointB, pointA) = ThreeDrivePoints(pattern, points, i);

                        checkD = CheckPoint(points[i].Close, pointD, _priceMovement, checkD = false);
                        checkC = CheckPoint(points[i - 1].Close, pointC, _priceMovement, checkC = false);
                        checkB = CheckPoint(points[i - 2].Close, pointB, _priceMovement, checkB = false);
                        checkA = CheckPoint(points[i - 3].Close, pointA, _priceMovement, checkA = false);

                        if (checkD = true && checkC && checkB && checkA)
                        {
                            for (int x = -5; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            points[i].Signal = true;
                        }
                    }

                }
            }
            return points;
        }

        private (decimal d, decimal c, decimal b, decimal a) ThreeDrivePoints(string pattern, List<ZigZagObject> points, int i)
        {
            decimal pointD = 0M;
            decimal pointC = 0M;
            decimal pointB = 0M;
            decimal pointA = 0M;

            if (pattern == "bullish")
            {
                pointD = CalculateFibonacciPoint(points, 1, 2, 2, 1.272M, i);
                pointC = CalculateFibonacciPoint(points, 2, 3, 2, 0.618M, i);
                pointB = CalculateFibonacciPoint(points, 3, 4, 4, 1.272M, i);
                pointA = CalculateFibonacciPoint(points, 4, 5, 4, 0.618M, i);
            }

            if (pattern == "bearish")
            {
                pointD = CalculateFibonacciPoint(points, 1, 2, 1, 1.272M, i);
                pointC = CalculateFibonacciPoint(points, 2, 3, 3, 0.618M, i);
                pointB = CalculateFibonacciPoint(points, 3, 4, 3, 1.272M, i);
                pointA = CalculateFibonacciPoint(points, 4, 5, 5, 0.618M, i);
            }
            return (d: pointD, c: pointC, b: pointB, a: pointA);
        }

        private bool FirstCheckFor3DrivePattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close && points[i - 4].Close < points[i - 5].Close)
                {
                    return true;
                }
            }

            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 4].Close > points[i - 5].Close)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
