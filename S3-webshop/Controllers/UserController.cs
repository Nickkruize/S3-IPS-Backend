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

        public UserController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResource>>> Get()
        {
            try
            {
                IEnumerable<User> users = await userService.GetAll();
                return Ok(mapper.Map<List<User>, List<UserResource>>(users.ToList()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> Get(int id)
        {
            try
            {
                User user = await userService.GetById(id);

                if (user == null)
                {
                    return NotFound();
                }

                UserResource result = mapper.Map<User, UserResource>(user);
                return Ok(result);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }

        }
    }
}
