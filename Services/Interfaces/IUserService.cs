using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IUserService
    {
        User RegisterUser(string email, string password);
        bool Login(User user);
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Save();
    }
}
