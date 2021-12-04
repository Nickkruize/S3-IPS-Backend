using DAL.ContextModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> FindAllWithProductCategories();
        Task<Product> FindByIdWithCategoires(int id);
    }
}
