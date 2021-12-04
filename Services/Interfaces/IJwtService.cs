using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(IdentityUser user, List<IdentityRole> roles);
    }
}
