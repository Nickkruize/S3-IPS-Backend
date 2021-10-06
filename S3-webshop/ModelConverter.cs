using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop
{
    public static class ModelConverter
    {
        public static List<Product> ProductsContextModelsToProductViewModels(IEnumerable<DAL.ContextModels.Product> contextmodels)
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

        public static DAL.ContextModels.Product ProductViewModelToProductContextModel(Product viewmodel)
        {
            DAL.ContextModels.Product result = new DAL.ContextModels.Product
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
