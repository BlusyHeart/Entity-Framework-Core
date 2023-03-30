namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Xml.Linq;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var despetchers = context.Despatchers
                 .Where(t => t.Trucks.Count >= 1)                
                 .Select(d => new ExportDespatchers
                 {
                     DespatcherName = d.Name,
                     TrucksCount = d.Trucks.Count,
                     Trucks = d.Trucks.Select(t => new TruckDTO
                     {
                         RegistrationNumber = t.RegistrationNumber!,
                         Make = t.MakeType.ToString()
                     })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                 })
                 .OrderByDescending(d => d.Trucks.Count())
                 .ThenBy(d => d.DespatcherName)
                 .ToArray();

            return xmlHelper.Serialize("Despatchers", despetchers);
                
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
           var clients = context.Clients                
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new
                {
                    c.Name,
                    Trucks = c.ClientsTrucks
                    .Where(c => c.Truck.TankCapacity >= capacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        t.Truck.VinNumber,
                        t.Truck.TankCapacity,
                        t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString()
                    }).OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
               
        }
    }
}
