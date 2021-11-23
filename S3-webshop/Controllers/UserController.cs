using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3_webshop.Resources;
using Services;
using Services.Interfaces;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IJwtService jwtService;

        public UserController(IMapper mapper, IUserService userService, IJwtService jwtService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.jwtService = jwtService;
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

        [HttpPost]
        [Route("[action]")]
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
                    UserResource userResponse = mapper.Map<User, UserResource>(userService.GetByEmail(user.Email));
                    var jwt = jwtService.Generate();
                    userResponse.Jwt = jwt;

                    //Response.Cookies.Append("jwt", jwt, new CookieOptions
                    //{
                    //    HttpOnly = true
                    //});

                    return Ok(userResponse);
                }

                return BadRequest("Incorrect information");
                
            }
            catch
            {
                return BadRequest("Database Error");
            }
        }

        //[HttpPost("Logout")]
        //public IActionResult Logout()
        //{
        //    Response.Cookies.Delete("jwt");

        //    return Ok("Logout succes");
        //}
    }
}
