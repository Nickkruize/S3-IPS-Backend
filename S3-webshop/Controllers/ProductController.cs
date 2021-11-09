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
using Repositories.Interfaces;
using S3_webshop.Resources;
using Services.Interfaces;

namespace S3_webshop.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IMapper mapper)
        {
            _logger = logger;
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProductResource> Get()
        {
            List<Product> products = productService.GetAllWithCategories().ToList();
            return mapper.Map<List<Product>, List<ProductResource>>(products);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductWithCategoriesResource> Get(int id)
        {     
            if (productService.GetById(id) == null)
            {
                return NotFound();
            }

            Product product = productService.GetByIdWithCategories(id);
            ProductWithCategoriesResource result = mapper.Map<Product, ProductWithCategoriesResource>(product);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductResource product, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Product product1 = mapper.Map<ProductResource, Product>(product);

            if (id != product.Id)
            {
                return BadRequest();
            }

            productService.Update(product1);

            try
            {
                productService.Save();
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
        public IActionResult Post(NewProductResource input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Product product = mapper.Map<NewProductResource, Product>(input);

            try
            {
                productService.AddProduct(product);
                productService.Save();

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
            Product product = productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            try
            {
                productService.Delete(product);
                productService.Save();
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
            if (productService.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
