using FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Data.Models.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ColorNameMaxLength)]
        public string Name { get; set; }

        [InverseProperty(nameof(Teams.PrimaryKitColor))]
        public virtual ICollection<Teams> PrimaryKitTeams{ get; set; }

        [InverseProperty(nameof(Teams.SecondoryKitColor))]
        public virtual ICollection <Teams> SecondoryKitTeams { get; set; }
    }
}
