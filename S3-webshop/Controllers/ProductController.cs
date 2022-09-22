using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using S3_webshop.Resources;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Controllers
{
    [EnableCors("ClientPermission")]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public ProductController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWithCategoriesResource>>> Get()
        {
            try
            {
                IEnumerable<Product> products = await _productService.GetAllWithCategories();
                return Ok(_mapper.Map<List<Product>, List<ProductWithCategoriesResource>>(products.ToList()));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(500, ex.InnerException.Message);
                }

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWithCategoriesResource>> Get(int id)
        {
            try
            {
                Product product = await _productService.GetByIdWithCategories(id);
                if (product == null)
                {
                    return NotFound();
                }
                ProductWithCategoriesResource result = _mapper.Map<Product, ProductWithCategoriesResource>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(500, ex.InnerException.Message);
                }

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductWithCategoriesResource product, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Information");
            }

            Product currentProduct = await _productService.GetByIdWithCategories(id);
            Category category = await _categoryService.GetById(categoryId);

            if (currentProduct == null)
            {
                return BadRequest("Product does not exist");
            }

            if (category == null)
            {
                return BadRequest("This category does not exist");
            }

            if (currentProduct.Categories != null && currentProduct.Categories.Contains(category))
            {
                return BadRequest("This product is already assigned to this category");
            }

            try
            {
                var result = await _productService.Update(currentProduct, categoryId);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(500, ex.InnerException.Message);
                }

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewProductResource input)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = _mapper.Map<NewProductResource, Product>(input);

                    product = await _productService.AppendCategoriesToProduct(input.CategoryIds, product);

                    if (!_productService.VerifyAllSubmittedCategoriesWhereFound(product, input.CategoryIds))
                    {
                        return BadRequest("One or more invalid CategoryIds");
                    }

                    await _productService.AddProduct(product);

                    return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return StatusCode(500, ex.InnerException.Message);
                    }

                    return StatusCode(500, ex.Message);
                }
            }

            return BadRequest("Invalid information");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                Product product = await _productService.GetById(id);
                
                if (product == null)
                {
                    return NotFound("Product was not found");
                }
                
                await _productService.Delete(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(500, ex.InnerException.Message);
                }

                return StatusCode(500, ex.Message);
            }
        }
    }
}
