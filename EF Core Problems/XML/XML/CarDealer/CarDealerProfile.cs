using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Supplier

            this.CreateMap<ImportSuppliersDTO, Supplier>();

            this.CreateMap<Supplier, ExportLocalSuppliersDTO>()
                .ForMember(d => d.PartsCount,
                                        opt => opt.MapFrom(s => s.Parts.Count));          

            // Part

            this.CreateMap<ImportPartsDto, Part>();

            this.CreateMap<Part, PartDTO>();
         
            //Car

            this.CreateMap<ImportCarsDTO, Car>();

            this.CreateMap<Car, ExportCarsDto>();

            this.CreateMap<Car, ExportBmwCarDTO>();

            this.CreateMap<Car, ExportCarsWithPartsDTO>()
                .ForMember(d => d.Parts,
                                    opt => opt.MapFrom(s => s.PartsCars.Select(pc => pc.Part)
                                    .OrderByDescending(p => p.Price)
                                    .ToArray()));
                
            //Customers

            this.CreateMap<ImportCustomers, Customer>()
                .ForMember(d => d.BirthDate,
                                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            this.CreateMap<Customer, ExportTotalSalesByCustomerDTO>()
                .ForMember(d => d.CountCars,
                                       opt => opt.MapFrom(s => s.Sales.Count));
               
                                        
                                        
                
                                     
                
            // Sales

            this.CreateMap<ImportSalesDTO, Sale>();
              
        }
    }
}
