using Newtonsoft.Json;


namespace Candlestick_Patterns
{
    public class ZigZagObject
    {
        //[JsonProperty(PropertyName = "date")]
        //public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }

        public bool Signal { get; set; } = false;
    }
}
