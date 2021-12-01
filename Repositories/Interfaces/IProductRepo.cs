using DAL.ContextModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> FindAllWithProductCategories();
        Task<Product> FindByIdWithCategoires(int id);
    }
}
