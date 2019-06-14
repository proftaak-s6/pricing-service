using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rekeningrijden.PricingService.Model
{
    public class PriceHistoryEntry
    {
        [JsonIgnore]
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
