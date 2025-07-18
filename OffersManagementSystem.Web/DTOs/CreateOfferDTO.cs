using System.ComponentModel.DataAnnotations;

namespace OffersManagementSystem.Web.DTOs
{
    public class CreateOfferDTO
    {
        [Required(ErrorMessage = "Serial is required")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "Offer address is required")]
        public string OfferAddress { get; set; }
        
        [Required(ErrorMessage = "Offer date is required")]
        [DataType(DataType.Date)]
        public DateTime OfferDate { get; set; }
        public int OfferDayes { get; set; }
        public decimal Total { get; set; }
        
        [Required(ErrorMessage = "Details is required")]
        public string Details { get; set; }
    }
}
