using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRgenerator
{
    public class QRgenerator
    {
        public Stream  QrGenerator(string urlPerfil)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(urlPerfil, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
            var bitMap = bitmapByteQRCode.GetGraphic(20);

            using var ms = new MemoryStream();
            ms.Write(bitMap);

            return ms;
        }


    }
}
