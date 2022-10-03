using System.Security.Claims;

namespace Fintech.Library.Core.Extentions
{
    public static class ClaimsPrincipalExtentions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            List<string> result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();


            return result;
        }


        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims(ClaimTypes.Role);
        }
        public static int GetMerchantId(this ClaimsPrincipal claimsPrincipal)
        {
            _ = int.TryParse(claimsPrincipal.FindFirst("MerchantId")?.Value, out int merchantId);
            return merchantId;
        }
    }
}
