using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Validation
{
    public class UniqueEntriesInList : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            List<int> categoryIds = value as List<int>;

            IEnumerable<int> duplicates = categoryIds.GroupBy(x => x)
                                            .SelectMany(g => g.Skip(1));

            if (duplicates.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
