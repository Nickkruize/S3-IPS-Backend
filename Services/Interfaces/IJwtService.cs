using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(IdentityUser user, List<IdentityRole> roles);
    }
}
