using DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<IdentityUser> GetAll();
        Task<User> GetById(int id);
        Task<IdentityUser> GetByName(string name);
        Task<IdentityUser> GetByEmail(string email);
        Task Save();
        Task<List<IdentityRole>> GetUserRoles(IdentityUser user);
        Task<IdentityUser> GetById(string id);
        Task<bool> CheckPassword(IdentityUser user, string password);
        Task<IdentityResult> CheckCreation(IdentityUser user, string password);
        Task<IdentityResult> AddRoleToNewUser(IdentityUser user);
        Task<IdentityResult> Delete(IdentityUser user);
    }
}
