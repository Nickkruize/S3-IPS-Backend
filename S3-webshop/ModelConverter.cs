using DAL.ContextModels;
using S3_webshop.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop
{
    public static class ModelConverter
    {
        public static Product ProductViewModelToProductContextModel(ProductResource viewmodel)
        {
            Product result = new Product
            {
                Id = viewmodel.Id,
                Name = viewmodel.Name,
                Description = viewmodel.Description,
                Price = viewmodel.Price
            };

            return result;
        }
    }
}
