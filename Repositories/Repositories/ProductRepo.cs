using DAL;
using DAL.ContextModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
