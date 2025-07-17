using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OffersManagementSystem.Domain.Entities;
using OffersManagementSystem.Domain.EntitiesConfigrations;
using OffersManagementSystem.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppIdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new OfferConfig());
        }

        public DbSet<Offer> Offers { get; set; }


    }
}
