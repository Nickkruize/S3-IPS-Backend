using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using S3_webshop.Resources;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResource>>> Get()
        {
            try
            {
                IEnumerable<IdentityUser> users = _userService.GetAll();
                return Ok(_mapper.Map<List<IdentityUser>, List<UserResource>>(users.ToList()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> GetById(string id)
        {
            try
            {
                IdentityUser user = await _userService.GetById(id);
                UserResource userResource = _mapper.Map<IdentityUser, UserResource>(user);

                if (userResource == null)
                {
                    return NotFound();
                }

                return Ok(userResource);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<UserResource>> GetByName(string name)
        {
            try
            {
                IdentityUser user = await _userService.GetByName(name);
                UserResource userResource = _mapper.Map<IdentityUser, UserResource>(user);

                if (userResource == null)
                {
                    return NotFound();
                }

                return Ok(userResource);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<UserResource>> GetByEmail(string email)
        {
            try
            {
                IdentityUser user = await _userService.GetByEmail(email);
                UserResource userResource = _mapper.Map<IdentityUser, UserResource>(user);

                if (userResource == null)
                {
                    return NotFound();
                }

                return Ok(userResource);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }
    }
}

