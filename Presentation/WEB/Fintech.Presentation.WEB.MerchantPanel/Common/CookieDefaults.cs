namespace Fintech.Presentation.WEB.MerchantPanel.Common
{
    public static class CookieDefaults
    {
        private static string Prefix => ".Modalog";
        public static string MerchantCookie => $"{Prefix}.Merchant";

        public static string AntiforgeryCookie => $"{Prefix}.Antiforgery";

        public static string SessionCookie => $"{Prefix}.Session";

        public static string CultureCookie => $"{Prefix}.Culture";

        public static string TempDataCookie => $"{Prefix}.TempData";

        public static string AuthenticationCookie => $"{Prefix}.Authentication";

        public static string IgnoreKVKKCookiewWarning => $"{Prefix}.IgnoreEuCookieLawWarning";
    }
}
