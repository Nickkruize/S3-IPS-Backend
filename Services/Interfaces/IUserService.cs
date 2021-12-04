using DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task Save();
        Task<List<IdentityRole>> GetUserRoles(IdentityUser user);
    }
}
