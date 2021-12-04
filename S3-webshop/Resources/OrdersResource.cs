using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
