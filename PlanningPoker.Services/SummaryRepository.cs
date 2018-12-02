namespace PlanningPoker.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
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

        public Task<bool> DeleteAsync(int summaryId)
        {
            throw new NotImplementedException();
        }

        public Task<SummaryDTO> FindAsync(int summaryId)
        {
            throw new NotImplementedException();
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
