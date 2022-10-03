using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Entities.Concrete
{
    public class Caches
    {
       
        public string Id { get; set; }

        public string CacheName { get; set; }
        public int? TimeoutMin { get; set; }
        public DateTime? LastUpdateDate { get; set; }

    }
}
