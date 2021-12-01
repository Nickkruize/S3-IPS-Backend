using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;

        public UserService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await userRepo.FindAll();
        }

        public async Task<User> GetById(int id)
        {
            return await userRepo.GetById(id);
        }

        public async Task Save()
        {
            await userRepo.Save();
        }
    }
}
