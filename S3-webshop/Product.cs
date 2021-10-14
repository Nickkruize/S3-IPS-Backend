using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace S3_webshop
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<int> CategorieIds { get; set; }

        public Product()
        {
            CategorieIds = new List<int>();
        }

        public Product(int id, string name, string desc, double price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Price = price;
            CategorieIds = new List<int>();
        }

        public void AddCategory (int categoryId)
        {
            CategorieIds.Add(categoryId);
        }
    }
}
