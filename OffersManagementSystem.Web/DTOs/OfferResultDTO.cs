using OffersManagementSystem.Domain.Entities;
using QRCoder;

namespace OffersManagementSystem.Web.DTOs
{
    public class OfferResultDTO : Offer
    {
        public DateTime ExpirationDate 
        {
            get
            {
                return OfferDate.AddDays(OfferDayes);
            }
        }

        public int DayesToExpire 
        {
            get
            {
                return (ExpirationDate - DateTime.Now).Days + 1;
            }
        }

        public bool IsExpired
        {
            get
            {
                return DateTime.Now > ExpirationDate;
            }
        }

        public string SerialQrCode 
        {
            get
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(Serial, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                var qrBytes = qrCode.GetGraphic(20);
                return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
            } 
        }
    }
}
