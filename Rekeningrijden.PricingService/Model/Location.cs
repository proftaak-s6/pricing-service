using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rekeningrijden.PricingService.Model
{
    public class Location
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Name { get; set; }

        public virtual ICollection<PriceHistoryEntry> PriceHistory { get; set; }

        public static Location CreateWithHistory(string name, double price)
        {
            var location = Create(name);

            location.PriceHistory = new[]
            {
                new PriceHistoryEntry
                {
                    Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Europe/Amsterdam"),
                    Price = price
                }
            };

            return location;
        }

        public static Location Create(string name)
        {
            return new Location
            {
                Name = name,
                PriceHistory = new List<PriceHistoryEntry>()
            };
        }
    }
}
