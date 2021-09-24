using Newtonsoft.Json;

namespace ITSoftStore.Models.Cart
{
    public class CartItem
    {
        [JsonProperty("feedId")]
        public int FeedId { get; set; }
        [JsonProperty("offerId")]
        public string OfferId { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
