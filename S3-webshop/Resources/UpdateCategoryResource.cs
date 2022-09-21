using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Resources
{
    public class UpdateCategoryResource
    {
        [Required(ErrorMessage ="A Name is required to alter the category")]
        [MaxLength(20, ErrorMessage ="Category name can not be more than 20 characters")]
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }
}
