using Fintech.ExternalService.StorageHelper;
using Fintech.ExternalService.StorageHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Concrete
{
    public class FixerManager : IFixerService
    {
        private readonly IStorageHelper _storageHelper;
        public FixerManager(IStorageHelper storageHelper)
        {
            _storageHelper = storageHelper;
        }
        public Task<BaseResponse<FixerSymbolsModel>> GetAllCruncy()
        {
            return _storageHelper.GetAllCruncy();
        }
    }
}
