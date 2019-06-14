using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rekeningrijden.PricingService.Config;
using Rekeningrijden.PricingService.Data;
using Rekeningrijden.PricingService.Model;

namespace Rekeningrijden.PricingService.Service
{
    public class LocationService : ILocationService
    {
        private readonly LocationContext context;
        private AppConfig config;

        public LocationService(LocationContext context, IOptions<AppConfig> config)
        {
            this.context = context;
            this.config = config.Value;
        }

        public IEnumerable<Location> GetAll()
        {
            var locations = context.Locations.Include(l => l.PriceHistory);

            // Order price histories on date.
            foreach (var location in locations)
            {
                location.PriceHistory = location.PriceHistory.OrderByDescending(p => p.Date).ToList();
            }

            return locations;
        }

        public Location GetOrCreate(string name, bool populateDefaultHistory = true)
        {
            var location = context.Locations
                .Include(l => l.PriceHistory)
                .FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase));

            if (location == null)
            {
                location = Add(populateDefaultHistory ? Location.CreateWithHistory(name, config.DefaultPrice) : Location.Create(name));
            }

            location.PriceHistory = location.PriceHistory.OrderByDescending(p => p.Date).ToList();

            return location;
        }

        public Location Add(Location location)
        {
            var addedEntity = context.Locations.Add(location);
            context.SaveChanges();

            return addedEntity.Entity;
        }

        public void AddHistoryEntry(string name, PriceHistoryEntry entry)
        {
            var location = GetOrCreate(name, false);
            location.PriceHistory.Add(entry);

            context.SaveChanges();
        }
    }
}