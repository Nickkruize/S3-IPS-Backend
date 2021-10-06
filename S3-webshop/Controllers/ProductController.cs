using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using S3_webshop.Interfaces;

namespace S3_webshop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly List<Product> products = new List<Product>
            {
                new Product(1, "testProduct1", "this is a test product", 9.99),
                new Product(2, "testProduct2", "this is a test product", 21.99),
                new Product(3, "testProduct3", "this is a test product", 10.50),
                new Product(4, "testProduct4", "this is a test product", 0.99),
                new Product(5, "testProduct5", "this is a test product", 100.99)
            };

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductRepo productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ModelConverter.ProductsContextModelsToProductViewModels(_productRepo.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            if (_productRepo.GetById(id) == null)
            {
                return NotFound();
            }

            Product product = ModelConverter.ProductContextModelToProductViewModel(_productRepo.GetById(id));
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            DAL.ContextModels.Product product1 = ModelConverter.ProductViewModelToProductContextModel(product);

            if (id != product.Id)
            {
                return BadRequest();
            }

            _productRepo.Update(product1);

            try
            {
                _productRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult PostProduct(Product input)
        {
            DAL.ContextModels.Product product = ModelConverter.ProductViewModelToProductContextModel(input);

            _productRepo.Create(product);
            _productRepo.Save();

            return CreatedAtAction("Get", new { id = input.Id }, input);
        }

        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id)
        {
            DAL.ContextModels.Product product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepo.Delete(product);
            _productRepo.Save();

            return ModelConverter.ProductContextModelToProductViewModel(product);
        }

        private bool ProductExists(int id)
        {
            if (_productRepo.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
