using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllWithCategories();
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        void Update(Product product, int categoryId);
        void Save();
        void Delete(Product product);
        int AddProduct(Product product);
    }
}
