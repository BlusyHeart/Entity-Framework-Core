
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RealEstates.Data;
using RealEstates.Services;
using System.Text;
using System.Text.Unicode;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var db = new ApplicationDbContext();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Chose an option");
            Console.WriteLine("1. Property Search");
            Console.WriteLine("2. Districts info");
            Console.WriteLine("0. EXIT");

            bool parsed = int.TryParse(Console.ReadLine(), out int option);

            if (parsed && option == 0)
            {
                break;
            }

            if (parsed && (option >= 1 || option <= 2))
            {
                switch (option)
                {
                    case 1:
                        PropertySearch(db);
                        break;
                    case 2:
                        GetMostExpensiveDistricts(db);
                        break;
                }

                Console.WriteLine("Press any key to continiou...");
                Console.ReadKey();
            }
        }
    }

    private static void GetMostExpensiveDistricts(ApplicationDbContext db)
    {
        IDistrictsService districtsService = new DistrictService(db);
        var districts = districtsService.GetMostExpensiveDistricts(3);

        foreach (var d in districts)
        {
            Console.WriteLine($"{d.Name} => {d.AvgPricePerSquareMeter:f2} ({d.PropertiesTotalCount})");
        }
    }

    private static void PropertySearch(ApplicationDbContext db)
    {
        Console.WriteLine("Min price:");
        int minPrice = int.Parse(Console.ReadLine());
        Console.WriteLine("Max price:");
        int maxPrice = int.Parse(Console.ReadLine());
        Console.WriteLine("Min size:");
        int minSize = int.Parse(Console.ReadLine());
        Console.WriteLine("Max size:");
        int maxSize = int.Parse(Console.ReadLine());

        IProportiesService service = new PropertiesService(db);
        var properties = service.Search(minPrice, maxPrice, minSize, maxSize);

        foreach (var property in properties)
        {
            Console.WriteLine($"{property.DistrictName}; {property.BuildingType}; {property.PropertyType} => {property.Price} => {property.Size}");
        }
    }
}