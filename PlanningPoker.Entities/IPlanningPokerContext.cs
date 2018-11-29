using Microsoft.EntityFrameworkCore;

namespace PlanningPoker.Entities
{
    using System;

    public interface IPlanningPokerContext : IDisposable
    {
        DbSet<Item> Items { get; set; }
        DbSet<ItemEstimate> ItemEstimates { get; set; }
        DbSet<Round> Rounds { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<Summary> Summaries { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Vote> Votes { get; set; }

        int SaveChanges();
    }
}
