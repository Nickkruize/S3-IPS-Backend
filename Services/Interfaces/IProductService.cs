using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetAllWithCategories();
        Task<Product> GetById(int id);
        Task<Product> GetByIdWithCategories(int id);
        Task Update(Product product, int categoryId);
        Task Save();
        void Delete(Product product);
        Task<Product> AddProduct(Product product);
    }
}
