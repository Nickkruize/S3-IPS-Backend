using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
