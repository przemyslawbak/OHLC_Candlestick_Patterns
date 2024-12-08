using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal  class SetPeaksVallyes
    {
        static decimal _priceMovementTenthOfPercent =  0.002M; //default value
        
        internal static List<ZigZagObject> GetCloseAndSignalsData(List<OhlcvObject> data)
        {
            var dataToShapeZigZag = data.Select(x => new ZigZagObject()
            {
                Signal = x.Signal,
                Close = x.Close,
            }).ToList();

            return dataToShapeZigZag;
        }

        internal static List<ZigZagObject> GetPoints(List<decimal> _peaksFromZigZag)
        {
            var points  = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            return points;
        }


        internal static List<decimal> PeaksFromZigZag(List<ZigZagObject> _data, decimal zigZagParam)
        {
            _priceMovementTenthOfPercent = zigZagParam;
            var change = 0M;
            var zigZagList = new List<decimal>();
            var dataZigZag = _data.Select(x => new ZigZagObject() { Close = x.Close, Signal = false }).ToList();

            for (int i = 1; i < _data.Count; i++)
            {
                if (zigZagList.Count == 0)
                {
                    change = _data[0].Close * _priceMovementTenthOfPercent;
                    zigZagList.Add(_data[0].Close);
                }
                else
                {
                    change = zigZagList.Last() * _priceMovementTenthOfPercent;
                }

                var lastPoint = zigZagList.Last();
                if (_data[i].Close < (lastPoint - change) || _data[i].Close > (lastPoint + change))
                {
                    var point = _data[i].Close;
                    zigZagList.Add((point));
                    change = point * _priceMovementTenthOfPercent;
                }
            }

            return GetValleysAndPeaksFromZigZAg(zigZagList);
        }

        static List<decimal> GetValleysAndPeaksFromZigZAg(List<decimal> zigZagList)
        {
            var allPoints = new List<decimal>();

            bool directionUp = zigZagList[0] <= zigZagList[1];
            for (int i = 1; i < zigZagList.Count - 1; i++)
            {
                if (directionUp && zigZagList[i + 1] < zigZagList[i])
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = false;
                }
                else if (!directionUp && zigZagList[i + 1] > zigZagList[i])
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = true;
                }
            }

            return allPoints;
        }
    }
}
