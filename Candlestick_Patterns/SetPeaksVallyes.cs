using Candlestick_Patterns;

namespace OHLC_Candlestick_Patterns
{
    internal class SetPeaksVallyes
    {
        internal static List<ZigZagObject> GetCloseAndSignalsData(List<OhlcvObject> data)
        {
            ArgumentNullException.ThrowIfNull(data);
            return data.Select((candle, index) => new ZigZagObject
            {
                Close = candle.Close,
                Signal = candle.Signal,
                IndexOHLCV = index,
                ConfirmationIndexOHLCV = index
            }).ToList();
        }

        internal static List<ZigZagObject> GetPoints(List<ZigZagObject> peaks)
        {
            ArgumentNullException.ThrowIfNull(peaks);
            return peaks.Select(point => new ZigZagObject
            {
                Close = point.Close,
                IndexOHLCV = point.IndexOHLCV,
                ConfirmationIndexOHLCV = point.ConfirmationIndexOHLCV,
                Signal = false,
                Initiation = false
            }).ToList();
        }

        internal static List<ZigZagObject> PeaksFromZigZag(List<ZigZagObject> data, decimal zigZagParam)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (zigZagParam <= 0M || zigZagParam >= 1M)
                throw new ArgumentOutOfRangeException(nameof(zigZagParam),
                    "The reversal threshold must be a decimal fraction between zero and one.");

            var pivots = new List<ZigZagObject>();
            if (data.Count == 0)
                return pivots;
            if (data[0].Close <= 0M)
                throw new ArgumentOutOfRangeException(nameof(data), "ZigZag requires positive closing prices.");
            if (data.Count < 2)
                return pivots;

            // A pivot is confirmed only after a reversal of at least the given fraction.
            // The unfinished final leg is intentionally excluded to avoid future leakage.
            int direction = 0;
            int highestIndex = 0;
            int lowestIndex = 0;
            int candidateIndex = 0;
            decimal highest = data[0].Close;
            decimal lowest = data[0].Close;
            decimal candidatePrice = data[0].Close;

            for (int i = 1; i < data.Count; i++)
            {
                decimal price = data[i].Close;
                if (price <= 0M)
                    throw new ArgumentOutOfRangeException(nameof(data), "ZigZag requires positive closing prices.");

                if (direction == 0)
                {
                    if (price > highest) { highest = price; highestIndex = i; }
                    if (price < lowest) { lowest = price; lowestIndex = i; }

                    if (price >= lowest * (1M + zigZagParam))
                    {
                        pivots.Add(CopyPivot(data[lowestIndex], i));
                        direction = 1;
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                    else if (price <= highest * (1M - zigZagParam))
                    {
                        pivots.Add(CopyPivot(data[highestIndex], i));
                        direction = -1;
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                    continue;
                }

                if (direction > 0)
                {
                    if (price >= candidatePrice)
                    {
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                    else if (price <= candidatePrice * (1M - zigZagParam))
                    {
                        pivots.Add(CopyPivot(data[candidateIndex], i));
                        direction = -1;
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                }
                else
                {
                    if (price <= candidatePrice)
                    {
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                    else if (price >= candidatePrice * (1M + zigZagParam))
                    {
                        pivots.Add(CopyPivot(data[candidateIndex], i));
                        direction = 1;
                        candidatePrice = price;
                        candidateIndex = i;
                    }
                }
            }

            return pivots;
        }

        private static ZigZagObject CopyPivot(ZigZagObject point, int confirmationIndex) => new()
        {
            Close = point.Close,
            IndexOHLCV = point.IndexOHLCV,
            ConfirmationIndexOHLCV = confirmationIndex,
            Signal = false,
            Initiation = false
        };
    }
}
