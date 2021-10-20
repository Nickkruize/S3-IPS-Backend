using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using S3_webshop.Resources;

namespace S3_webshop.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper mapper;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductRepo productRepo, IMapper mapper)
        {
            _logger = logger;
            _productRepo = productRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProductResource> Get()
        {
            List<Product> products = _productRepo.FindAllWithProductCategories().ToList();
            return mapper.Map<List<Product>, List<ProductResource>>(products);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductWithCategoriesResource> Get(int id)
        {     
            if (_productRepo.GetById(id) == null)
            {
                return NotFound();
            }

            ProductWithCategoriesResource product = mapper.Map<Product, ProductWithCategoriesResource>(_productRepo.FindByIdWithCategoires(id));
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductResource product)
        {
            Product product1 = ModelConverter.ProductViewModelToProductContextModel(product);

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
        public IActionResult Post(ProductResource input)
        {
            Product product = new Product
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
        public IActionResult DeleteProduct(int id)
        {
            Product product = _productRepo.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _productRepo.Delete(product);
                _productRepo.Save();
                return Accepted("Product deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
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
