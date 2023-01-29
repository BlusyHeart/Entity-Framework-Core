using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs.CategoryProducts
{
    [JsonObject]
    public class ExportCategoriesByProductDTO
    {
        [JsonProperty("category")]
        public string Name { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public decimal AvgPrice { get; set; }

        [JsonProperty("totalRevenue")]
        public decimal TotalIncome { get; set; }
    }
}
