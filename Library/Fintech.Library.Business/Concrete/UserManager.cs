using Fintech.Library.Business.Constants;
using Fintech.Library.Core.Utilities.Hashing;
using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly ICurrencyHistoryDal _currencyHistoryDal;
        public UserManager(IUserDal userDal, ICurrencyHistoryDal currencyHistoryDal)
        {
            _userDal = userDal;
            _currencyHistoryDal = currencyHistoryDal;
        }

        public async Task<BaseResponse<User>> GetUser(int UserId)
        {
            var result = await _userDal.Get(x => x.Id == UserId);
            result.currencyHistories=(List<CurrencyHistory>) await _currencyHistoryDal.GetAll(x=>x.UserId==UserId);        
     
            return new BaseResponse<User>(result,true);
        }

        public async Task<BaseResponse<User>> Login(LoginModel model)
        {
            var result = await _userDal.Get(x => x.Email == model.Email);           
            if (HashingHelper.VerifyPasswordHash(model.Password, result.PasswordHash, result.PasswordSalt))
                return CheckUser(result);
            else
                return new BaseResponse<User>() { Success = false, error = new Error { message = Messages.AuthMessage.UserNotFound } };


        }
        public async Task<BaseResponse<int>> Register(User Model, string Password)
        {
            Model.IsActive = true;
            Model.CreateDate = DateTime.UtcNow;
            Model.UpdateDate = DateTime.UtcNow;

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(Password, out passwordHash, out passwordSalt);
            Model.PasswordHash = passwordHash;
            Model.PasswordSalt = passwordSalt;
            try
            {
                await _userDal.Add(Model);
                return new BaseResponse<int> { Success = true };
            }
            catch (Exception)
            {

                return new BaseResponse<int> { Success = false };
            }


        }



        private BaseResponse<User> CheckUser(User UserModel)
        {
            if (UserModel is null)
                return new BaseResponse<User>() { Success = false, error = new Error { message = Messages.UserMessages.UserNotFound } };

            if (UserModel.IsDeleted == true)
                return new BaseResponse<User>() { Success = false, error = new Error { message = Messages.UserMessages.UserDeleted } };

            if (UserModel.IsActive == false)
                return new BaseResponse<User>() { Success = false, error = new Error { message = Messages.UserMessages.UserNotActive } };

            return new BaseResponse<User>(UserModel, true);
        }
    }
}
