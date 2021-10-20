﻿using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop
{
    public static class ModelConverter
    {
        public static List<ProductResource> ProductsContextModelsToProductViewModels(IEnumerable<Product> contextmodels)
        {
            List<ProductResource> products = new List<ProductResource>();
            foreach (var model in contextmodels)
            {
                products.Add(ProductContextModelToProductViewModel(model));
            }

            return products;
        }

        public static ProductResource ProductContextModelToProductViewModel(Product contextmodel)
        {
            ProductResource result = new ProductResource
            {
                Id = contextmodel.Id,
                Name = contextmodel.Name,
                Description = contextmodel.Description,
                Price = contextmodel.Price
            };

            foreach (ProductCategory cat in contextmodel.ProductCategories)
            {
                result.AddCategoryId(cat.CategoryId);
            }

            foreach (ProductCategory cat in contextmodel.ProductCategories)
            {
                result.AddCategory(cat.Category);
            }

            return result;
        }

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
