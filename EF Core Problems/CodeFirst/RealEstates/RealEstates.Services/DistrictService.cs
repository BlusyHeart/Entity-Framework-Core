using RealEstates.Data;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class DistrictService : IDistrictsService
    {
        private readonly ApplicationDbContext dbContext;
        public DistrictService(ApplicationDbContext db)
        {
            dbContext = db;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = dbContext.Districts.Select(x => new DistrictInfoDto
            {
                Name = x.Name,
                PropertiesTotalCount = x.Properties.Count,
                AvgPricePerSquareMeter = x.Properties
                .Where(x => x.Price.HasValue)
                    .Average(x => x.Price / (decimal)x.Size) ?? 0,
            })
                .OrderByDescending(x => x.AvgPricePerSquareMeter)
                .Take(count)
                .ToArray();

            return districts;
        }
    }
}
