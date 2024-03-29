﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace S3_webshop.Validation
{
    public class UniqueEntriesInListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            List<int> categoryIds = value as List<int>;

            IEnumerable<int> duplicates = categoryIds.GroupBy(x => x)
                                            .SelectMany(g => g.Skip(1));

            if (duplicates.Any())
            {
                return false;
            }
            return true;
        }
    }
}
