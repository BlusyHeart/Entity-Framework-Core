using Newtonsoft.Json;
using ProductShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductShop.DTOs.Users
{
    [JsonObject]
    public class ImportUserDTO
    {

        [JsonProperty(nameof(FirstName))]
        public string FirstName { get; set; }

        [JsonProperty(nameof(LastName))]       
        public string LastName { get; set; }

        [JsonProperty(nameof(Age))]
        public int? Age { get; set; }
    }
}
