using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using S3_webshop.Resources;
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
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResource>>> Get()
        {
            try
            {
                IEnumerable<Category> categories = await _categoryRepo.FindAll();
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
                if (await _categoryRepo.GetById(id) == null)
                {
                    return NotFound();
                }

                Category category = await _categoryRepo.FindByIdWithProducts(id);

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
                await _categoryRepo.Create(category);
                await _categoryRepo.Save();
                return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryResource vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Category category = _mapper.Map<CategoryResource, Category>(vm);

            if (id != vm.Id)
            {
                return BadRequest();
            }

            try
            {
                _categoryRepo.Update(category);
                await _categoryRepo.Save();
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

            return CreatedAtAction("Get", new { id }, category);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Category category = await _categoryRepo.GetById(id);
                
                if (category == null)
                {
                    return NotFound();
                }

                _categoryRepo.Delete(category);
                await _categoryRepo.Save();
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
            if (_categoryRepo.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
