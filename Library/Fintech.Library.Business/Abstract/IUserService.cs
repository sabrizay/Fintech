using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Abstract
{
    public interface IUserService
    {
        Task<BaseResponse<User>> GetUser(int UserId);
        Task<BaseResponse<User>> Login(LoginModel model);
        Task<BaseResponse<int>> Register(User Model,string Password);
    }
}
