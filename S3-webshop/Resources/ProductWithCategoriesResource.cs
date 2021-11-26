using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Resources
{
    public class ProductWithCategoriesResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string imgUrl { get; set; }
        public List<CategoryResource> Categories { get; set; }

        public ProductWithCategoriesResource()
        {
            Categories = new List<CategoryResource>();
        }

        public ProductWithCategoriesResource(int id, string name, string desc, double price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Price = price;
            Categories = new List<CategoryResource>();
        }
    }
}
