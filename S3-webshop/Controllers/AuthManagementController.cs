using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using S3_webshop.Resources;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthManagementController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetByEmail(user.Email);

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

                IdentityResult isCreated = await _userService.CheckCreation(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    IdentityResult roleAdded = await _userService.AddRoleToNewUser(newUser);
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
                IdentityUser existingUser = await _userService.GetByEmail(user.Email);

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

                bool isCorrect = await _userService.CheckPassword(existingUser, user.Password);

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

                List<IdentityRole> Roles = await _userService.GetUserRoles(existingUser);

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
