using System;
using System.Collections.Generic;

namespace S3_webshop.Resources
{
    public class OrdersResource
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResource> OrderItems { get; set; }
        public UserResource User { get; set; }
    }
}
