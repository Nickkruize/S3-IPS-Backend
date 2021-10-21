using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace S3_webshop.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<Category> Categories { get; set; }

        public ProductResource()
        {
            Categories = new List<Category>();
        }

        public ProductResource(int id, string name, string desc, double price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Price = price;
            Categories = new List<Category>();
        }

        public void AddCategory (Category category)
        {
            Categories.Add(category);
        }
    }
}
