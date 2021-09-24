using ITSoftStore.Models.Order;
using ITSoftStore.Modules;
using ITSoftStore.Modules.Yandex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;

namespace ITSoftStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        public OrderController(ProductsContext products)
        {
            ProductsContext = products;
        }
        private ProductsContext ProductsContext { get; set; }

        [HttpPost("accept")]
        public IActionResult Accept()
        {
            if (Request.Headers["Authorization"].FirstOrDefault() != "D60000017D7B3B7A")
            {
                return new Forbidden();
            }

            return Ok(new
            {
                order = new
                {
                    accepted = true,
                    shipmentDate = DateTime.Now.ToString("dd-MM-yyyy"),
                    id = new Random().Next(0, 99999).ToString(),
                }
            });
        }
        [HttpGet("new")]
        public IActionResult NewToken(string token, string clientId)
        {
            YandexModule.HttpClient.DefaultRequestHeaders.Clear();
            YandexModule.HttpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth oauth_token={token}, oauth_client_id={clientId}");

            return Ok($"OAuth oauth_token={token}, oauth_client_id={clientId}");
        }
        [HttpPost("status")]
        public async Task<IActionResult> Status([FromBody] Root root)
        {
            if (Request.Headers["Authorization"].FirstOrDefault() != "D60000017D7B3B7A")
            {
                return new Forbidden();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (root.Order.Status == "PROCESSING")
            {
                var listItems = new List<OrderLineItem>();

                foreach (var item in root.Order.Items)
                {
                    listItems.Add(new WooCommerceNET.WooCommerce.v2.OrderLineItem()
                    {
                        name = item.OfferName,
                        price = item.Price,
                        total = item.Price * item.Count,
                        sku = item.OfferId,
                        quantity = item.Count,
                    });
                }

                Console.WriteLine("Делаю запрос на Yandex.Market для отправки ключа...");

                var codes =await YandexModule.SendCode(root.Order, ProductsContext);

                Console.WriteLine($"Запрос успешно выполнен");

                string note = $"YandexID: {root.Order.Id}\n";

                foreach (var codeItem in codes)
                {
                    note += $"Ключ: {codeItem.Code}\n"; 
                }

                try
                {
                    if (root.Order.Buyer != null)
                    {
                        var order = await Commerce.wc.Order.Add(new WooCommerceNET.WooCommerce.v3.Order()
                        {
                            customer_note = note,
                            billing = new WooCommerceNET.WooCommerce.v2.OrderBilling()
                            {
                                email = "yandex@yandex.ru",
                                first_name = "Yandex: " + root.Order.Buyer.FirstName,
                                last_name = " " + root.Order.Buyer.LastName,
                                phone = root.Order.Buyer.Phone,
                            },
                            status = "completed",
                            line_items = listItems
                        });
                    }
                    else
                    {
                        var order = await Commerce.wc.Order.Add(new WooCommerceNET.WooCommerce.v3.Order()
                        {
                            customer_note = note,
                            billing = new WooCommerceNET.WooCommerce.v2.OrderBilling()
                            {
                                email = "yandex@yandex.ru",
                                first_name = "Yandex: Неавторизованный пользователь",
                                phone = "",
                            },
                            status = "completed",
                            line_items = listItems
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    await Commerce.wc.Order.Add(new WooCommerceNET.WooCommerce.v3.Order()
                    {
                        customer_note = note,
                        billing = new WooCommerceNET.WooCommerce.v2.OrderBilling()
                        {
                            email = "yandex@yandex.ru",
                            first_name = "Yandex",
                            last_name = "Market",
                            phone = root.Order.Buyer.Phone,
                        },
                        status = "completed",
                        line_items = listItems
                    });
                }
            }
            else if (root.Order.Status == "DELIVERED")
            {
                var parameters = new Dictionary<string, string>();

                parameters.Add("status", "completed");

                var orders = await Commerce.wc.Order.GetAll();

                try
                {
                    WooCommerceNET.WooCommerce.v3.Order order = orders.Where(o => o.customer_note.Split('\n').First() == "YandexID: " + root.Order.Id.ToString()).FirstOrDefault();

                    if (order != null)
                    {
                        order.billing.email = "yandex@yandex.ru";

                        order.status = "completed";

                        await Commerce.wc.Order.Update(Convert.ToUInt16(order.id), order);
                    }
                }
                catch
                { 
                    
                }
            }

            return Ok();
        }
    }
}

