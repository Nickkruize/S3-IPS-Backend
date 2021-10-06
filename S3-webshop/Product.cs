using System;

namespace S3_webshop
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public Product()
        {

        }

        public Product(int id, string name, string desc, double price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Price = price;
        }
    }
}
