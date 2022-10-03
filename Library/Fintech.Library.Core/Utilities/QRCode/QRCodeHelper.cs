using QRCoder;

namespace Fintech.Library.Core.Utilities.QRCode;

public static class QRCodeHelper
{
    public static string CreateBase64QRCode(string QryCodeString)
    {
        QRCodeGenerator qRCodeGenerator = new();
        var qrCodeData = qRCodeGenerator.CreateQrCode(QryCodeString, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(qrCodeData);
        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);

        return $"data:image/png;base64,{Convert.ToBase64String(qrCodeAsPngByteArr.ToArray())}";
    }
}
