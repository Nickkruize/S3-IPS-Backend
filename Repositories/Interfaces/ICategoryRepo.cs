using DAL.ContextModels;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICategoryRepo : IGenericRepository<Category>
    {
        Task<Category> FindByIdWithProducts(int id);
    }
}
