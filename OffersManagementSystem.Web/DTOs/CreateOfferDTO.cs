namespace OffersManagementSystem.Web.DTOs
{
    public class CreateOfferDTO
    {
        public string Serial { get; set; }
        public string OfferAddress { get; set; }
        public DateTime OfferDate { get; set; }
        public int OfferDayes { get; set; }
        public decimal Total { get; set; }
        public string Details { get; set; }
    }
}
