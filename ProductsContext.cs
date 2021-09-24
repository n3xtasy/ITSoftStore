using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSoftStore
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
            try
            {
                //Database.EnsureDeleted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try
            {
                //Database.EnsureCreated();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        public DbSet<Product> Products { get; set; }
        public void AddProduct(Product product) => Products.Add(product);
        public Product GetProductAsync(string offerName)
        {
            var products = Products.Where(product => product.OfferName == offerName).OrderBy(products=> products.Id);

            Console.WriteLine($"Найдено: {products.Count()} продуктов {offerName}");

            foreach (var product in products)
            {
                if (product.Activated)
                {
                    Console.WriteLine($"Продукт: {product.Code} активирован");

                    continue;
                }

                product.Activated = true;

                return product;
            }

            return new Product()
            {
                Activated = true,
                ActivateTill = "1970-01-01",
                Code = "0000",
                OfferName = "Ошибка. Свяжитесь с администратором магазина",
                Slip = "0000",
            };
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public string Code { get; set; }
        public string Slip { get; set; }
        public string ActivateTill { get; set; }
        public bool Activated { get; set; }
    }
}
