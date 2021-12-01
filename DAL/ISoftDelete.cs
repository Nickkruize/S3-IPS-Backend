using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface ISoftDelete
    {
        public bool SoftDeleted { get; set; }
    }
}
