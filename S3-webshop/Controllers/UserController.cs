using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3_webshop.Resources;
using Services.Interfaces;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        public IEnumerable<UserResource> Get()
        {
            List<User> users = userService.GetAll().ToList();
            return mapper.Map<List<User>, List<UserResource>>(users);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResource> Get(int id)
        {
            User user = userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            UserResource result = mapper.Map<User, UserResource>(user);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(NewUserResource input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = mapper.Map<NewUserResource, User>(input);

            try
            {
                userService.RegisterUser(user.Email, user.Password);
                userService.Save();

                return CreatedAtAction("Get", new { id = user.Id }, user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("/Login")]
        public IActionResult Login(LoginResource input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = mapper.Map<LoginResource, User>(input);

            try
            {
                if (userService.Login(user))
                {
                    return Ok();
                }

                return BadRequest("Incorrect information");
                
            }
            catch
            {
                return BadRequest("Database Error");
            }
        }
    }
}
