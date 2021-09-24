using ITSoftStore.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ITSoftStore.Modules.Yandex
{
    public static class YandexModule
    {
        private static string AuthorizationToken { get; set; }
        public static HttpClient HttpClient { get; set; }
        public static void Initialize()
        {
            HttpClient = new HttpClient();

            HttpClient.DefaultRequestHeaders.Add("Authorization", "OAuth oauth_token=AQAAAABT02vKAAdbI1-eFux-ZkI8ll5Lj2eXfZw, oauth_client_id=5da1d4d2bb45404b9ab056849b686afb");
        }
        public static async Task<List<YandexItem>> SendCode(Order order, ProductsContext products)
        {
            var items = new List<YandexItem>();

            foreach (var item in order.Items)
            {
                Console.WriteLine($"Поиск ключа по OfferId: {item.OfferId}");

                for (int count = 0; count < item.Count; count += 1)
                {
                    var product = products.GetProductAsync(item.OfferId);

                    items.Add(new YandexItem()
                    {
                        Id = item.Id,
                        activate_till = product.ActivateTill,
                        Code = product.Code,
                        Slip = product.Slip
                    });

                    Console.WriteLine($"Отправляю код: ID: {item.Id} - Code: {product.Code} - Slip: {product.Slip}");

                    products.SaveChanges();

                    await Task.Delay(1000);
                }
            }

            

            var response = await HttpClient.PostAsJsonAsync($"https://api.partner.market.yandex.ru/v2/campaigns/22116971/orders/{order.Id}/deliverDigitalGoods.json", new
            {
                items = items
            });

            Console.WriteLine(response);

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode)
            {
                return new List<YandexItem>()
                {
                    new YandexItem()
                    {
                        Code = await response.Content.ReadAsStringAsync(),
                        Id = 0,
                        Slip = HttpClient.DefaultRequestHeaders.GetValues("Authorization").First(),
                        activate_till = "1970-01-01"
                    }
                };
            }

            return items;
        }

        public static bool IsValid(string token) => AuthorizationToken == token;
    }
}
