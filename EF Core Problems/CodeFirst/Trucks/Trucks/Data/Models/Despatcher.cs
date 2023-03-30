
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new HashSet<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        public string? Position { get; set; }

        public virtual ICollection<Truck> Trucks  { get; set; }

    }
}
