using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace S3_webshop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly List<Product> products = new List<Product>
            {
                new Product(1, "testProduct1"),
                new Product(2, "testProduct2"),
                new Product(3, "testProduct3"),
                new Product(4, "testProduct4"),
                new Product(5, "testProduct5")
            };

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return products;
        }

        public Product Get(int id)
        {
            return products.Find(item => item.Id == id);
        }
    }
}
