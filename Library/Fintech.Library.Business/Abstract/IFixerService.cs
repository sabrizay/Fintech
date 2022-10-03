using Fintech.ExternalService.StorageHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Abstract
{
    public interface IFixerService
    {
      
        Task<BaseResponse<FixerSymbolsModel>> GetAllCruncy();
    }
}
