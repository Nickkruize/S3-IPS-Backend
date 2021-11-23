using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class JwtService : IJwtService
    {
        private string securityKey = "this is a very secure key";
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            this._config = config;
        }

        public string Generate()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken Verifty(string Jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);
            tokenHandler.ValidateToken(Jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false

            }
            , out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
