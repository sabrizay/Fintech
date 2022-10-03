using Fintech.Library.DataAccess.Concrete.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Abstract
{
    public interface IMerchantDal : IGenericRepository
    {
        //Task<IEnumerable<MerchantDto>> GetAllAsync(int pageIndex, int pageSize);
        //Task<MerchantDto> GetById(int merchantId);
        //Task<MerchantDto> GetByParameters(string emailAdress, string taxId);
    }
}
