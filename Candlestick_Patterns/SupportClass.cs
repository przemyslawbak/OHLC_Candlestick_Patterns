using Candlestick_Patterns;
using System.Data;

namespace OHLC_Candlestick_Patterns
{
    internal interface ISupportClass
    {
        bool CheckPoint(decimal point, decimal pointAD, decimal _priceMovement, bool checkPoint);
        decimal GetRetracement(IReadOnlyList<ZigZagObject> points, int i, int number1, int number2, int number3);
        List<decimal> PointsRange(decimal point, decimal _priceMovement);
        bool CheckIfRetracemntIsInRange(List<decimal> range1, List<decimal> range2, decimal retracement);
    }

    internal class SupportClass : ISupportClass
    {

        public bool CheckPoint(decimal point, decimal pointAD, decimal _priceMovement, bool checkPoint)
        {
            if (PointsRange(pointAD, _priceMovement).First() >= point && PointsRange(pointAD, _priceMovement).Last() <= point)
            {
                checkPoint = true;
            }
            return checkPoint;
        }

        public  List<decimal> PointsRange(decimal point, decimal _priceMovement) => new List<decimal>()
        {
            point - (point * _priceMovement),
            point + (point * _priceMovement),
        };

        public decimal GetRetracement(IReadOnlyList<ZigZagObject> points, int i, int number1, int number2, int number3)
        {
            var retracement = (Math.Abs(points[i - number1].Close - points[i - number2].Close) * 100) / (Math.Abs(points[i - number2].Close - points[i - number3].Close));

            return retracement;
        }


        public bool CheckIfRetracemntIsInRange(List<decimal> range1, List<decimal> range2, decimal retracement)
        {
            if (range1.Min() < retracement && range2.Max() > retracement)
            {
                return true;
            }
            return false;
        }
    }
}
