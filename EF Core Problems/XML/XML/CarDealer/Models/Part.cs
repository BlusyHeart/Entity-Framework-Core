using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class Part
    {
        public Part()
        {
            PartsCars = new HashSet<PartCar>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!; 

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; } = null!;

        public virtual ICollection<PartCar> PartsCars { get; set; }
    }
}
