﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.ContextModels
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
