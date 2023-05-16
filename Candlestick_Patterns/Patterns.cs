using System;

namespace Candlestick_Patterns
{
    public class Patterns
    {
        decimal _minCandleSize = 0.5M;

        public List<OhlcvObject> AddSignals(List<OhlcvObject> data)
        {
            /*Rules:
            1.The first candlestick is a long green candlestick.
            2.The second candlestick is a red candlestick which gaps up.
            3.The third candlestick is a red candlestick which opens in the body range of the second candlestick and closes in the body range of the first candlestick.

            Pattern Type:
            Bearish Reversal.*/

            /*var m = Enumerable.Range(1, data.Count - 1)
                  .Where(i => data[i - 2].Open < data[i - 2].Close && (data[i - 2].Close - data[i - 2].Open) / data[i - 2].Open > _minCandleSize)
                  .Where(i => data[i - 1].Open > data[i - 1].Close && data[i - 1].Low > data[i - 2].High)
                  .Where(i => data[i].Open > data[i].Close && data[i - 1].Close <= data[i].Open && data[i].Open <= data[i - 1].Open && data[i - 2].Open <= data[i].Close && data[i].Close <= data[i - 2].Close)
                  .Count();*/

            var m = Enumerable.Range(2, data.Count - 1)
                  .Select(i => new OhlcvObject() 
                  { 
                      Open = data[i].Open,
                      High= data[i].High,
                      Low= data[i].Low,
                      Close = data[i].Close, 
                      Signal = (data[i - 2].Open < data[i - 2].Close) ? true : false
                  })
                  .ToList();

            /*for (var i = 0; i < data.Count; i++)
            {
                // Check whether the first candlestick matches. 
                if (DataOpen(2) < DataClose(2) && 100 * (DataClose(2) - DataOpen(2)) / DataOpen(2) > _minCandleSize)
                {
                    // Check whether the second candlestick matches. 
                    if (DataOpen(1) > DataClose(1) && DataLow(1) > DataHigh(2))
                    {
                        // Check whether the third candlestick matches. 
                        if (DataOpen(0) > DataClose(0) && DataClose(1) <= DataOpen(0) && DataOpen(0) <= DataOpen(1) && DataOpen(2) <= DataClose(0) && DataClose(0) <= DataClose(2))
                        {
                            return 3;
                        }
                    }
                }
                return 0;
            }*/

            return data;
        }
    }
}
