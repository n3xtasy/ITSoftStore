using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSoftStore.Modules.Yandex
{
    public class YandexItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("slip")]
        public string Slip { get; set; }
        [JsonProperty("activate_till")]
        public string activate_till { get; set; }
    }
}
