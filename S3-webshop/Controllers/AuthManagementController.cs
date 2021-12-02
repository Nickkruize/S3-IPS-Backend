using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using S3_webshop.Resources;
using Services.Interfaces;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AuthManagementController(UserManager<IdentityUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponseDto()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Succes = false
                    });
                }

                IdentityUser newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Username,
                };

                IdentityResult isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    IdentityResult roleAdded = await _userManager.AddToRoleAsync(newUser, "User");
                    if (roleAdded.Succeeded)
                    {
                        return Ok(new RegistrationResponseDto()
                        {
                            Succes = true
                        });
                    }
                    else
                    {
                        return BadRequest(new RegistrationResponseDto()
                        {
                            Errors = roleAdded.Errors.Select(x => x.Description).ToList(),
                            Succes = false
                        });
                    }
                }
                else
                {
                    return BadRequest(new RegistrationResponseDto()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Succes = false
                    });
                }
            }

            return BadRequest(new RegistrationResponseDto()
            {
                Errors = new List<string>()
                { 
                    "Invalid password or email"
                },
                Succes = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new LoginResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Succes = false
                    });
                }

                bool isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new LoginResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Succes = false
                    });
                }

                IList<string> roleNames = await _userManager.GetRolesAsync(existingUser);
                List<IdentityRole> Roles = new List<IdentityRole>();
                foreach (string rolename in roleNames)
                {
                    IdentityRole Role = await _roleManager.FindByNameAsync(rolename);
                    Roles.Add(Role);
                }
                string jwtToken = _jwtService.GenerateJwtToken(existingUser, Roles);

                HttpContext.Response.Cookies.Append("UserLoginCookie", jwtToken, new CookieOptions
                {
                    HttpOnly = true 
                });

                return Ok(new LoginResponse()
                {
                    Succes = true,
                    Token = jwtToken
                });
            }


            return BadRequest(new LoginResponse()
            {
                Errors = new List<string>()
                {
                    "Please enter a password and a valid email"
                },
                Succes = false
            });
        }
    }
}
