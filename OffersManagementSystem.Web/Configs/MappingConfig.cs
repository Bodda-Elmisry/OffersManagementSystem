using Mapster;
using OffersManagementSystem.Domain.Entities;
using OffersManagementSystem.Web.DTOs;

namespace OffersManagementSystem.Web.Configs
{
    public static class MappingConfig
    {
        public static void MapOffersConfig()
        {
            TypeAdapterConfig<Offer, OfferResultDTO>.NewConfig();
            TypeAdapterConfig<CreateOfferDTO, Offer>.NewConfig();
        }
    }
}
