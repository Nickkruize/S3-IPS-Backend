using DAL.ContextModels;
using GenericBusinessLogic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Interfaces
{
    public interface IProductRepo : IGenericRepository<DAL.ContextModels.Product>
    {
        int AddProduct(DAL.ContextModels.Product product);
        IEnumerable<DAL.ContextModels.Product> FindAllWithProductCategories();
        Product FindByIdWithCategoires(int id);
    }
}
