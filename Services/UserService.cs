using DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
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

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepo.FindAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepo.GetById(id);
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
    }
}
