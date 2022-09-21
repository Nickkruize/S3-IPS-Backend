using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S3_webshop.Resources;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace S3_webshop.Controllers
{
    [EnableCors("ClientPermission")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResource>>> Get()
        {
            try
            {
                IEnumerable<Category> categories = await _categoryService.GetAll();
                return _mapper.Map<List<Category>, List<CategoryResource>>(categories.ToList());
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryProductResource>> Get(int id)
        {
            try
            {
                Category category = await _categoryService.GetByIdWithProduct(id);

                if (category == null)
                {
                    return NotFound();
                }

                CategoryProductResource result = _mapper.Map<Category, CategoryProductResource>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCategoryResource vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                Category category = _mapper.Map<NewCategoryResource, Category>(vm);
                await _categoryService.AddCategory(category);
                return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryResource vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                Category category = await _categoryService.GetById(id);
                if (category == null)
                {
                    return BadRequest("This product can't be altered as it doesn't exist");
                }

                category.Name = vm.Name;
                category.ImgUrl = vm.ImgUrl;
                await _categoryService.Update(category);
                return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Category category = await _categoryService.GetById(id);
                
                if (category == null)
                {
                    return NotFound();
                }

                await _categoryService.Delete(category);
                return Accepted("category deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [NonAction]
        private bool CategoryExists(int id)
        {
            if (_categoryService.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
