using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class ProductService :  IProductService
    {
        private readonly IProductRepo productRepo;
        private readonly ICategoryRepo categoryRepo;

        public ProductService(IProductRepo productRepo, ICategoryRepo categoryRepo)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
        }

        public IEnumerable<Product> GetAll()
        {
            return productRepo.FindAll();
        }

        public IEnumerable<Product> GetAllWithCategories()
        {
            return productRepo.FindAllWithProductCategories();
        }

        public Product GetById(int id)
        {
            return productRepo.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return productRepo.FindByIdWithCategoires(id);
        }

        public void Update(Product product, int categoryId)
        {
            Category category = categoryRepo.GetById(categoryId);
            List<Category> categories = new List<Category>
            {
                category
            };
            product.Categories = categories;
            productRepo.Update(product);
        }

        public void Save()
        {
            productRepo.Save();
        }
        public void Delete(Product product)
        {
            productRepo.Delete(product);
        }

        public int AddProduct(Product product)
        {
            return productRepo.AddProduct(product);
        }
    }
}
