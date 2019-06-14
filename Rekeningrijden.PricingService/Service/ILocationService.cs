using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rekeningrijden.PricingService.Model;

namespace Rekeningrijden.PricingService.Service
{
    public interface ILocationService
    {
        IEnumerable<Location> GetAll();
        Location GetOrCreate(string name, bool populateDefaultHistory = true);
        Location Add(Location location);
        void AddHistoryEntry(string name, PriceHistoryEntry entry);
    }
}
