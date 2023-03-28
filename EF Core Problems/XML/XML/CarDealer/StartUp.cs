using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            
            string result = GetTotalSalesByCustomer(context);

            Console.WriteLine(result);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var Sales = context.Sales
               .Select(s => new ExportSalesDTO
               {
                   Car = new CarDTO
                   {
                       Make = s.Car.Model,
                       Model = s.Car.Make,
                       TraveledDistance = s.Car.TraveledDistance
                   },

                   Discount = s.Discount,
                   CustomerName = s.Customer.Name,
                   Price = s.Car.PartsCars.Sum(cp => cp.Part.Price),
                   PriceWithDiscount = s.Car.PartsCars.Sum(cp => cp.Part.Price) - 
                                                    (s.Car.PartsCars.Sum(cp => cp.Part.Price) * (s.Discount / 100))

               })
               .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("sales");

            string result = xmlHelper.Serialize(rootAttribute, Sales);

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            IMapper mapper = InitializeAutoMapper();

            var totalSales = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new ExportTotalSalesByCustomerDTO
                {
                    Name = c.Name,
                    CountCars = c.Sales.Count,
                    TotalCost = c.Sales.Select(x => x.Car)
                                       .SelectMany(x => x.PartsCars)
                                       .Sum(x => x.Part.Price)
                })
                .OrderByDescending(c => c.TotalCost)
                .AsNoTracking()
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("customers");

            string result = xmlHelper.Serialize(rootAttribute, totalSales);

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();

            var carsParts = context.Cars.
                 ProjectTo<ExportCarsWithPartsDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .AsNoTracking()
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("cars");

            string result = xmlHelper.Serialize(rootAttribute, carsParts);

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();

            var localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSuppliersDTO>(mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("suppliers");

            string result = xmlHelper.Serialize(rootAttribute, localSuppliers);

            return result;

        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            string targetMake = "BMW";

            IMapper mapper = InitializeAutoMapper();
          
            var carsBMW = context.Cars
                .Where(c => c.Make.ToUpper() == targetMake)
                .ProjectTo<ExportBmwCarDTO>(mapper.ConfigurationProvider)
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .AsNoTracking()
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("cars");

            string result = xmlHelper.Serialize(rootAttribute, carsBMW);

            return result;
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();
            
            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .ProjectTo<ExportCarsDto>(mapper.ConfigurationProvider)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .AsNoTracking()
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();  

            XmlRootAttribute rootAttribute = new XmlRootAttribute("cars");

            string result = xmlHelper.Serialize(rootAttribute, cars);

            return result;
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportSalesDTO[] salesDtos = xmlHelper.Deserialize<ImportSalesDTO[]>(inputXml, "Sales");

            ICollection<Sale> sales = new List<Sale>();    

            foreach (var sDto in salesDtos)
            {
                if (context.Cars.All(c => c.Id != sDto.CarId.Value))
                {
                    continue;
                }
                    
                Sale sale = mapper.Map<Sale>(sDto);
                sales.Add(sale);
            }

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCustomers[] customersDto = xmlHelper.Deserialize<ImportCustomers[]>(inputXml, "Customers");

            ICollection<Customer> validCustomers = new List<Customer>();

            foreach (var cDto in customersDto)
            {
                if (string.IsNullOrEmpty(cDto.Name)
                    || string.IsNullOrEmpty(cDto.BirthDate))
                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(cDto);
                validCustomers.Add(customer);   

            }

            context.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var carsDtos = xmlHelper.Deserialize<ImportCarsDTO[]>(inputXml, "Cars");

            Car car = mapper.Map<Car>(carsDtos);
            ICollection<Car> cars = new List<Car>();

            ICollection<PartCar> parts = new List<PartCar>();

            foreach (var carDto in carsDtos)
            {

                foreach (var partDto in carDto.PartsCars.DistinctBy(p => p.Id))
                {
                    if (!context.Parts.Any(p => p.Id == partDto.Id))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        PartId = partDto.Id
                    };

                    car.PartsCars.Add(partCar);

                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return "";

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var importPartsDto =
                xmlHelper.Deserialize<ImportPartsDto[]>(inputXml, "Parts");

            ICollection<Part> parts = new List<Part>();

            foreach (var pDto in importPartsDto)
            {

                if (!pDto.SupplierId.HasValue ||
                    !context.Suppliers.Any(s => s.Id == pDto.SupplierId))
                {
                    continue;
                }

              
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportSuppliersDTO[] suppliersDTO =
                xmlHelper.Deserialize<ImportSuppliersDTO[]>(inputXml, "Suppliers");

            ICollection<Supplier> importSuppliersDTOs = new HashSet<Supplier>();

            foreach (var spDto in suppliersDTO)
            {
                if (string.IsNullOrEmpty(spDto.Name))
                {
                    continue;
                }

                Supplier supplier = mapper.Map<Supplier>(spDto);

                importSuppliersDTOs.Add(supplier);
            }

            context.Suppliers.AddRange(importSuppliersDTOs);
            context.SaveChanges();

            return $"Successfully imported {importSuppliersDTOs.Count}";
        }

        private static IMapper InitializeAutoMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<CarDealerProfile>();

            }));
    }
}