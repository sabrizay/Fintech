using Fintech.Library.Entities.Dto;

namespace Fintech.Library.Core.Utilities.Security.Jwt;

public interface ITokenHelper
{
    AccessToken CreateToken(UserDto User, IEnumerable<string> UserOperationClaims);
}
