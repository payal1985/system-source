using SkiaSharp;
using System.IO;

namespace SSInventory.Web.Utilities
{
    public static class ImageUtilities
    {
        public static void AddTextToBitmap(string imageUrl, string text)
        {
            //var bitmap = SkiaSharp.SKBitmap.Decode(imageUrl);

            var resizeFactor = 1;
            var bitmap = SKBitmap.Decode(imageUrl);
            var toBitmap = new SKBitmap(bitmap.Width, bitmap.Height + 30, bitmap.ColorType, bitmap.AlphaType);

            var canvas = new SKCanvas(toBitmap);
            // Draw a bitmap rescaled
            canvas.SetMatrix(SKMatrix.CreateScale(resizeFactor, resizeFactor));
            canvas.DrawBitmap(bitmap, 0, 0);
            canvas.ResetMatrix();

            var font = SKTypeface.FromFamilyName("Arial");
            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = 13,
                IsAntialias = true,
                Color = new SKColor(0, 0, 0, 100)
            };
            canvas.DrawText(text, (bitmap.Height * resizeFactor / 2.0f) + 130, 220, brush);

            canvas.Flush();

            var image = SKImage.FromBitmap(toBitmap);
            var data = image.Encode(SKEncodedImageFormat.Png, 100);

            using (var stream = new FileStream(imageUrl, FileMode.Create, FileAccess.Write))
                data.SaveTo(stream);

            data.Dispose();
            image.Dispose();
            canvas.Dispose();
            brush.Dispose();
            font.Dispose();
            toBitmap.Dispose();
            bitmap.Dispose();
        }
    }
}
