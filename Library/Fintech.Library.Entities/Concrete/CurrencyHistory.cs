using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Entities.Concrete
{
    [Table("CurrencyHistory")]
    public class CurrencyHistory : BaseEntity, IEntity
    {
        public string? CurrencySymbol { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceEUR { get; set; }
        public int? UserId { get; set; }


    }
}
