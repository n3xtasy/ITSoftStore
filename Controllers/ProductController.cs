using ITSoftStore.Modules.Yandex;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSoftStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController(ProductsContext productsContext)
        {
            ProductsContext = productsContext;
        }

        private ProductsContext ProductsContext { get; set; }

        [HttpPost("create")]
        public IActionResult Create(ProductItem productItem)
        {


            Console.WriteLine($"Создаю продукт: {productItem.code} - {productItem.offerName}");

            ProductsContext.AddProduct(new Product()
            {
                OfferName = productItem.offerName,
                ActivateTill = productItem.activateTill,
                Code = productItem.code,
                Slip = productItem.slip
            });

            Console.WriteLine($"Продукт: {productItem.code} - {productItem.offerName} добавлен в базу данных");

            Console.WriteLine($"Сохраняю изменения");

            ProductsContext.SaveChanges();

            Console.WriteLine($"Сохранено");

            return Ok(ProductsContext.Products);
        }
        [HttpGet("product")]
        public IActionResult GetProductByOfferName(string offerName)
        {
            var product = ProductsContext.GetProductAsync(offerName);

            return Ok(product);
        }
    }

    public class ProductItem
    { 
        public string offerName { get; set; }
        public string code { get; set; }
        public string slip { get; set; }
        public string activateTill { get; set; }
    }
}
