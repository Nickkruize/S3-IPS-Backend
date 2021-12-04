using DAL;
using DAL.ContextModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
    {
        private readonly WebshopContext _context;

        public CategoryRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Category> FindByIdWithProducts(int id)
        {
            return await this._context.Categories.Include(p => p.Products).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
