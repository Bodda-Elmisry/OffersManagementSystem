using OffersManagementSystem.Application.IRepositories;
using OffersManagementSystem.Application.IServices;
using OffersManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Infrastructure.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            this._offerRepository = offerRepository;
        }

        public async Task<int> CreateOfferAsync(Offer offer)
        {
            return await _offerRepository.CreateOfferAsync(offer);
        }

        public async Task<int> DeleteOfferAsync(int id)
        {
            return await _offerRepository.DeleteOfferAsync(id);
        }

        public async Task<IEnumerable<Offer>> GetAllOffersAsync(string? serial, string? offerAddress, DateOnly? fromDate, DateOnly? toDate, bool? active, decimal? totalFrom, decimal? totalTo)
        {
            return await _offerRepository.GetAllOffersAsync(serial, offerAddress, fromDate, toDate, active, totalFrom, totalTo);
        }

        public async Task<Offer> GetOfferByIdAsync(long id)
        {
            return await _offerRepository.GetOfferByIdAsync(id);
        }

        public async Task<int> UpdateOfferAsync(Offer offer)
        {
            return await _offerRepository.UpdateOfferAsync(offer);
        }
    }
}
