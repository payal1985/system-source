using QRCoder;

namespace SSInventory.Web.Utilities
{
    /// <summary>
    /// QRcode Utilities
    /// </summary>
    public static class QRcodePayloadUtilities
    {
        /// <summary>
        /// Embed text into QRcode
        /// </summary>
        /// <param name="text"></param>
        /// <returns>byte array</returns>
        public static byte[] ToPngByteQRCode(this string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}
