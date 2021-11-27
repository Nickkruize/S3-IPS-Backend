using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using S3_webshop.Resources;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace S3_webshop.Controllers
{
    [EnableCors("ClientPermission")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepo categoryRepo, IMapper mapper)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<CategoryResource> Get()
        {
            List<Category> categories = categoryRepo.FindAll().ToList();
            return mapper.Map<List<Category>, List<CategoryResource>>(categories);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public ActionResult<CategoryProductResource> Get(int id)
        {
            if (categoryRepo.GetById(id) == null)
            {
                return NotFound();
            }

            Category category = categoryRepo.FindByIdWithProducts(id);

            CategoryProductResource result = mapper.Map<Category, CategoryProductResource>(category);
            return Ok(result);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post([FromBody] NewCategoryResource vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Category category = mapper.Map<NewCategoryResource, Category>(vm);

            try
            {
                categoryRepo.Create(category);
                categoryRepo.Save();

                return CreatedAtAction("Get", new { id = category.Id }, category);
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryResource vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Category category = mapper.Map<CategoryResource, Category>(vm);

            if (id != vm.Id)
            {
                return BadRequest();
            }

            try
            {
                categoryRepo.Update(category);
                categoryRepo.Save();
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
        public IActionResult Delete(int id)
        {
            Category category = categoryRepo.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            try
            {
                categoryRepo.Delete(category);
                categoryRepo.Save();
                return Accepted("category deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        private bool CategoryExists(int id)
        {
            if (categoryRepo.GetById(id) != null)
            {
                return true;
            }

            return false;
        }
    }
}
