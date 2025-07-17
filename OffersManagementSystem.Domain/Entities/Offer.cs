using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Domain.Entities
{
    public class Offer
    {
        public long Id { get; set; }
        public string Serial { get; set; }
        public string OfferAddress { get; set; }
        public DateOnly OfferDate { get; set; }
        public int OfferDayes { get; set; }
        public decimal Total { get; set; }
        public string Details { get; set; }
        public string FilePath { get; set; }
    }
}
