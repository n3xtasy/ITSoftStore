using ITSoftStore.Models.Stocks;
using ITSoftStore.Models.Stocks.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSoftStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        public StocksController(ProductsContext productsContext)
        {
            ProductsContext = productsContext;
        }
        private ProductsContext ProductsContext { get; set; }
        [HttpPost]
        public async Task<IActionResult> Stocks([FromBody] Root root)
        {
            if (Request.Headers["Authorization"].FirstOrDefault() != "D60000017D7B3B7A")
            {
                return new Forbidden();
            }

            Console.WriteLine(JsonConvert.SerializeObject(root));

            var response = new Models.Stocks.Response.Root();

            response.Skus = new List<Models.Stocks.Response.SkuItem>();

            foreach (var skuItem in root.Skus)
            {
                response.Skus.Add(new Models.Stocks.Response.SkuItem()
                {
                    Sku = skuItem,
                    WarehouseId = root.WarehouseId,
                    Items = new List<Models.Stocks.Response.Item>()
                });

                var item = new Models.Stocks.Response.Item();

                try
                {
                    item.Count = ProductsContext.Products.Where(p => p.OfferName.Contains(skuItem) && !p.Activated).Count();
                }
                catch (Exception e)
                {
                    item.Count = 1;

                    Console.WriteLine(e.Message);
                }

                item.Type = "FIT";
                item.UpdatedAt = DateTime.Now;

                response.Skus.Last().Items.Add(item);
            }

            Console.WriteLine(JsonConvert.SerializeObject(response));

            //await Task.Delay(5000);

            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
