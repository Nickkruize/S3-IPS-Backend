using S3_webshop.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S3_webshop.Resources
{
    public class NewProductResource
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [UniqueEntriesInListAttribute(ErrorMessage = "CategoryId can't be used multiple times")]
        public List<int> CategoryIds { get; set; }
    }
}
