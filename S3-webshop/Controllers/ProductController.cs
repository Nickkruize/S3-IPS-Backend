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
        private readonly IProductRepo productRepo;
        private readonly IMapper mapper;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductRepo productRepo, IMapper mapper)
        {
            _logger = logger;
            this.productRepo = productRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProductResource> Get()
        {
            List<Product> products = productRepo.FindAllWithProductCategories().ToList();
            return mapper.Map<List<Product>, List<ProductResource>>(products);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductWithCategoriesResource> Get(int id)
        {     
            if (productRepo.GetById(id) == null)
            {
                return NotFound();
            }

            Product product = productRepo.FindByIdWithCategoires(id);
            ProductWithCategoriesResource result = mapper.Map<Product, ProductWithCategoriesResource>(product);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductResource product, int categoryId)
        {
            Product product1 = mapper.Map<ProductResource, Product>(product);

            if (id != product.Id)
            {
                return BadRequest();
            }

            productRepo.Update(product1);

            try
            {
                productRepo.Save();
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
                productRepo.AddProduct(product);
                productRepo.Save();

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
            Product product = productRepo.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            try
            {
                productRepo.Delete(product);
                productRepo.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        private bool ProductExists(int id)
        {
            if (productRepo.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
