using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task Delete(Category category);
        Task Update(Category category);
        Task<Category> AddCategory(Category category);
        Task<Category> GetByIdWithProduct(int id);
    }
}
