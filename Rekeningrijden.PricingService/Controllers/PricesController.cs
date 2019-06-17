using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rekeningrijden.PricingService.Config;
using Rekeningrijden.PricingService.Data;
using Rekeningrijden.PricingService.Model;
using Rekeningrijden.PricingService.Service;

namespace Rekeningrijden.PricingService.Controllers
{
    [Route("prices")]
    public class PricesController : Controller
    {
        private readonly ILocationService locationService;

        public PricesController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var locations = locationService.GetAll();

            if (!locations.Any())
            {
                return NoContent();
            }

            return Ok(locations);
        }

        [HttpGet("{name}")]
        public IActionResult GetPriceHistoryByName(string name)
        {
            var location = locationService.GetOrCreate(name);

            return Ok(location.PriceHistory);
        }

        [HttpGet("{name}/{date}")]
        public IActionResult GetPriceForLocationAtTime(string name, DateTime date)
        {
            var location = locationService.GetOrCreate(name);
            double price = GetPriceAtTime(location.PriceHistory, date);

            return Ok(new { Price = price });
        }


        [HttpPut("{name}")]
        public IActionResult AddPriceHistoryEntry(string name, [FromBody] PriceHistoryEntry newEntry)
        {
            // Check if a date is provided, otherwise use the current time.
            newEntry.Date = newEntry.Date == DateTime.MinValue 
                ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Europe/Amsterdam") 
                : newEntry.Date;

            try
            {
                locationService.AddHistoryEntry(name, newEntry);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private static double GetPriceAtTime(ICollection<PriceHistoryEntry> priceHistory, DateTime date)
        {
            // Try getting the first price notation at (or before) the given date and time.
            var sorted = priceHistory.Where(p => p.Date <= date).OrderBy(p => Math.Abs((p.Date - date).Ticks));
            var closest = sorted.FirstOrDefault();

            if (closest != null)
            {
                return closest.Price;
            }

            // If no price notation existed before or at the given date and time, return first entry.
            sorted = priceHistory.Where(p => p.Date >= date).OrderBy(p => Math.Abs((p.Date - date).Ticks));
            closest = sorted.First();

            return closest.Price;
        }
    }
}
