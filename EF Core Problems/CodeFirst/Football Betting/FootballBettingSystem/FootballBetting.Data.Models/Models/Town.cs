using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Data.Models.Models
{
    public class Town
    {

        public Town()
        {
            this.Teams = new HashSet<Teams>();
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength()]
        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public Country Country { get; set; }
       
        public virtual ICollection<Teams> Teams { get; set; }
    }
}
