namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Shared;
    using Util;

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
                ItemEstimates = EntityMapper.ToItemEstimateEntities(summary.ItemEstimates),
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
                .Include(ie => ie.ItemEstimates)
                .Select(s => new SummaryDTO
                {
                    Id = s.Id,
                    ItemEstimates = EntityMapper.ToItemEstimateDtos(s.ItemEstimates),
                    SessionId = s.SessionId
                });

            return await entities.FirstOrDefaultAsync();
        }

        public async Task<SummaryDTO> FindBySessionIdAsync(int sessionId)
        {
            var entities = this.context.Summaries
                .Where(s => s.SessionId == sessionId)
                .Select(s => new SummaryDTO
                {
                    Id = s.Id,
                    ItemEstimates = EntityMapper.ToItemEstimateDtos(s.ItemEstimates),
                    SessionId = s.SessionId
                });

            return await entities.FirstOrDefaultAsync();
        }

        public IQueryable<SummaryDTO> Read()
        {
            var entities = this.context.Summaries
                .Include(ie => ie.ItemEstimates)
                .Select(s => new SummaryDTO
                {
                    Id = s.Id,
                    ItemEstimates = EntityMapper.ToItemEstimateDtos(s.ItemEstimates),
                    SessionId = s.SessionId
                });

            return entities;
        }

        public async Task<bool> UpdateAsync(SummaryCreateUpdateDTO summary)
        {
            var entity = await this.context.Summaries.FindAsync(summary.Id);

            if (entity == null)
            {
                return false;
            }

            entity.Id = summary.Id;
            entity.ItemEstimates = EntityMapper.ToItemEstimateEntities(summary.ItemEstimates);
            entity.SessionId = summary.SessionId;

            this.context.SaveChanges();

            return true;
        }

        public async Task<SummaryDTO> BuildSummary(SessionDTO session)
        {
            var summary = new SummaryCreateUpdateDTO
            {
                SessionId = session.Id,
                ItemEstimates = this.BuildItemEstimates(session).ToList()
            };

            return await this.CreateAsync(summary);
        }

        public ICollection<ItemEstimateDTO> BuildItemEstimates(SessionDTO session)
        {
            var itemEstimates = new HashSet<ItemEstimateDTO>();
            session.Items.ToList().ForEach(i => itemEstimates.Add(
                new ItemEstimateDTO
                {
                    Estimate = i.Rounds.LastOrDefault().Votes.FirstOrDefault().Estimate,
                    ItemTitle = i.Title
                }));
            return itemEstimates;
        }
    }
}
