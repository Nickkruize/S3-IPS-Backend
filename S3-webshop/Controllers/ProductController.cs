﻿using AutoMapper;
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
        public async Task<ActionResult<IEnumerable<ProductWithCategoriesResource>>> Get()
        {
            try
            {
                IEnumerable<Product> products = await _productService.GetAllWithCategories();
                return Ok(_mapper.Map<List<Product>, List<ProductWithCategoriesResource>>(products.ToList()));
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
        public async Task<IActionResult> Put(int id, ProductWithCategoriesResource product, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Information");
            }

            Product updatedProduct = _mapper.Map<ProductWithCategoriesResource, Product>(product);

            if (id != product.Id)
            {
                return BadRequest("submitted Id doesn't match the productId");
            }

            try
            {
                await _productService.Update(updatedProduct, categoryId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (await _productService.GetById(id) == null)
                {
                    return NotFound("This product doesn't exist");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return CreatedAtAction(nameof(Get), new { id = updatedProduct.Id }, updatedProduct);
        }

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
                    return StatusCode(500, ex.Message);
                }
            }

            return BadRequest("Invalid information");
        }

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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
