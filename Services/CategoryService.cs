using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepo.FindAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryRepo.GetById(id);
        }

        public async Task<Category> GetByIdWithProduct(int id)
        {
            return await _categoryRepo.FindByIdWithProducts(id);
        }

        public async Task Delete(Category category)
        {
            _categoryRepo.Delete(category);
            await _categoryRepo.Save();
        }

        public async Task Update(Category category)
        {
            _categoryRepo.Update(category);
            await _categoryRepo.Save();
        }

        public async Task<Category> AddCategory(Category category)
        {
            Category result = await _categoryRepo.Create(category);
            await _categoryRepo.Save();
            return result;
        }
    }
}
