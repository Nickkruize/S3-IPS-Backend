using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.ContextModels
{
    public static class Seed
    {
        public static void InitializeProducts(IApplicationBuilder app)
        {
            var scopee = app.ApplicationServices.CreateScope();
            using (var context = new WebshopContext(
                scopee.ServiceProvider.GetRequiredService<DbContextOptions<WebshopContext>>()))
            {
                // Look for any movies.
                if (context.Products.Any())
                {
                    return;   // DB has been seeded
                }

                context.Products.AddRange(
                     new Product
                     {
                         Name = "World of Warcraft",
                         Description = "MMORPG By Blizzard Entertaimment",
                         Price = 34.99
                     },

                     new Product
                     {
                         Name = "Harry Potter and the Chamber of Secrets",
                         Description = "2nd Harry Potter book in the series",
                         Price = 9.99
                     },

                     new Product
                     {
                         Name = "No Time To Die",
                         Description = "James Bond Movie released in 2021",
                         Price = 24.99
                     }
                ) ;
                context.SaveChanges();
            }
        }

        private static List<Category> GetCategories()
        {
            Category category1 = new Category
            {
                Name = "Game"
            };

            Category category2 = new Category
            {
                Name = "Book"
            };

            Category category3 = new Category
            {
                Name = "Movie"
            };

            List<Category> categories = new List<Category>
            {
                category1,
                category2,
                category3
            };
            return categories;
        }

        public static void InitializeCategories(IApplicationBuilder app)
        {
            var scopee = app.ApplicationServices.CreateScope();
            using (var context = new WebshopContext(
                scopee.ServiceProvider.GetRequiredService<DbContextOptions<WebshopContext>>()))
            {
                // Look for any movies.
                if (context.Categories.Any())
                {
                    return;   // DB has been seeded
                }

                List<Category> categories = GetCategories();
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        public static void AssignCategories(IApplicationBuilder app)
        {
            var scopee = app.ApplicationServices.CreateScope();
            using (var context = new WebshopContext(
                scopee.ServiceProvider.GetRequiredService<DbContextOptions<WebshopContext>>()))
            {
                if (!context.Categories.Any() && !context.Products.Any())
                {
                    List<Category> categories = context.Categories.ToList();
                    List<Product> products = context.Products.ToList();

                    for (int i = 0; i < categories.Count; i++)
                    {
                        products[i].Categories = new List<Category>
                        {
                        categories[i]
                        };
                    }
                    context.Products.UpdateRange(products);
                }
            }
        }
    }
}
