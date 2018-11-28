using Microsoft.EntityFrameworkCore;

namespace PlanningPoker.Entities
{
    public class PlanningPokerContext : DbContext, IPlanningPokerContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemEstimate> ItemEstimates { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public PlanningPokerContext(DbContextOptions options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
