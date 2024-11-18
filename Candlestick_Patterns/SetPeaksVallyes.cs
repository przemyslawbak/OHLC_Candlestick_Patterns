using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal  class SetPeaksVallyes
    {
        static decimal _priceMovement { get { return 0.002M; }}

        internal static List<ZigZagObject> GetCloseAndSignalsData(List<OhlcvObject> data)
        {
            var dataToShapeZigZag = data.Select(x => new ZigZagObject()
            {
                Signal = x.Signal,
                Close = x.Close,
            }).Reverse().ToList();

            return dataToShapeZigZag;
        }

        internal static List<decimal> PeaksFromZigZag(List<ZigZagObject> _data)
        {
            var change = 0M;
            var zigZagList = new List<decimal>();
            var dataZigZag = _data.Select(x => new ZigZagObject() { Close = x.Close, Signal = false }).ToList();

            for (int i = 1; i < _data.Count; i++)
            {
                if (zigZagList.Count == 0)
                {
                    change = _data[0].Close * _priceMovement;
                    zigZagList.Add(_data[0].Close);
                }
                else
                {
                    change = zigZagList.Last() * _priceMovement;
                }

                var lastPoint = zigZagList.Last();
                if (_data[i].Close < (lastPoint - change) || _data[i].Close > (lastPoint + change))
                {
                    var point = _data[i].Close;
                    zigZagList.Add((point));
                    change = point * _priceMovement;
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
