
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.MAX_REGISTRATION_NUMBER)]
        public string? RegistrationNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MAX_LENGTH_VIN_NUMBER)]
        public string VinNumber { get; set; } = null!;

        public int TankCapacity  { get; set; }
       
        public int CargoCapacity  { get; set; }
       
        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId  { get; set; }

        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks  { get; set; }

    }
}
