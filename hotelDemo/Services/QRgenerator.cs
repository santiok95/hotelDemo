using QRCoder;

namespace hotelDemo.Services;

public static class QRgenerator
{
    public static Stream QrGenerator(string code)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
        var bitMap = bitmapByteQRCode.GetGraphic(20);

        var ms = new MemoryStream();
        ms.Write(bitMap);
        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }


}
