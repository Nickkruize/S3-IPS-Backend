using DAL;
using DAL.ContextModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Repositories
{
    public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
    {
        private readonly WebshopContext _context;

        public CategoryRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public Category FindByIdWithProducts (int id)
        {
            return this._context.Categories.Include(p => p.ProductCategories).ThenInclude(q => q.Product).FirstOrDefault(e => e.Id == id);
        }
    }
}
