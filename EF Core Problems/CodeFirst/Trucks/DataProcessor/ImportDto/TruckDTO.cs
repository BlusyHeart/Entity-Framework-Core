using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckDTO
    {
        [XmlElement("RegistrationNumber")]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression(@"[A-Z]{2}\d{4}[A-Z]{2}")]
        public string? RegistrationNumber { get; set; }

      
        [XmlElement("VinNumber")]
        [MinLength(17)]
        [MaxLength(17)]
        [Required]
        public string? VinNumber { get; set; }
       
        [XmlElement("TankCapacity")]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

       
        [XmlElement("CategoryType")]
        [Range(0, 3)]
        public int CategoryType { get; set; }

       
        [XmlElement("MakeType")]
        [Range(0, 4)]
        public int MakeType { get; set; }
    }
}
