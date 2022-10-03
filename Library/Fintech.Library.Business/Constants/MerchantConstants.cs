namespace Fintech.Library.Business.Constants;
public static class MerchantConstants
{
    public static Dictionary<string,int> GetCompanyTypes()
    {
        var companyTypes = new Dictionary<string, int>
        {
            { "Limited Şirketi", 1 },
            { "Şahış Şirketi", 2 },
            { "Anonim Şirketi", 3 },
            { "Kollektif Şirket", 4 },
            { "Kooperatif Şirket", 5 },
            { "Adi Ortaklık", 6 },
            { "Adi Komandit Şirket", 7 },
            { "EHS Komandit Şirket", 8 },
            { "Diğer", 99 }

        };
        return companyTypes;
    }
}
