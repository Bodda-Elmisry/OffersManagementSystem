namespace OffersManagementSystem.Web.DTOs
{
    public class OffersFilterDTO
    {
        public string? Serial { get; set; }
        public string? OfferAddress { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public bool? Active { get; set; }
        public decimal? TotalFrom { get; set; }
        public decimal? TotalTo { get; set; }
    }
}
