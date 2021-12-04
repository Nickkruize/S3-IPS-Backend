using DAL.ContextModels;
using S3_webshop.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        [UniqueEntriesInList(ErrorMessage = "CategoryId can't be used multiple times")]
        public List<int> CategoryIds { get; set; }
    }
}
