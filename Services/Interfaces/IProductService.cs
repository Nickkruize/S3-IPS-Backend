using DAL.ContextModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllWithCategories();
        Task<Product> GetById(int id);
        Task<Product> GetByIdWithCategories(int id);
        Task<Product> Update(Product product, int categoryId);
        Task<Product> Delete(Product product);
        Task<EntityEntry> Delete2(Product product);
        Task<Product> AddProduct(Product product);
        Task<Product> AppendCategoriesToProduct(List<int> ids, Product product);
        bool VerifyAllSubmittedCategoriesWhereFound(Product product, List<int> categoryIds);
    }
}
