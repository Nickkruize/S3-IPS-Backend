using DAL;
using DAL.ContextModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Repositories
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly WebshopContext context;

        public UserRepo(WebshopContext context) : base(context)
        {
            this.context = context;
        }

        //public User FindByEmail(string email)
        //{
        //    return context.Users.FirstOrDefault(e => e.Email == email);
        //}
    }
}
