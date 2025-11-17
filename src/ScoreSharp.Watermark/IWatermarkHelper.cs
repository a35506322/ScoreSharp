namespace ScoreSharp.Watermark;

public interface IWatermarkHelper
{
    public void PdfWatermark(string text, string path, byte[] filebytes);
    public byte[] PdfWatermarkAndGetBytes(string text, byte[] filebytes);
    public byte[] ImageWatermarkAndGetBytes(string text, string format, byte[] filebytes);
}
