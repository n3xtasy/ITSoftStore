using ITSoftStore.Models.Cart;
using ITSoftStore.Modules.Yandex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSoftStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        public CartController(ProductsContext products)
        {
            ProductsContext = products;
        }
        private ProductsContext ProductsContext { get; set; }

        [HttpPost]
        public IActionResult Post( Models.Cart.Root root)
        {
            if (Request.Headers["Authorization"].FirstOrDefault() != "D60000017D7B3B7A")
            {
                return new Forbidden();
            }

            List<CartItem> cartItems = new List<CartItem>();

            foreach (var item in root.Cart.Items)
            {
                cartItems.Add(new CartItem()
                {
                    FeedId = item.FeedId,
                    Count = ProductsContext.Products.Where(p => p.OfferName == item.OfferId && !p.Activated).Count(),
                    OfferId = item.OfferId
                });
            }

            var response = new
            {
                cart = new
                {
                    deliveryCurrency = "RUR",
                    deliveryOptions = new[] {
                        new {
                            price = 0,
                            serviceName = "Доставка на электронную почту",
                            type = "DIGITAL",
                            dates = new {
                                fromDate = DateTime.Now.ToString("dd-MM-yyyy")
                            }
                        }
                    },
                    address = new[] {
                        ""
                    },
                    items = cartItems,
                    paymentMethods = new[] {
                        "YANDEX", "APPLE_PAY", "GOOGLE_PAY"
                    }
                }
            };

            return Ok(response);
        }
    }
}
