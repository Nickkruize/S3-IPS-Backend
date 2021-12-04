using DAL.ContextModels;

namespace Repositories.Interfaces
{
    public interface IUserRepo : IGenericRepository<User>
    {
        //User FindByEmail(string email);
    }
}
