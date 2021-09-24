using System.Collections.Generic;
using Newtonsoft.Json;

namespace ITSoftStore.Models.Cart
{
    public class Item
    {
        [JsonProperty("feedId")]
        public int FeedId { get; set; }

        [JsonProperty("offerId")]
        public string OfferId { get; set; }

        [JsonProperty("feedCategoryId")]
        public string FeedCategoryId { get; set; }

        [JsonProperty("offerName")]
        public string OfferName { get; set; }

        [JsonProperty("subsidy")]
        public int Subsidy { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }

        [JsonProperty("fulfilmentShopId")]
        public int FulfilmentShopId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("warehouseId")]
        public int WarehouseId { get; set; }

        [JsonProperty("partnerWarehouseId")]
        public string PartnerWarehouseId { get; set; }
    }

    public class Parent4
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

    }

    public class Region
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Delivery
    {
        [JsonProperty("region")]
        public Region Region { get; set; }
    }

    public class Cart
    {
        [JsonProperty("businessId")]
        public int BusinessId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("delivery")]
        public Delivery Delivery { get; set; }
    }

    public class Root
    {
        [JsonProperty("cart")]
        public Cart Cart { get; set; }
    }

}
