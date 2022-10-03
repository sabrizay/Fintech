using Fintech.ExternalService.StorageHelper;
using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Concrete
{
    public class CurrencyManager : ICurrencyService
    {
        private readonly ICurrencyHistoryDal _currencyHistoryDal;
        private readonly IStorageHelper _storageHelper;
        private readonly IUserDal _userDal;
        public CurrencyManager(ICurrencyHistoryDal currencyHistoryDal, IStorageHelper storageHelper, IUserDal userDal)
        {
            _currencyHistoryDal = currencyHistoryDal;
            _storageHelper = storageHelper;
            _userDal = userDal;
        }
        public async Task<BaseResponse<int>> AddCurrencyHistory(CurrencyHistory Model)
        {
            var User = await _userDal.Get(x => x.Id == Model.UserId);
            var CrurrencyConvert = await _storageHelper.ConvertCruncy(Model.CurrencySymbol, Model.Price.ToString());
            User.UserPrice = User.UserPrice - CrurrencyConvert.Data.result;
            Model.PriceEUR = CrurrencyConvert.Data.result;
            await _userDal.Update(User);

            Model.IsActive = true;
            Model.CreateDate = DateTime.UtcNow;
            Model.UpdateDate = DateTime.UtcNow;

            try
            {
                await _currencyHistoryDal.Add(Model);
                return new BaseResponse<int> { Success = true };
            }
            catch (Exception)
            {

                return new BaseResponse<int> { Success = false };
            }
        }


        public async Task<BaseResponse<List<CurrencyHistory>>> GetAllCurrencyHistory(int UserId)
        {

            try
            {
                var Result = (List<CurrencyHistory>)await _currencyHistoryDal.GetAll(x => x.UserId == UserId);
                return new BaseResponse<List<CurrencyHistory>>(Result, true);
            }
            catch (Exception)
            {

                return new BaseResponse<List<CurrencyHistory>> { Success = false };
            }
        }
    }
}
