using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using ZXing;
using ZXing.QrCode;
using ZXing.SkiaSharp.Rendering;
using ZXing.Windows.Compatibility;

namespace SSInventory.Web.Utilities
{
    /// <summary>
    /// Barcode utilities
    /// </summary>
    public static class BarcodeUtilities
    {
        public static SKData Encode(this string content)
        {
            var writer = new BarcodeWriter<SKBitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new QrCodeEncodingOptions
                {
                    Height = 200,
                    Width = 600,
                    PureBarcode = false,
                },
                Renderer = new SKBitmapRenderer() { TextSize = 13 }
            };

            var qrCodeImage = writer.Write(content);

            return qrCodeImage.Encode(SKEncodedImageFormat.Png, 100);
        }

        /// <summary>
        /// Generate barcode image with text
        /// </summary>
        /// <param name="content"></param>
        /// <param name="saveImagePath"></param>
        /// <remarks>
        /// PureBarcode: show text at the bottom
        /// </remarks>
        /// <returns>Memory stream array</returns>
        public static byte[] CreateBarCode(this string content, string saveImagePath)
        {
            try
            {
                var writer = new BarcodeWriter<SKBitmap>
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 200,
                        Width = 600,
                        PureBarcode = false
                    },
                    Renderer = new SKBitmapRenderer()
                };

                var qrCodeImage = writer.Write(content);

                var skEncodedData = qrCodeImage.Encode(SKEncodedImageFormat.Png, 80);
                var bitmapImageStream = File.Open(saveImagePath, FileMode.Create, FileAccess.Write, FileShare.None);
                skEncodedData.SaveTo(bitmapImageStream);
                bitmapImageStream.Flush(true);
                bitmapImageStream.Dispose();

            }
            catch
            {

            }
            return null;
        }

        /// <summary>
        /// Decode binary barcode array to text
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>The text inside barcode image</returns>
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("ios14.0")]
        public static string DecodeBarcode(this byte[] barcode)
        {
            BarcodeReader coreCompatReader = new BarcodeReader();

            using Stream stream = new MemoryStream(barcode);
            using var coreCompatImage = (Bitmap)Image.FromStream(stream);
            return coreCompatReader.Decode(coreCompatImage).Text;
        }

        /// <summary>
        /// Decode text inside bitmap image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>The text inside barcode image</returns>
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("ios14.0")]
        public static string DecodeBarcode(this Bitmap bitmap)
        {
            BarcodeReader coreCompatReader = new BarcodeReader();

            var result = coreCompatReader.Decode(bitmap);
            if (result is null || string.IsNullOrWhiteSpace(result.Text))
                return null;

            return result.Text;
        }
    }
}
