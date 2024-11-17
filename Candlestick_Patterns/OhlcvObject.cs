using Newtonsoft.Json;

namespace Candlestick_Patterns
{
    public class OhlcvObject
    {
        [JsonProperty(PropertyName = "Open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "High")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "Low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "Close")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "Volume")]
        public decimal Volume { get; set; }
        public bool Signal { get; set; } = false;
    }
}
