using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IJwtService jwtService;
        public LoginController(IJwtService jwtService)
        {
            this.jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = jwtService.Generate();
                Response.Cookies.Append("Jwt", tokenString, new CookieOptions
                {
                    HttpOnly = true
                });
                
                response = Ok(new { token = tokenString });
            }
            return response;
        }
        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.Username == "Jignesh")
            {
                user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com" };
            }
            return user;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
