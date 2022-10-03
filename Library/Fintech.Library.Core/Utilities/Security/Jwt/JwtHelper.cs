using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Fintech.Library.Core.Extentions;
using Fintech.Library.Core.Utilities.Security.Encryption;
using Fintech.Library.Entities.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fintech.Library.Core.Utilities.Security.Jwt;

public class JwtHelper : ITokenHelper
{
    private readonly IConfiguration _configuration;
    private readonly TokenOptions _tokenOptions;
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }

    public AccessToken CreateToken(UserDto user, IEnumerable<string> userOperationClaims)
    {
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);


        var jwtSecurityToken = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, userOperationClaims);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


        return new AccessToken()
        {
            Token = token,
            Expiration = jwtSecurityToken.ValidTo
        };

    }
    private static JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, UserDto user, SigningCredentials signingCredentials, IEnumerable<string> userOperationClaims)
    {
        JwtSecurityToken Jwt = new(
            issuer: tokenOptions.Issuer,
            audience: tokenOptions.Audience,
            claims: SetClaims(user, userOperationClaims),
            expires: DateTime.Now.AddHours(tokenOptions.AccessTokenExpiration),
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials);

        return Jwt;
    }

    private static IEnumerable<Claim> SetClaims(UserDto user, IEnumerable<string> userOperationClaims)
    {
        List<Claim> claimList = new()
        {
            //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.Name, user.FullName),
            //new Claim("MerchantId", user.MerchantId.ToString()),
        };

        claimList.AddRoles(userOperationClaims);
        return claimList;
    }
}




