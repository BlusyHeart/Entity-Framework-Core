using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealEstates.Importer
{
    public class Program
    {
        static void Main()
        {
            string jsonPath = "../../../imot.bg-raw-data-2021-03-18.json";

            ImportJsonFile(jsonPath);
        }

        private static void ImportJsonFile(string jsonPath)
        {
            var dbContext = new ApplicationDbContext();
           
            var json = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(jsonPath));

            IProportiesService propertyService = new PropertiesService(dbContext);

            foreach (var property in json)
            {
                string district = property.District;
                int floor = property.Floor;
                int maxFloor = property.TotalFloors;
                int size = property.Size;
                int yeardSize = property.YardSize;
                string propertyType = property.Type;
                string buildingType = property.BuildingType;
                int price = property.Price;

                propertyService.Add(district, floor, maxFloor, size, yeardSize, propertyType, buildingType, price);
                Console.Write(".");
            }
        }
    }
}
