using System;

namespace S3_webshop.Resources
{
    public class OrderItemResource
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProductWithCategoriesResource Product { get; set; }
    }
}
