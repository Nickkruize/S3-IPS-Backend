using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface ICategoryRepo : IGenericRepository<Category>
    {
        Category FindByIdWithProducts(int id);
    }
}
