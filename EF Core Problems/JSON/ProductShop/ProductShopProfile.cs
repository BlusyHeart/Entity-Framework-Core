using AutoMapper;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProducts;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.Products;
using ProductShop.DTOs.Users;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDTO, User>();

            this.CreateMap<ImportProductsDTO, Product>();

            this.CreateMap<ImportCategoryDTO, Category>();

            this.CreateMap<ImportCategoryProductsDTO, CategoryProduct>();

            this.CreateMap<Product, ExportProducts>()
                .ForMember(d => d.Seller, mo => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            this.CreateMap<Product, ProductDTO>()
                .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName));

            this.CreateMap<Product, ProductDTO>()
                .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));

            this.CreateMap<User, ExportUserSoldProducts>()
                .ForMember(d => d.ProductsSold, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            this.CreateMap<User, ExportUsersAndProductsDTO>()
                .ForMember(d => d.UsersCount, mo => mo.MapFrom(s => s.ProductsSold.Count()));

            this.CreateMap<User, ExportUsersDTO>();

            this.CreateMap<Product, UserProductDTO>();               

            this.CreateMap<ExportUsersDTO, ExportUsersAndProductsDTO>()
                .ForMember(d => d.Users, mo => mo.MapFrom(s => s.ProductsSold.Where(ps => ps.BuyerId != null)));
        }
    }
}
