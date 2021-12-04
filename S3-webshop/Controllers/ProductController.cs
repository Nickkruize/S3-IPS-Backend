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
    [EnableCors("ClientPermission")]
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
        public async Task<ActionResult<IEnumerable<ProductResource>>> Get()
        {
            try
            {
                IEnumerable<Product> products = await productService.GetAllWithCategories();
                return mapper.Map<List<Product>, List<ProductResource>>(products.ToList());
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWithCategoriesResource>> Get(int id)
        {
            try
            {
                if (await productService.GetById(id) == null)
                {
                    return NotFound();
                }

                Product product = await productService.GetByIdWithCategories(id);
                ProductWithCategoriesResource result = mapper.Map<Product, ProductWithCategoriesResource>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductResource product, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Product updatedProduct = mapper.Map<ProductResource, Product>(product);

            if (id != product.Id)
            {
                return BadRequest("submitted Id doesn't match productId");
            }

            try
            {
                await productService.Update(updatedProduct, categoryId);
                await productService.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return CreatedAtAction(nameof(Get), new { id = updatedProduct.Id }, updatedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Post(NewProductResource input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid information");
            }

            try
            {
                Product product = mapper.Map<NewProductResource, Product>(input);

                await productService.AddProduct(product);
                await productService.Save();

                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                Product product = await productService.GetById(id);
                
                if (product == null)
                {
                    return NotFound();
                }
                
                productService.Delete(product);
                await productService.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
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
