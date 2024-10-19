namespace Services.QrCodes;

public interface IQrCodeService
{
    /// <summary>
    /// Créer un qrCode en base64 avec un message
    /// </summary>
    /// <param name="_grandeurQrCode">de 1 à 20 (très petit à très grand)</param>
    /// <param name="_message">Message dans le qr code</param>
    /// <returns>QR code en base64</returns>
    string Generer(ushort _grandeurQrCode, string _message);

    /// <summary>
    /// Créer un qrCode en base64 avec une Url
    /// </summary>
    /// <param name="_grandeurQrCode">de 1 à 20 (très petit à très grand)</param>
    /// <param name="_uri">Url</param>
    /// <returns>QR code en base64</returns>
    string Generer(ushort _grandeurQrCode, Uri _uri);
}