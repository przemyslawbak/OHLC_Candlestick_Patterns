using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal class Fibonacci3DrivePattern: SupportClass
    {
        private decimal CalculateFibonacciPoint(List<ZigZagObject> points, int one, int two, int three, decimal fibonacci, int i, string trend)
        {
            var point = 0M;
            if (trend == "plus")
            {
                point = points[i - three].Close + Math.Abs(points[i - one].Close - points[i - two].Close) * fibonacci;
            }
            else if(trend == "minus")
            {
                point = points[i - three].Close - Math.Abs(points[i - one].Close - points[i - two].Close) * fibonacci ;
            }
            return point;
        }

        internal List<ZigZagObject> ThreeDrivePattern(string pattern, List<ZigZagObject> points, decimal _priceMovement)
        {
            bool checkD, checkC1, checkC2, checkB, checkA1, checkA2;
            decimal pointD, pointC1, pointC2, pointB, pointA1, pointA2;
            var dateList = new List<decimal>();

            for (int i = 5; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i - 5].IndexOHLCV))
                {
                    if (FirstCheckFor3DrivePattern(points, i, pattern))
                    {
                        (pointD, pointC1, pointC2, pointB, pointA1, pointA2) = ThreeDrivePoints(pattern, points, i);

                        checkD = CheckPoint(points[i].Close, pointD, _priceMovement, checkD = false);
                        checkC1 = CheckPoint(points[i - 1].Close, pointC1, _priceMovement, checkC1 = false);
                        checkC2 = CheckPoint(points[i - 1].Close, pointC2, _priceMovement, checkC2 = false);
                        checkB = CheckPoint(points[i - 2].Close, pointB, _priceMovement, checkB = false);
                        checkA1 = CheckPoint(points[i - 3].Close, pointA1, _priceMovement, checkA1 = false);
                        checkA2 = CheckPoint(points[i - 3].Close, pointA2, _priceMovement, checkA2 = false);
                        /*Fibonacci Extension: The C-D and E-F drives should ideally encompass 127.2% and 161.8% of the pullbacks observed during the B-C and D-E phases, respectively.
                        Fibonacci Retracement: The pullbacks observed during the B-C and D-E phases should ideally represent retracements of 61.8% or 78.6% relative to the preceding drive.*/

                        if (/*checkD && checkB && */ (checkC1 || checkC2) && ( checkA1 || checkA2))
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
            return points;
        }

        private (decimal d, decimal c1,decimal c2, decimal b, decimal a1, decimal a2) ThreeDrivePoints(string pattern, List<ZigZagObject> points, int i)
        {
            decimal pointD = 0M;
            decimal pointC1 = 0M;
            decimal pointC2 = 0M;
            decimal pointB = 0M;
            decimal pointA1 = 0M;
            decimal pointA2 = 0M;

            if (pattern == "bullish")
            {
                pointD = CalculateFibonacciPoint(points, 1, 2, 2, 1.272M, i,"minus");
                pointC1 = CalculateFibonacciPoint(points, 2, 3, 2, 0.618M, i,"plus");
                pointC2 = CalculateFibonacciPoint(points, 2, 3, 2, 0.786M, i,"plus");
                pointB = CalculateFibonacciPoint(points, 3, 4, 4, 1.272M, i, "minus");
                pointA1 = CalculateFibonacciPoint(points, 4, 5, 4, 0.618M, i, "plus");
                pointA2 = CalculateFibonacciPoint(points, 4, 5, 4, 0.786M, i, "plus");
            }

            if (pattern == "bearish")
            {
                pointD = CalculateFibonacciPoint(points, 1, 2, 1, 1.272M, i, "plus");
                pointC1 = CalculateFibonacciPoint(points, 2, 3, 3, 0.618M, i, "minus");
                pointC2 = CalculateFibonacciPoint(points, 2, 3, 3, 0.786M, i, "minus");
                pointB = CalculateFibonacciPoint(points, 3, 4, 3, 1.272M, i, "plus");
                pointA1 = CalculateFibonacciPoint(points, 4, 5, 5, 0.618M, i, "minus");
                pointA2 = CalculateFibonacciPoint(points, 4, 5, 5, 0.786M, i, "minus");
            }
            return (d: pointD, c1: pointC1, c2: pointC2, b: pointB, a1: pointA1, a2: pointA2);
        }

        private bool FirstCheckFor3DrivePattern(List<ZigZagObject> points, int i, string pattern)
        {
            if (pattern == "bullish")
            {
                if (points[i].Close < points[i - 1].Close && points[i - 1].Close > points[i - 2].Close && points[i - 2].Close < points[i - 3].Close && points[i - 3].Close > points[i - 4].Close && points[i - 4].Close < points[i - 5].Close)
                {
                    if (points[i - 1].Close < points[i - 3].Close && points[i - 3].Close < points[i - 5].Close  && points[i - 2].Close > points[i].Close && points[i - 2].Close < points[i - 4].Close)
                    { 
                        return true;
                    }
                }
            }

            if (pattern == "bearish")
            {
                if (points[i].Close > points[i - 1].Close && points[i - 1].Close < points[i - 2].Close && points[i - 2].Close > points[i - 3].Close && points[i - 3].Close < points[i - 4].Close && points[i - 4].Close > points[i - 5].Close)
                {
                    if (points[i - 1].Close > points[i - 3].Close && points[i - 3].Close > points[i - 5].Close  && points[i - 2].Close < points[i].Close && points[i - 2].Close > points[i - 4].Close)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
