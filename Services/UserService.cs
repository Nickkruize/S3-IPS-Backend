using DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepo userRepo, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IEnumerable<IdentityUser> GetAll()
        {
            return _userManager.Users;
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepo.GetById(id);
        }

        public async Task<IdentityUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityUser> GetByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<IdentityUser> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> Delete(IdentityUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task Save()
        {
            await _userRepo.Save();
        }

        public async Task<List<IdentityRole>> GetUserRoles(IdentityUser user)
        {
            IList<string> roleNames = await _userManager.GetRolesAsync(user);
            List<IdentityRole> Roles = new List<IdentityRole>();
            foreach (string rolename in roleNames)
            {
                IdentityRole Role = await _roleManager.FindByNameAsync(rolename);
                Roles.Add(Role);
            }

            return Roles;
        }

        public async Task<bool> CheckPassword(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CheckCreation(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddRoleToNewUser(IdentityUser user)
        {
            return await _userManager.AddToRoleAsync(user, "User");
        }
    }
}
