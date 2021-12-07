using DAL;
using DAL.ContextModels;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly WebshopContext context;

        public UserRepo(WebshopContext context) : base(context)
        {
            this.context = context;
        }
    }
}
