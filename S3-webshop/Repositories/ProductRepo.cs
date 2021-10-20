using DAL;
using GenericBusinessLogic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using S3_webshop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.ContextModels;

namespace S3_webshop.Repositories
{
    public class ProductRepo : GenericRepository<DAL.ContextModels.Product>, IProductRepo
    {
        private readonly WebshopContext _context;

        public ProductRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public int AddProduct(Product product)
        {
                EntityEntry<Product> created = this._context.Products.Add(product);
                _context.SaveChanges();
                return created.Entity.Id;
        }

        public IEnumerable<Product> FindAllWithProductCategories()
        {
            return this._context.Products.Include(p => p.ProductCategories).ThenInclude(q => q.Category).ToList();
        }

        public Product FindByIdWithCategoires(int id)
        {
            return this._context.Products.Include(p => p.ProductCategories).ThenInclude(q => q.Category).FirstOrDefault(e => e.Id == id);
        }
    }
}
