using Newtonsoft.Json;

namespace Candlestick_Patterns
{
    public class ZigZagObject
    {
        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }
        public int IndexOHLCV { get; set; }
        // The earliest bar on which this pivot was confirmed by the reversal threshold.
        public int ConfirmationIndexOHLCV { get; set; }
        public bool Signal { get; set; } = false;
        public bool Initiation { get; set; } = false;
    }
}
