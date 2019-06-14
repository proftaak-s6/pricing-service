using Microsoft.EntityFrameworkCore;
using Rekeningrijden.PricingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rekeningrijden.PricingService.Data
{
    public class LocationContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public LocationContext(DbContextOptions options) : base(options)
        {

        }
    }
}
