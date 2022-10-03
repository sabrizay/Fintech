
using Fintech.ExternalService.StorageHelper.Enums;
using Fintech.ExternalService.StorageHelper.fixer;
using Fintech.ExternalService.StorageHelper.Models;
using Fintech.Library.Entities.Models;

namespace Fintech.ExternalService.StorageHelper;

public class StorageHelper : IStorageHelper
{
    private readonly FixerHelper _fixerHelper;
    public StorageHelper()
    {
      
        _fixerHelper = new FixerHelper();
    }

  

    public async Task<BaseResponse<FixerSymbolsModel>> GetAllCruncy()
    {
        var result = await _fixerHelper.GetAllCruncy();
        return new BaseResponse<FixerSymbolsModel>(result, true);
    } 
    
    public async Task<BaseResponse<FixerConvertResponseModel>> ConvertCruncy(string key, string amount)
    {
        var result = await _fixerHelper.ConvertCruncy(key,amount);
        return new BaseResponse<FixerConvertResponseModel>(result, true);
    }

  
}
