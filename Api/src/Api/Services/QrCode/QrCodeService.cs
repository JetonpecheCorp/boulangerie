using QRCoder;

namespace Services.QrCodes;
public sealed class QrCodeService : IQrCodeService
{
    public string Generer(ushort _grandeurQrCode, Uri _uri)
    {
        if (_grandeurQrCode is 0)
            _grandeurQrCode = 1;

        if (_grandeurQrCode > 20)
            _grandeurQrCode = 20;

        PayloadGenerator.Url url = new(_uri.OriginalString);

        using QRCodeGenerator qrGenerator = new();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new(qrCodeData);

        string qrCodeBase64 = Convert.ToBase64String(qrCode.GetGraphic(_grandeurQrCode));

        return qrCodeBase64;
    }

    public string Generer(ushort _grandeurQrCode, string _message)
    {
        if (_grandeurQrCode is 0)
            _grandeurQrCode = 1;

        if (_grandeurQrCode > 20)
            _grandeurQrCode = 20;

        if (string.IsNullOrWhiteSpace(_message))
            _message = "";

        using QRCodeGenerator qrGenerator = new();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(_message, QRCodeGenerator.ECCLevel.Q, true);
        using PngByteQRCode qrCode = new(qrCodeData);

        string qrCodeBase64 = Convert.ToBase64String(qrCode.GetGraphic(_grandeurQrCode));

        return qrCodeBase64;
    }
}