using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Data.Models.Models
{
    // Mapping class
    public class PlayerStatistics
    {
        [Key]
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        [Key]
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        [Required]
        public byte ScoredGoals { get; set; }

        [Required]
        public byte Assists { get; set; }

        [Required]
        public byte MinutesPlayed { get; set; }
    }
}
