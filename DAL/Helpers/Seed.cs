using Bogus;
using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Helpers
{
    public static class Seed
    {
        public static List<Product> SeedProducts(List<Category> categories)
        {
            Faker<Product> productToFake = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => double.Parse(f.Commerce.Price(1, 1000, 2)))
                .RuleFor(p => p.Description, f => f.Lorem.Lines(10))
                .RuleFor(p => p.ImgUrl, f => f.Image.PicsumUrl(480, 480));

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

            List<Category> categories = categoryToFake.Generate(10);
            List<Category> result = new List<Category>();
            foreach (Category category in categories)
            {
                if (categories.Count(c => c.Name == category.Name) == 1 || result.Count(e => e.Name == category.Name) == 0)
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

        private static Product GetRandomProduct(List<Product> products)
        {
            Random random = new Random();

            int index = random.Next(products.Count());
            Product product = products[index];

            return product;
        }

        public static List<OrderItem> SeedOrderItems(List<Product> products, List<Order> orders)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            Faker<OrderItem> orderItemToFake = new Faker<OrderItem>()
                .RuleFor(o => o.CreatedAt, f => f.Date.Future(1))
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));
            Random random = new Random();

            for (int i = 0; i < 30; i++)
            {
                OrderItem orderItem = orderItemToFake.Generate();
                orderItem.Product = GetRandomProduct(products);
                int index = random.Next(0, orders.Count);
                orderItem.Order = orders[index];
                orderItems.Add(orderItem);
            }

            return orderItems;
        }

        public static List<Order> SeedOrders(List<User> users)
        {
            List<Order> orders = new List<Order>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(0, users.Count);
                Order order = new Order
                {
                    CreatedAt = DateTime.Now,
                    User = users[index],
                };
                orders.Add(order);
            }

            return orders;
        }

        public static List<User> SeedUser()
        {
            List<User> users = new List<User>();

            Faker<User> userToFake = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Internet.ExampleEmail())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password(10)));

            for (int i = 0; i < 10; i++)
            {
                User user = userToFake.Generate();
                users.Add(user);
            }

            return users;
        }
    }
}
