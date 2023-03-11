using P02_FootballBetting.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
            PlayerStatistic = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int PlayerId { get; set; }

        [MaxLength(GlobalConstants.PLAYER_MAX_NAME)]
        public string Name { get; set; } = null!;

        public int SquadNumber { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        public virtual Position Position { get; set; } = null!;

        public bool IsInjured { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistic { get; set; }
    }
}
