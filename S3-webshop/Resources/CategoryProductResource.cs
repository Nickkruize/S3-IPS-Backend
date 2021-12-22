using System.Collections.Generic;

namespace S3_webshop.Resources
{
    public class CategoryProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductWithCategoriesResource> Products { get; set; }

        public CategoryProductResource()
        {
            Products = new List<ProductWithCategoriesResource>();
        }
    }
}
