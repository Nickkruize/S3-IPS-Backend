using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IUserRepo : IGenericRepository<User>
    {
        User FindByEmail(string email);
    }
}
