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
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepo.FindAll();
        }

        public IEnumerable<Product> GetAllWithCategories()
        {
            return _productRepo.FindAllWithProductCategories();
        }

        public Product GetById(int id)
        {
            return _productRepo.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepo.FindByIdWithCategoires(id);
        }

        public void Update(Product product)
        {
            _productRepo.Update(product);
        }

        public void Save()
        {
            _productRepo.Save();
        }
        public void Delete(Product product)
        {
            _productRepo.Delete(product);
        }

        public int AddProduct(Product product)
        {
            return _productRepo.AddProduct(product);
        }
    }
}
