using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class PropertiesService : IProportiesService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext appContext)
        {
            dbContext = appContext;
        }

        public void Add(string districtName, int floor, int maxFloor, int size, int yardSize, string propertyTypeName, string buildingTypeName, int price)
        {
            var Property = new Property
            {
                Size = size,
                Floor = floor <= 0 || floor > 255 ? null : (byte)floor,
                Price = floor <= 0 ? null : price,
                TotalFloors = maxFloor <= 0 || maxFloor > 255 ? null : (byte)maxFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
            };

            var district = dbContext.Districts.FirstOrDefault(d => d.Name == districtName);

            if (district == null)
            {
                district = new District { Name = districtName };
            }

            Property.District = district;

            var propertyType = dbContext.PropertTypes.FirstOrDefault(p => p.Name == propertyTypeName);

            if (propertyType == null)
            {
                propertyType = new PropertyTypes { Name = propertyTypeName };
            }

            Property.Type = propertyType;

            var buildingType = dbContext.BuildingTypes.FirstOrDefault(bt => bt.Name == buildingTypeName);

            if (buildingType == null)
            {
                buildingType = new BuildingType { Name = propertyTypeName };
            }

            Property.BuildingType = buildingType;

            dbContext.Properties.Add(Property);

            dbContext.SaveChanges();

        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties = dbContext
                .Properties.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.Size >= minSize && p.Size <= maxSize)
                .Select(x => new PropertyInfoDto
                {
                    Size= x.Size,
                    Price = x.Price ?? 0,
                    BuildingType = x.BuildingType.Name,
                    PropertyType = x.Type.Name,
                    DistrictName = x.District.Name,

                })
                .ToArray();

            return properties;
        }
    }
}
