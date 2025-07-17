using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Domain.EntitiesConfigrations
{
    public class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Serial)
                .HasMaxLength(50);

            builder.Property(x=> x.OfferAddress)
                .HasMaxLength(200);

            builder.Property(x => x.Total)
                .HasPrecision(18, 2);

        }
    }
}
