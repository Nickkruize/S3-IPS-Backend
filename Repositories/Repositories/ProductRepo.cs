using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.ContextModels;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class ProductRepo : GenericRepository<Product>, IProductRepo
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
            return this._context.Products.Include(p => p.Categories).ToList();
        }

        public Product FindByIdWithCategoires(int id)
        {
            return this._context.Products.Include(p => p.Categories).FirstOrDefault(e => e.Id == id);
        }
    }
}
