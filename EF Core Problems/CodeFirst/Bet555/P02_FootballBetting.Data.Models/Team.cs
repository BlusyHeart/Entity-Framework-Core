using P02_FootballBetting.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [MaxLength(GlobalConstants.TEAM_NAME_MAX_LENGTH)]
        public string Name { get; set; } = null!;

        [MaxLength(GlobalConstants.LOGO_URL_MAX_LENGTH)]
        public string LogoUrl { get; set; } = null!;

        [MaxLength(GlobalConstants.INITIALS_MAX_LENGTH)]
        public string Initials { get; set; } = null!;

        public decimal Budget { get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }

        public virtual Color PrimaryKitColor { get; set; } = null!;

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }

        public virtual Color SecondaryKitColor { get; set; } = null!;

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public virtual Town Town { get; set; } = null!;

        [InverseProperty(nameof(Game.HomeTeam))]
        public virtual ICollection<Game> HomeGames { get; set; }

        [InverseProperty(nameof(Game.AwayTeam))]
        public virtual ICollection<Game> AwayGames { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
