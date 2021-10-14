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

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductRepo productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            List<DAL.ContextModels.Product> products = _productRepo.FindAllWithProductCategories().ToList();
            return ModelConverter.ProductsContextModelsToProductViewModels(products);
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
        public async Task<IActionResult> Put(int id, Product product)
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
                    return BadRequest("Generic Error");
                }
            }

            return CreatedAtAction("Get", new { id = id }, id);
        }

        [HttpPost]
        public IActionResult Post(Product input)
        {
            //DAL.ContextModels.Product product = ModelConverter.ProductViewModelToProductContextModel(input);
            DAL.ContextModels.Product product = new DAL.ContextModels.Product
            {
                Description = input.Description,
                Name = input.Name,
                Price = input.Price
            };

            try
            {
                _productRepo.AddProduct(product);
                _productRepo.Save();

                return CreatedAtAction("Get", new { id = product.Id }, product);
            }
            catch
            {
                return BadRequest();
            }
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
