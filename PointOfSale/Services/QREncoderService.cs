using QRCoder;
using System.Drawing.Imaging;

namespace PointOfSale.Services
{
    public class QREncoderService
    {
        public static Byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCodeImage = new BitmapByteQRCode();
            qrCodeImage.SetQRCodeData(qrCodeData);
            
            return qrCodeImage.GetGraphic(10);
        }
    }
}
