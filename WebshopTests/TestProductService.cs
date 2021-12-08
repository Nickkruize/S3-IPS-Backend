using DAL.ContextModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests
{
    public class TestProductService : IProductService
    {
        public Task<IEnumerable<Product>> GetAllWithCategories()
        {
            List<Product> Products = new List<Product>();
            Product product1 = new Product
            {
                Id = 1,
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99
            };
            Product product2 = new Product
            {
                Id = 2,
                Description = "nieuwe beschrijving",
                ImgUrl = "random image",
                Name = "testproduct2",
                Price = 99.99
            };
            Products.Add(product1);
            Products.Add(product2);
            return Task.Run(() => Products as IEnumerable<Product>);

        }
        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdWithCategories(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product product, int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> AddProduct(Product product)
        {
            throw new NotImplementedException();
        }
        
        public Task<Product> AppendCategoriesToProduct(List<int> ids, Product product)
        {
            throw new NotImplementedException();
        }

        public bool VerifyAllSubmittedCategoriesWhereFound(Product product, List<int> categoryIds)
        {
            throw new NotImplementedException();
        }
    }
}
