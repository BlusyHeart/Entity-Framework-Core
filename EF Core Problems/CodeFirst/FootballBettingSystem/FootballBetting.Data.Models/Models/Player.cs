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
    public class Player
    {
        public Player()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistics>();    
        }

        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PlayerNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PlayerMaxSquadNumberLength)]
        public int SquadNumber { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        public Teams Team { get; set; }

        [Required]
        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        public Position Position { get; set; }

        [Required]
        public bool IsInjured { get; set; }

        public ICollection<PlayerStatistics> PlayerStatistics { get; set; }   

    }
}
