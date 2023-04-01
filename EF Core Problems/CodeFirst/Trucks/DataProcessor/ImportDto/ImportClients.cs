using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ImportDto
{
    
    public class ImportClients
    {
        [JsonProperty(nameof(Name))]
        [MinLength(3)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        [JsonProperty(nameof(Nationality))]
        public string Nationality { get; set; }

        [JsonProperty(nameof(Type))]       
        [Required]
        public string Type { get; set; }

        [JsonProperty(nameof(Trucks))]        
        public int[] Trucks { get; set; }
    }
}
