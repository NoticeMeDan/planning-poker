namespace PlanningPoker.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;

    public class SummaryRepository : ISummaryRepository
    {
        private readonly IPlanningPokerContext context;

        public SummaryRepository(IPlanningPokerContext planningPokerContext)
        {
            this.context = planningPokerContext;
        }

        public async Task<SummaryDTO> CreateAsync(SummaryCreateUpdateDTO summary)
        {
            var entity = new Summary
            {
                ItemEstimates = summary.ItemEstimates,
                SessionId = summary.SessionId
            };

            this.context.Summaries.Add(entity);
            this.context.SaveChanges();

            return await this.FindAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int summaryId)
        {
            var entity = await this.context.Summaries.FindAsync(summaryId);

            if (entity == null)
            {
                return false;
            }

            this.context.Summaries.Remove(entity);
            this.context.SaveChanges();

            return true;
        }

        public async Task<SummaryDTO> FindAsync(int summaryId)
        {
            var entities = this.context.Summaries
                .Where(s => s.Id == summaryId)
                .Select(s => new SummaryDTO
                {
                    Id = s.Id,
                    ItemEstimates = s.ItemEstimates,
                    SessionId = s.SessionId
                });

            return await entities.FirstOrDefaultAsync();
        }

        public IQueryable<SummaryDTO> Read()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary)
        {
            throw new NotImplementedException();
        }
    }
}
