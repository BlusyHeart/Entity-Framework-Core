namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            var importDTOs = xmlHelper.Deserialize<ImportDespatchers[]>(xmlString, "Despatchers");

            ICollection<Despatcher> validDespetchers = new HashSet<Despatcher>();

            foreach (var dto in importDTOs)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                ICollection<Truck> validTrucks = new HashSet<Truck>();

                foreach (var t in dto.Trucks)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage); continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = (CategoryType)t.CategoryType,
                        MakeType = (MakeType)t.MakeType
                    };

                    validTrucks.Add(truck);
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position,
                    Trucks = validTrucks
                };

                validDespetchers.Add(despatcher);
                sb.AppendLine($"Successfully imported despatcher - {despatcher.Name} with {despatcher.Trucks.Count} trucks.");

            }

                context.AddRange(validDespetchers);
                context.SaveChanges();
            
            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var importDTOs = JsonConvert.DeserializeObject<ImportClients[]>(jsonString);

            ICollection<Client> validClients = new HashSet<Client>();
            ICollection<int> validTrucksIds = new HashSet<int>();

            foreach (var dto in importDTOs)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Type = dto.Type,
                };

                validTrucksIds = context.Trucks.Select(t => t.Id).ToArray();
                foreach (int id in dto.Trucks.Distinct())
                {
                    if (!validTrucksIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }

                    ClientTruck clientTruck = new ClientTruck()
                    {
                        Client = client,
                        TruckId = id
                    };

                    client.ClientsTrucks.Add(clientTruck);
                }

                validClients.Add(client);
                sb.AppendLine($"Successfully imported client - {client.Name} with {client.ClientsTrucks.Count} trucks.");
            }
           
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}