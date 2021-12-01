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

        public async Task<IEnumerable<Product>> FindAllWithProductCategories()
        {
            return await this._context.Products.Include(p => p.Categories).ToListAsync();
        }

        public async Task<Product> FindByIdWithCategoires(int id)
        {
            return await this._context.Products.Include(p => p.Categories).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
