using FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace FootballBetting.Data.Models.Models
{
    public class Teams
    {

        public Teams()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }
      
        public decimal Budget { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamInitialsMaxLength)]
        public string Initials { get; set; }

        [MaxLength(GlobalConstants.TeamLogicUrlMaxLength)]
        public string LogoUrl { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]  
        public int PrimaryKitColorId { get; set; }

        public virtual Color PrimaryKitColor { get; set; }


        [ForeignKey(nameof(SecondoryKitColor))]
        public int SecondoryKitColorId { get; set; }

        public virtual Color SecondoryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }


        [InverseProperty(nameof(Game.HomeTeam))]
        public ICollection<Game> HomeGames { get; set; }

        [InverseProperty(nameof(Game.AwayTeam))]
        public ICollection<Game> AwayGames { get; set; }

        public virtual ICollection<Player> Players { get; set; }

    }
}