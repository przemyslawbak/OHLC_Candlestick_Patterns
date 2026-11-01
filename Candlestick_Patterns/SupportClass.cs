using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal interface ISupportClass
    {
        bool CheckPoint(decimal point, decimal pointAD, decimal priceMovement, bool checkPoint);
        decimal GetRetracement(IReadOnlyList<ZigZagObject> points, int i, int number1, int number2, int number3);
        List<decimal> PointsRange(decimal point, decimal priceMovement);
        bool CheckIfRetracemntIsInRange(List<decimal> range1, List<decimal> range2, decimal retracement);
    }

    internal class SupportClass : ISupportClass
    {
        public bool CheckPoint(decimal point, decimal pointAD, decimal priceMovement, bool checkPoint)
        {
            var range = PointsRange(pointAD, priceMovement);
            return checkPoint || (point >= range[0] && point <= range[1]);
        }

        public List<decimal> PointsRange(decimal point, decimal priceMovement)
        {
            if (priceMovement < 0M)
                throw new ArgumentOutOfRangeException(nameof(priceMovement));
            decimal tolerance = Math.Abs(point) * priceMovement;
            return new List<decimal> { point - tolerance, point + tolerance };
        }

        public decimal GetRetracement(IReadOnlyList<ZigZagObject> points, int i, int number1, int number2, int number3)
        {
            decimal numerator = Math.Abs(points[i - number1].Close - points[i - number2].Close);
            decimal denominator = Math.Abs(points[i - number2].Close - points[i - number3].Close);
            // A zero reference leg cannot define a Fibonacci ratio.
            return denominator == 0M ? 0M : numerator * 100M / denominator;
        }

        public bool CheckIfRetracemntIsInRange(List<decimal> range1, List<decimal> range2, decimal retracement)
        {
            return retracement >= Math.Min(range1[0], range2[0])
                && retracement <= Math.Max(range1[1], range2[1]);
        }
    }
}
