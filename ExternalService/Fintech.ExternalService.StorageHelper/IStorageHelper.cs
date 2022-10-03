using Fintech.ExternalService.StorageHelper.Enums;
using Fintech.ExternalService.StorageHelper.Models;
using Fintech.Library.Entities.Models;

namespace Fintech.ExternalService.StorageHelper;

public interface IStorageHelper
{

    Task<BaseResponse<FixerConvertResponseModel>> ConvertCruncy(string key, string amount);
    /// <summary>
    /// Get All Cruncy Symbol
    /// </summary>
    /// <returns></returns>
    Task<BaseResponse<FixerSymbolsModel>> GetAllCruncy();
  
}
