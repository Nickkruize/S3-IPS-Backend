using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;

        public UserService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public User RegisterUser(string email, string password, string username)
        {
            User user = new User
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Username = username
            };

            return userRepo.Create(user);
        }

        //public bool Login(User user)
        //{
        //    try
        //    {
        //        User StoredInfo = userRepo.FindByEmail(user.Email);
        //        if (StoredInfo == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return BCrypt.Net.BCrypt.Verify(user.Password, StoredInfo.Password);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IEnumerable<User> GetAll()
        {
            return userRepo.FindAll();
        }

        public User GetById(int id)
        {
            return userRepo.GetById(id);
        }

        //public User GetByEmail(string email)
        //{
        //    return userRepo.FindByEmail(email);
        //}

        public void Save()
        {
            userRepo.Save();
        }
    }
}
