using DAL;
using GenericBusinessLogic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using S3_webshop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Repositories
{
    public class ProductRepo : GenericRepository<DAL.ContextModels.Product>, IProductRepo
    {
        private readonly WebshopContext _context;

        public ProductRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public int AddProduct(DAL.ContextModels.Product product)
        {
                EntityEntry<DAL.ContextModels.Product> created = this._context.Products.Add(product);
                _context.SaveChanges();
                return created.Entity.Id;
        }

        public IEnumerable<DAL.ContextModels.Product> FindAll2()
        {
            return this._context.Products.ToList();
        }
    }
}
