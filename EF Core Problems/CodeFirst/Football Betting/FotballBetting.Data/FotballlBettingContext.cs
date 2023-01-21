using FootballBetting.Data.Common;
using FootballBetting.Data.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace FotballBetting.Data
{
    public class FotballBettingContext : DbContext
    {
        public FotballBettingContext()
        {

        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Teams> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Bet77;Integrated Security=true;Encrypt=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<PlayerStatistics>(e =>
                {
                    e.HasKey("PlayerId", "GameId");
                });

            modelBuilder
                .Entity<Teams>(e =>
                {
                    e.HasOne(t => t.PrimaryKitColor)
                     .WithMany(c => c.PrimaryKitTeams)
                     .HasForeignKey(t => t.PrimaryKitColorId)
                     .OnDelete(DeleteBehavior.ClientSetNull);

                    e.HasOne(t => t.SecondoryKitColor)
                      .WithMany(c => c.SecondoryKitTeams)
                      .HasForeignKey(t => t.SecondoryKitColorId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
                });

            modelBuilder
                .Entity<Game>(e =>
                {
                     e.HasOne(ht => ht.HomeTeam)
                    .WithMany(hg => hg.HomeGames)
                    .HasForeignKey(t => t.HomeTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    e.HasOne(t => t.AwayTeam)
                        .WithMany(c => c.AwayGames)
                        .HasForeignKey(t => t.AwayTeamId)
                        .OnDelete(DeleteBehavior.ClientSetNull);
                });
        }
    }
}