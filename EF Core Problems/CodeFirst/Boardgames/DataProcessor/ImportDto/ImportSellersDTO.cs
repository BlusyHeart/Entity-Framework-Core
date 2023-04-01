using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{    
    public class ImportSellersDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Address))]
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Address { get; set; } = null!;

        [JsonProperty(nameof(Country))]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty(nameof(Website))]
        [Required]
        [RegularExpression(@"[www.]{4}[A-Za-z0-9-]*.com")]
        public string Website { get; set; } = null!;

        [JsonProperty(nameof(Boardgames))]
        public int[] Boardgames { get; set; } = null!;
    }
}
