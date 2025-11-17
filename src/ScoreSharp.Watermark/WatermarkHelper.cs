using System.Numerics;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ScoreSharp.Watermark
{
    public class WatermarkHelper : IWatermarkHelper
    {
        public WatermarkHelper()
        {
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new FontResolver();
            }
        }

        public byte[] ImageWatermarkAndGetBytes(string text, string format, byte[] filebytes)
        {
            using (var memoryStream = new MemoryStream(filebytes))
            {
                using (var image = Image.Load<Rgba32>(memoryStream))
                {
                    // 套用浮水印
                    image.Mutate(ctx => ctx.ApplyWatermark(text, Color.Gray, 5, false));

                    // 選擇適當的格式
                    IImageEncoder encoder = format.ToLower() switch
                    {
                        ".png" => new PngEncoder(),
                        ".jpeg" or ".jpg" => new JpegEncoder { Quality = 90 },
                        ".bmp" => new BmpEncoder(),
                        ".gif" => new GifEncoder(),
                        _ => throw new ArgumentException("不支援的格式: " + format),
                    };

                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, encoder);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public void PdfWatermark(string text, string path, byte[] filebytes)
        {
            using (var document = PdfReader.Open(new MemoryStream(filebytes), PdfDocumentOpenMode.Modify))
            {
                foreach (var page in document.Pages)
                {
                    var gfx = XGraphics.FromPdfPage(page);

                    // 根據頁面大小動態計算字體大小（取寬高的最小值的一定比例）
                    double fontSize = Math.Min(page.Width, page.Height) * 0.15; // 15% 的頁面大小

                    //標楷體
                    var font = new XFont("DFKai-SB", fontSize, XFontStyle.Regular);
                    var textColor = new XSolidBrush(XColor.FromArgb(100, 128, 128, 128));

                    // 設定浮水印的位置 & 旋轉
                    gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                    gfx.RotateTransform(-45);

                    var textSize = gfx.MeasureString(text, font);
                    var layout = new XRect(-textSize.Width / 2, -textSize.Height / 2, textSize.Width, textSize.Height);
                    var format = XStringFormats.Center;

                    gfx.DrawString(text, font, textColor, layout, format);
                }

                document.Save(path);
            }
        }

        public byte[] PdfWatermarkAndGetBytes(string text, byte[] filebytes)
        {
            using (var document = PdfReader.Open(new MemoryStream(filebytes), PdfDocumentOpenMode.Modify))
            {
                foreach (var page in document.Pages)
                {
                    var gfx = XGraphics.FromPdfPage(page);

                    // 根據頁面大小動態計算字體大小（取寬高的最小值的一定比例）
                    double fontSize = Math.Min(page.Width, page.Height) * 0.15; // 15% 的頁面大小

                    //標楷體
                    var font = new XFont("DFKai-SB", fontSize, XFontStyle.Regular);
                    var textColor = new XSolidBrush(XColor.FromArgb(100, 128, 128, 128));

                    // 設定浮水印的位置 & 旋轉
                    gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                    gfx.RotateTransform(-45);

                    var textSize = gfx.MeasureString(text, font);
                    var layout = new XRect(-textSize.Width / 2, -textSize.Height / 2, textSize.Width, textSize.Height);
                    var format = XStringFormats.Center;

                    gfx.DrawString(text, font, textColor, layout, format);
                }

                using (var outputStream = new MemoryStream())
                {
                    document.Save(outputStream, false); // `false` 代表不關閉 stream
                    return outputStream.ToArray();
                }
            }
        }
    }

    public static class WatermarkHelperExtensions
    {
        public static IImageProcessingContext ApplyWatermark(
            this IImageProcessingContext processingContext,
            string text,
            Color color,
            float padding,
            bool _
        )
        {
            // 1. 獲取圖片尺寸
            Size imageSize = processingContext.GetCurrentSize();

            // 2. 設置文字的基礎大小，取圖片寬高中較小的10%作為字體大小
            float fontSize = Math.Min(imageSize.Width, imageSize.Height) * 0.1f;
            Font font = SystemFonts.CreateFont("Microsoft JhengHei", fontSize);

            // 3. 測量文字的尺寸
            var textSize = TextMeasurer.MeasureSize(text, new TextOptions(font));

            // 4. 計算圖片中心點
            float centerX = imageSize.Width / 2;
            float centerY = imageSize.Height / 2;

            // 5. 計算旋轉角度（45度）
            float angle = (float)(Math.PI / 4);

            // 6. 設置文字選項，將 `Origin` 設為圖片中心
            var textOptions = new RichTextOptions(font)
            {
                Origin = new PointF(0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Dpi = 96,
            };

            // 7. 設置繪圖選項，包含旋轉和居中位置
            var drawingOptions = new DrawingOptions
            {
                GraphicsOptions = new GraphicsOptions { Antialias = true, BlendPercentage = 0.6f },
                ShapeOptions = new ShapeOptions { IntersectionRule = IntersectionRule.NonZero },
                Transform =
                    Matrix3x2.CreateTranslation(-textSize.Width / 2, -textSize.Height / 2)
                    * // 將文字移到中心點
                    Matrix3x2.CreateRotation(-angle)
                    * // 旋轉文字
                    Matrix3x2.CreateTranslation(centerX + textSize.Width * 0.4f, centerY - textSize.Height * 3), // 移動到圖片中心
            };

            // 8. 繪製浮水印
            return processingContext.DrawText(drawingOptions, textOptions, text, Brushes.Solid(color), null);
        }
    }
}
