namespace PlanningPoker.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;
    using Xunit;

    public class SummaryRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_creates_new_Summary()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);
                var dto = this.CreateDummySummaryDTO();

                var summary = await repository.CreateAsync(dto);

                Assert.Equal(1, summary.Id);

                var entity = await context.Summaries.FindAsync(1);

                Assert.Equal(42, entity.SessionId);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_returns_created_Summary()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);
                var dto = this.CreateDummySummaryDTO();

                var summary = await repository.CreateAsync(dto);

                Assert.Equal(1, summary.Id);
                Assert.Equal(42, summary.SessionId);
            }
        }

        [Fact]
        public async Task FindAsync_given_id_exists_returns_dto()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySummaryEntity();

                context.Summaries.Add(entity);
                context.SaveChanges();

                var repository = new SummaryRepository(context);

                var summary = await repository.FindBySessionIdAsync(42);

                Assert.Equal(1, summary.Id);
                Assert.Equal(42, summary.SessionId);
                Assert.Equal("item 1", summary.ItemEstimates.FirstOrDefault().ItemTitle);
            }
        }

        [Fact]
        public async Task FindBySessionIdAsync_given_id_exists_returns_dto()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySummaryEntity();

                context.Summaries.Add(entity);
                context.SaveChanges();

                var repository = new SummaryRepository(context);

                var summary = await repository.FindAsync(1);

                Assert.Equal(1, summary.Id);
                Assert.Equal(42, summary.SessionId);
                Assert.Equal("item 1", summary.ItemEstimates.FirstOrDefault().ItemTitle);
            }
        }

        [Fact]
        public async Task Read_returns_projection_of_all_Summarys()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySummaryEntity();
                context.Summaries.AddRange(entity);
                context.SaveChanges();

                var repository = new SummaryRepository(context);

                var summarys = repository.Read();

                var summary = await summarys.SingleAsync();

                Assert.Equal(1, summary.Id);
                Assert.Equal(42, summary.SessionId);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_dto_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);
                var dto = this.CreateDummySummaryDTO();
                dto.Id = 0;

                var updated = await repository.UpdateAsync(dto);

                Assert.False(updated);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_existing_dto_updates_entity()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                context.Summaries.Add(this.CreateDummySummaryEntity());
                context.SaveChanges();

                var repository = new SummaryRepository(context);
                var dto = this.CreateDummySummaryDTO();
                dto.Id = 1;
                dto.SessionId = 45;

                var updated = await repository.UpdateAsync(dto);

                Assert.True(updated);

                var entity = await context.Summaries.FindAsync(1);

                Assert.Equal(45, entity.SessionId);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_existing_SummaryId_deletes_and_returns_true()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySummaryEntity();
                context.Summaries.Add(entity);
                context.SaveChanges();

                var id = entity.Id;

                var repository = new SummaryRepository(context);

                var deleted = await repository.DeleteAsync(id);

                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_non_existing_SummaryId_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);

                var deleted = await repository.DeleteAsync(42);

                Assert.False(deleted);
            }
        }

        [Fact]
        public async Task BuildSummary_returns_correct_number_of_itemEstimates()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);
                var session = this.CreateDummySessionDTO();

                var summary = repository.BuildSummary(session);

                Assert.Equal(2, summary.ItemEstimates.Count);
            }
        }

        [Fact]
        public async Task BuildSummary_returns_correct_itemEstimates()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SummaryRepository(context);
                var session = this.CreateDummySessionDTO();

                var summary = repository.BuildSummary(session);

                Assert.Equal("item1", summary.ItemEstimates.FirstOrDefault().ItemTitle);
                Assert.Equal("item2", summary.ItemEstimates.LastOrDefault().ItemTitle);
                Assert.Equal(13, summary.ItemEstimates.FirstOrDefault().Estimate);
                Assert.Equal(37, summary.ItemEstimates.LastOrDefault().Estimate);
            }
        }

        private async Task<SqliteConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            return connection;
        }

        private async Task<IPlanningPokerContext> CreateContextAsync(SqliteConnection connection)
        {
            var builder = new DbContextOptionsBuilder<PlanningPokerContext>()
                              .UseSqlite(connection);

            var context = new PlanningPokerContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        private Summary CreateDummySummaryEntity()
        {
            return new Summary
            {
                ItemEstimates = new List<ItemEstimate>
                {
                    new ItemEstimate { Estimate = 5, ItemTitle = "item 1" },
                    new ItemEstimate { Estimate = 13, ItemTitle = "item 2" }
                },

                SessionId = 42
            };
        }

        private SummaryCreateUpdateDTO CreateDummySummaryDTO()
        {
            return new SummaryCreateUpdateDTO
            {
                ItemEstimates = new List<ItemEstimateDTO>
                {
                    new ItemEstimateDTO { Estimate = 5, ItemTitle = "item 1" },
                    new ItemEstimateDTO { Estimate = 13, ItemTitle = "item 2" }
                },

                SessionId = 42
            };
        }

        private SessionDTO CreateDummySessionDTO()
        {
            return new SessionDTO
            {
                SessionKey = "A1B2C3D",
                Items = this.CreateDummyItemHashSet(),
                Users = new HashSet<UserDTO>
                {
                    new UserDTO { Id = 1, IsHost = true, Email = string.Empty, Nickname = "user1" },
                    new UserDTO { Id = 2, IsHost = false, Email = string.Empty, Nickname = "user2" }
                }
            };
        }

        private ICollection<ItemDTO> CreateDummyItemHashSet()
        {
            return new HashSet<ItemDTO>
            {
                new ItemDTO
                {
                    Title = "item1",
                    Description = "description1",
                    Rounds = new HashSet<RoundDTO>
                    {
                        new RoundDTO
                        {
                            Votes = new List<VoteDTO> { new VoteDTO { UserId = 1, Estimate = 5}, new VoteDTO { UserId = 2, Estimate = 8 } }
                        },
                        new RoundDTO
                        {
                            Votes = new List<VoteDTO> { new VoteDTO { UserId = 1, Estimate = 13}, new VoteDTO { UserId = 2, Estimate = 13 } }
                        }
                    }
                },
                new ItemDTO
                {
                    Title = "item2",
                    Description = "description2",
                    Rounds = new HashSet<RoundDTO>
                    {
                        new RoundDTO
                        {
                            Votes = new List<VoteDTO> { new VoteDTO { UserId = 1, Estimate = 15}, new VoteDTO { UserId = 2, Estimate = 21 } }
                        },
                        new RoundDTO
                        {
                            Votes = new List<VoteDTO> { new VoteDTO { UserId = 1, Estimate = 37}, new VoteDTO { UserId = 2, Estimate = 37 } }
                        }
                    }
                }
            };
        }
    }
}
