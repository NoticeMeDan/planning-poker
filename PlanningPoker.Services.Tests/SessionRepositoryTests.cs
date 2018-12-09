namespace PlanningPoker.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Shared;
    using Xunit;

    public class SessionRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_creates_new_Session()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SessionRepository(context);
                var dto = this.CreateDummySessionDTO();

                var session = await repository.CreateAsync(dto);

                Assert.Equal(1, session.Id);

                var entity = await context.Sessions.FindAsync(1);

                Assert.Equal("A1B2C3D", entity.SessionKey);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_returns_created_Session()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SessionRepository(context);
                var dto = this.CreateDummySessionDTO();

                var session = await repository.CreateAsync(dto);

                Assert.Equal(1, session.Id);
                Assert.Equal("A1B2C3D", session.SessionKey);
            }
        }

        [Fact]
        public async Task FindAsync_given_id_exists_returns_dto()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySessionEntity();

                context.Sessions.Add(entity);
                context.SaveChanges();

                var repository = new SessionRepository(context);

                var session = await repository.FindAsync(1);

                Assert.Equal(1, session.Id);
                Assert.Equal("A1B2C3D", session.SessionKey);
                Assert.Equal("item 1", session.Items.FirstOrDefault()?.Title);
            }
        }

        [Fact]
        public async Task FindAsyncByKey_given_key_exists_return_dto()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySessionEntity();

                context.Sessions.Add(entity);
                context.SaveChanges();

                var repository = new SessionRepository(context);

                var session = await repository.FindAsyncByKey(entity.SessionKey);

                Assert.Equal(1, session.Id);
                Assert.Equal(entity.SessionKey, session.SessionKey);
                Assert.Equal("item 1", session.Items.FirstOrDefault()?.Title);
            }
        }

        [Fact]
        public async Task Read_returns_projection_of_all_sessions()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySessionEntity();
                context.Sessions.AddRange(entity);
                context.SaveChanges();

                var repository = new SessionRepository(context);

                var sessions = repository.Read();

                var session = await sessions.SingleAsync();

                Assert.Equal(1, session.Id);
                Assert.Equal("A1B2C3D", session.SessionKey);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_dto_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SessionRepository(context);
                var dto = this.CreateDummySessionDTO();
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
                context.Sessions.Add(this.CreateDummySessionEntity());
                context.SaveChanges();

                var repository = new SessionRepository(context);
                var dto = this.CreateDummySessionDTO();
                dto.Id = 1;
                dto.SessionKey = "ABCD123";

                var updated = await repository.UpdateAsync(dto);

                Assert.True(updated);

                var entity = await context.Sessions.FindAsync(1);

                Assert.Equal("ABCD123", entity.SessionKey);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_existing_sessionId_deletes_and_returns_true()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummySessionEntity();
                context.Sessions.Add(entity);
                context.SaveChanges();

                var id = entity.Id;

                var repository = new SessionRepository(context);

                var deleted = await repository.DeleteAsync(id);

                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_non_existing_sessionId_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new SessionRepository(context);

                var deleted = await repository.DeleteAsync(42);

                Assert.False(deleted);
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

        private Session CreateDummySessionEntity()
        {
            return new Session
            {
                SessionKey = "A1B2C3D",
                Items = new List<Item> { new Item { Title = "item 1", Rounds = new HashSet<Round>() }, new Item { Title = "item 2", Rounds = new HashSet<Round>() } },
                Users = new List<User> { new User { IsHost = true, Nickname = "user 1" } }
            };
        }

        private SessionCreateUpdateDTO CreateDummySessionDTO()
        {
            return new SessionCreateUpdateDTO
            {
                SessionKey = "A1B2C3D",
                Items = new List<ItemCreateUpdateDTO>
                {
                    new ItemCreateUpdateDTO { Title = "item 1", Rounds = new HashSet<RoundDTO>() },
                    new ItemCreateUpdateDTO { Title = "item 2", Rounds = new HashSet<RoundDTO>() }
                },
                Users = new List<UserCreateUpdateDTO>
                {
                    new UserCreateUpdateDTO { IsHost = true, Nickname = "user 1" }
                }
            };
        }
    }
}
