using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ContextModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
