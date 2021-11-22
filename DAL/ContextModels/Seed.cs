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
                         Price = 34.99,
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

                context.Categories.AddRange(
                     new Category
                     {
                         Name = "Game"
                     },

                     new Category
                     {
                         Name = "Book"
                     },

                     new Category
                     {
                         Name = "Movie"
                     }
                );
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

            List<Category> categories = new List<Category>();
            categories.Add(category1);
            categories.Add(category2);
            categories.Add(category3);
            return categories;
        }
    }
}
