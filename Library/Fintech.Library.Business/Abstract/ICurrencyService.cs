using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Abstract
{
    public interface ICurrencyService
    {
        Task<BaseResponse<int>> AddCurrencyHistory(CurrencyHistory Model);

        [CacheAspect(Duration: 30)]
        Task<BaseResponse<List<CurrencyHistory>>> GetAllCurrencyHistory(int UserId);
    }
}
