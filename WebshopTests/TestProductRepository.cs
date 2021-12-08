using DAL.ContextModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests
{
    public class TestProductRepository : IProductRepo
    {
        private List<Product> GetProducts()
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
            return Products;
        }

        public Task<IEnumerable<Product>> FindAll()
        {
            return Task.Run(() => this.GetProducts() as IEnumerable<Product>);
        }

        public Task<IEnumerable<Product>> FindByCondition(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int id)
        {
            Product result = this.GetProducts().Find(e => e.Id == id);
            return Task.Run(() => result);
        }

        public Task<Product> Create(Product entity)
        {
            throw new NotImplementedException();

        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> FindAllWithProductCategories()
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindByIdWithCategoires(int id)
        {
            throw new NotImplementedException();
        }
    }
}
