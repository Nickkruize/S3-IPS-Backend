using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.ContextModels
{
    public class PaymentDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string PaymentProvider { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get;set; }
        public DateTime ModifiedAt { get; set; }
    }
}
