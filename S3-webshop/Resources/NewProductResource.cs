using S3_webshop.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S3_webshop.Resources
{
    public class NewProductResource
    {
        [Required(ErrorMessage = "A product name must be provided")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A description of the product must be provided")]
        public string Description { get; set; }
        [Required(ErrorMessage = "A price for the product must be provided")]
        [Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9999.99")]
        public double Price { get; set; }
        [UniqueEntriesInListAttribute(ErrorMessage = "a categoryId can't be used multiple times")]
        public List<int> CategoryIds { get; set; }
    }
}
