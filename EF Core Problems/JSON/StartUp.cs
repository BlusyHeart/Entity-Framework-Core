using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProducts;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.Users;
using ProductShop.Models;
using AutoMapper.QueryableExtensions;
using ProductShop.DTOs.Products;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;

namespace ProductShop
{
   
    public class StartUp
    {
        private static string filePath;

        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));
            
            ProductShopContext dbcontext = new ProductShopContext();

            string json = GetUsersWithProducts(dbcontext);

            InitializeOutputFilePath("users-and-products.json");

            File.WriteAllText(filePath, json);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users.Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderByDescending(u => u.ProductsSold.Count)
                .ProjectTo<ExportUsersAndProductsDTO>()
                .ToArray();


            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var resultJson = JsonConvert.SerializeObject(users, Formatting.Indented, jsonSerializerSettings);
            return resultJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProducts = c.CategoryProducts.Count(),
                    AverageProductPrice = c.CategoryProducts.Count == 0 ? 0 : c.CategoryProducts.Average(p => p.Product.Price),
                    TotalProductPrice = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("F2")
                })
                .OrderByDescending(c => c.TotalProducts)
                .ToArray();

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUserSoldProducts[] users = context
                .Users.Where(u => u.ProductsSold.Count >= 1)
                       .OrderBy(u => u.LastName)
                       .ThenBy(u => u.FirstName)
                       .ProjectTo<ExportUserSoldProducts>()
                       .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        private static void InitializeDatasetFilePath(string fileName)
        {
            filePath =  Path.Combine(Directory.GetCurrentDirectory(), "../../../Datasets/", fileName);
        }

        private static void InitializeOutputFilePath(string fileName)
        {
             filePath =  Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", fileName);
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            int minRange = 500;
            int maxRange = 1000;

            ExportProducts[] products = context
                .Products.Where(p => p.Price >= minRange && p.Price <= maxRange)
                         .OrderBy(p => p.Price)
                         .ProjectTo<ExportProducts>()
                         .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);          
                         
            return json;              
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductsDTO[] categoryProductDTO = JsonConvert.DeserializeObject<ImportCategoryProductsDTO[]>(inputJson);

            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (ImportCategoryProductsDTO cDto in categoryProductDTO)
            {

                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(cDto);
                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDTO[] productsDTO = JsonConvert.DeserializeObject<ImportCategoryDTO[]>(inputJson);

            ICollection<Category> categories = new List<Category>();

            foreach (ImportCategoryDTO cDto in productsDTO)
            {
                if (!IsValid(cDto))
                {
                    continue;
                }

                Category category = Mapper.Map<Category>(cDto);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductsDTO[] productsDTO = JsonConvert.DeserializeObject<ImportProductsDTO[]>(inputJson);

            ICollection<Product> products = new List<Product>();

            foreach (ImportProductsDTO pDto in productsDTO)
            {
                products.Add(Mapper.Map<Product>(pDto));
            }

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDTO[] userDTOs = JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson);
          
            ICollection<User> users = new List<User>();

            foreach (ImportUserDTO uDto in userDTOs)
            {               
                users.Add(Mapper.Map<User>(uDto));
            }

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

    }
}