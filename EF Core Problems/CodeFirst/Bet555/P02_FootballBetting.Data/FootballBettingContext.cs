using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using System.Text;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
            
        }
      
        public DbSet<Team> Teams { get; set; }

        public DbSet<Team> Colors { get; set; }

        public DbSet<Team> Towns { get; set; }

        public DbSet<Team> Countries { get; set; }

        public DbSet<Team> Players { get; set; }

        public DbSet<Team> Positions { get; set; }

        public DbSet<Team> PlayerStatistics { get; set; }

        public DbSet<Team> Games { get; set; }

        public DbSet<Team> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Bet555;Integrated Security=True;Encrypt=False;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(ps => new
                {
                    ps.PlayerId,
                    ps.GameId
                });
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);               
                
               entity.HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(t => t.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(t => t.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);
            });

          
        }
    }
}
