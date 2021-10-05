using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop
{
    public static class ModelConverter
    {
        public static List<Product> ProductsContextModelsToProductViewModels(List<DAL.ContextModels.Product> contextmodels)
        {
            List<Product> products = new List<Product>();
            foreach (var model in contextmodels)
            {
                products.Add(ProductContextModelToProductViewModel(model));
            }

            return products;
        }

        public static Product ProductContextModelToProductViewModel(DAL.ContextModels.Product contextmodel)
        {
            Product result = new Product
            {
                Id = contextmodel.Id,
                Name = contextmodel.Name,
                Description = contextmodel.Description,
                Price = contextmodel.Price
            };

            return result;
        }
    }
}
