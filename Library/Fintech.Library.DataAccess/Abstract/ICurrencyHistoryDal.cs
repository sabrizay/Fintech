using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Abstract
{
    public interface ICurrencyHistoryDal : IEntityRepository<CurrencyHistory>
    {
    }
}
