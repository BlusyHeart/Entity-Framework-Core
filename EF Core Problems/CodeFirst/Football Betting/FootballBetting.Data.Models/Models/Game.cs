using FootballBetting.Data.Common;
using FootballBetting.Data.Models.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBetting.Data.Models.Models
{
    public class Game
    {

        public Game()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistics>();
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int GameId { get; set; }

        [ForeignKey(nameof(HomeTeam))]

        public int HomeTeamId { get; set; }

        public virtual Teams HomeTeam { get; set; }

        [ForeignKey(nameof(AwayTeam))]

        public int AwayTeamId { get; set; }

        public virtual Teams AwayTeam { get; set; }

        [Required]
        public byte HomeTeamGoals { get; set; }

        [Required]
        public byte AwayTeamGoals { get; set; }
     
        public DateTime GameDate { get; set; }

        [Required]
        public double HomeTeamBetRate { get; set; }

        [Required]
        public double AwayTeamBetRate { get; set; }

        [Required]
        public double DrawBetRate { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ResultMaxLength)]

        public virtual ICollection<PlayerStatistics> PlayerStatistics { get; set; }

        public ICollection<Bet> Bets { get; set; }
       
    }
}
