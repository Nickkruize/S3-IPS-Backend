using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.ContextModels
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        [Required]
        public User User { get; set; }
    }
}
