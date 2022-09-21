using System.Collections.Generic;

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
