using Newtonsoft.Json;

namespace Candlestick_Patterns
{
    public class OhlcvsObject
    {
        [JsonProperty(PropertyName = "open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "high")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        public bool Signal { get; set; } = false;
    }
}
