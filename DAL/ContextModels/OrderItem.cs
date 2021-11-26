using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.ContextModels
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public Order Order { get; set; }
        [Required]
        public Product Product { get; set; }
    }
}
