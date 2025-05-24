using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal class SetPeaksVallyes
    {
        static decimal _priceMovementTenthOfPercent = 0.002M; //default value
        
        internal static List<ZigZagObject> GetCloseAndSignalsData(List<OhlcvObject> data)
        {
            var dataToShapeZigZag = data.Select((x, index) => new ZigZagObject()
            {
                Signal = x.Signal,
                Close = x.Close,
                IndexOHLCV = index
            }).ToList();

            return dataToShapeZigZag;
        }

        internal static List<ZigZagObject> GetPoints(List<ZigZagObject> _peaksFromZigZag)
        {
            return _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x.Close, Signal = false, IndexOHLCV = x.IndexOHLCV }).ToList();
        }


        internal static List<ZigZagObject> PeaksFromZigZag(List<ZigZagObject> _data, decimal zigZagParam)
        {
            _priceMovementTenthOfPercent = zigZagParam;
            var change = 0M;
            var zigZagList = new List<ZigZagObject>();
            var dataZigZag = _data.Select(x => new ZigZagObject() { Close = x.Close, Signal = false }).ToList();

            for (int i = 0; i < _data.Count; i++)
            {
                if (zigZagList.Count == 0)
                {
                    zigZagList.Add(new ZigZagObject() { Close = _data[0].Close, Signal = false, IndexOHLCV = _data[0].IndexOHLCV });
                }
                else
                {
                    change = zigZagList.Last().Close * _priceMovementTenthOfPercent;
                }

                var lastPoint = zigZagList.Last();
                if ((_data[i].Close < (lastPoint.Close - change) || _data[i].Close > (lastPoint.Close + change)) && i != 0)
                {
                    var point = _data[i].Close;
                    zigZagList.Add((new ZigZagObject() { Close = point, Signal = false, IndexOHLCV = _data[i].IndexOHLCV }));
                    change = point * _priceMovementTenthOfPercent;
                }
            }

            return GetValleysAndPeaksFromZigZAg(zigZagList);
        }

        static List<ZigZagObject> GetValleysAndPeaksFromZigZAg(List<ZigZagObject> zigZagList)
        {
            var allPoints = new List<ZigZagObject>();
            if (zigZagList.Count < 2)
                return new();
            bool directionUp = zigZagList[0].Close <= zigZagList[1].Close;
            for (int i = 0; i < zigZagList.Count - 1; i++)
            {
                if (directionUp && zigZagList[i + 1].Close < zigZagList[i].Close)
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = false;
                }
                else if (!directionUp && zigZagList[i + 1].Close > zigZagList[i].Close)
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = true;
                }
            }

            return allPoints;
        }
    }
}
