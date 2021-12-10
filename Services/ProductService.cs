using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService :  IProductService
    {
        private readonly IProductRepo productRepo;
        private readonly ICategoryRepo categoryRepo;

        public ProductService(IProductRepo productRepo, ICategoryRepo categoryRepo)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Product>> GetAllWithCategories()
        {
            return await productRepo.FindAllWithProductCategories();
        }

        public async Task<Product> GetById(int id)
        {
            return await productRepo.GetById(id);
        }

        public async Task<Product> GetByIdWithCategories(int id)
        {
            return await productRepo.FindByIdWithCategoires(id);
        }

        public async Task Update(Product product, int categoryId)
        {
            Category category = await categoryRepo.GetById(categoryId);
            List<Category> categories = new List<Category>
            {
                category
            };
            product.Categories = categories;
            productRepo.Update(product);
            await productRepo.Save();
        }

        public async Task<Product> Delete(Product product)
        {
            await productRepo.Delete(product);
            await productRepo.Save();
            return product;
        }

        public async Task<Product> AddProduct(Product product)
        {
            Product result = await productRepo.Create(product);
            await productRepo.Save();
            return result;
        }

        public async Task<Product> AppendCategoriesToProduct(List<int> ids, Product product)
        {
            product.Categories = await categoryRepo.FindByCondition(e => ids.Contains(e.Id)) as List<Category>;
            return product;
        }

        public bool VerifyAllSubmittedCategoriesWhereFound(Product product, List<int> categoryIds)
        {
            if (product.Categories.Count != categoryIds.Count)
            {
                return false;
            }

            return true;
        }
    }
}
