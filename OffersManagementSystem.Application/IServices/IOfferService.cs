﻿using OffersManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Application.IServices
{
    public interface IOfferService
    {
        Task<IEnumerable<Offer>> GetAllOffersAsync(string? serial, string? offerAddress, DateOnly? fromDate, DateOnly? toDate, bool? active, decimal? totalFrom, decimal? totalTo);
        Task<Offer> GetOfferByIdAsync(long id);
        Task<int> CreateOfferAsync(Offer offer);
        Task<int> UpdateOfferAsync(Offer offer);
        Task<int> DeleteOfferAsync(int id);
    }
}
