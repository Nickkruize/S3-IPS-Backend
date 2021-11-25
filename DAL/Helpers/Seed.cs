﻿using Bogus;
using DAL.ContextModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Helpers
{
    public static class Seed
    {
        public static List<Product> SeedProducts(List<Category> categories)
        {
            Faker<Product> productToFake = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => double.Parse(f.Commerce.Price(1, 1000, 2)))
                .RuleFor(p => p.Description, f => f.Lorem.Lines(10));

            List<Product> products = productToFake.Generate(25);

            foreach (Product product in products)
            {
                product.Categories = GetRandomCategories(categories, 2);
            }

            return products;
        }

        public static List<Category> SeedCategories()
        {
            Faker<Category> categoryToFake = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0].ToString());

            List<Category> categories = categoryToFake.Generate(15);
            List<Category> result = new List<Category>();
            foreach (Category category in categories)
            {
                if (categories.Count(c => c.Name == category.Name) == 1)
                {
                    result.Add(category);
                }
            }

            return result;
        }

        private static List<Category> GetRandomCategories(List<Category> categories, int amount)
        {
            Random random = new Random();
            List<Category> categoriesResult = new List<Category>();
            for (int i = 1; i <= amount; i++)
            {
                int index = random.Next(categories.Count());
                categoriesResult.Add(categories[index]);
            }

            return categoriesResult;
        }
    }
}
