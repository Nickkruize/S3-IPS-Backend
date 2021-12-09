using AutoMapper;
using DAL.ContextModels;
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
        private readonly IMapper _mapper;


        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWithCategoryResource>>> Get()
        {
            try
            {
                IEnumerable<Product> products = await _productService.GetAllWithCategories();
                return Ok(_mapper.Map<List<Product>, List<ProductWithCategoryResource>>(products.ToList()));
            }
            catch(Exception ex)
            {
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductWithCategoryResource product, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Product updatedProduct = _mapper.Map<ProductWithCategoryResource, Product>(product);

            if (id != product.Id)
            {
                return BadRequest("submitted Id doesn't match productId");
            }

            try
            {
                await _productService.Update(updatedProduct, categoryId);
                await _productService.Save();
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
                Product product = _mapper.Map<NewProductResource, Product>(input);

                product = await _productService.AppendCategoriesToProduct(input.CategoryIds, product);

                if (!_productService.VerifyAllSubmittedCategoriesWhereFound(product, input.CategoryIds))
                {
                    return BadRequest("One or more invalid CategoryIds");
                }

                await _productService.AddProduct(product);
                await _productService.Save();

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
                Product product = await _productService.GetById(id);
                
                if (product == null)
                {
                    return NotFound();
                }
                
                _productService.Delete(product);
                await _productService.Save();
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
            if (_productService.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
