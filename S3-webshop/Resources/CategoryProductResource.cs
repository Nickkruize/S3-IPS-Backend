using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Resources
{
    public class CategoryProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductResource> Products { get; set; }

        public CategoryProductResource()
        {
            Products = new List<ProductResource>();
        }
    }
}
