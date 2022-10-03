using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Fintech.Library.Core.Extentions;

public static class ClaimExtentions
{
    public static void AddEmail(this ICollection<Claim> claims, string email)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, value: email));
    }
    public static void AddNameIdentifier(this ICollection<Claim> claims, string UserId)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, value: UserId));
    }
    public static void AddMerchantId(this ICollection<Claim> claims, string UserCompanyId)
    {
        claims.Add(new Claim("MerchantId", value: UserCompanyId));
    }
    public static void AddName(this ICollection<Claim> claims, string Name)
    {
        claims.Add(new Claim(ClaimTypes.Name, value: Name));
    }
    public static void AddRoles(this ICollection<Claim> claims, IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, value: role));
        }
    }
}
