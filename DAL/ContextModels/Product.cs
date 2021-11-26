using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ContextModels
{
    public class Product
    {
        public Product()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
