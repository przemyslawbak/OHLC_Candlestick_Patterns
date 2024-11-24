using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal interface ISupportClass
    {
        bool CheckPoint(decimal point, decimal pointAD, decimal _priceMovement, bool checkPoint);
        decimal GetRetracement(List<ZigZagObject> points, int i, int number1, int number2, int number3);
        List<decimal> PointsRange(decimal point, decimal _priceMovement);
        List<ZigZagObject> AddPointsToList(List<ZigZagObject> points, int i, List<decimal> dateList, int number);
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

        public decimal GetRetracement(List<ZigZagObject> points, int i, int number1, int number2, int number3)
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

        public List<ZigZagObject> AddPointsToList(List<ZigZagObject> points, int i, List<decimal> dateList, int number)
        {
            for (int x = -number; x < 1; x++)
            {
                dateList.Add(points[i + x].Close);
                points[i].Signal = true;

            }
            return points;
        }
    }
}
