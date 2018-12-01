namespace PlanningPoker.Services.Tests
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Services;
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
                var dto = new SessionCreateUpdateDTO()
                {
                    SessionKey = "A1B2C3D",
                    Items = { new ItemDTO(), new ItemDTO() },
                    Users = { new UserDTO() }
                };

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
                var dto = new SessionCreateUpdateDTO()
                {
                    SessionKey = "A1B2C3D",
                    Items = { new ItemDTO(), new ItemDTO() },
                    Users = { new UserDTO() }
                };

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
                var entity = new Session
                {
                    SessionKey = "A1B2C3D",
                    Items = new[] { new Item { Title = "item 1" }, new Item { Title = "item 2" } },
                    Users = new[] { new User { IsHost = true, Nickname = "user 1" } }
                };
                context.Sessions.Add(entity);
                context.SaveChanges();

                var repository = new SessionRepository(context);

                var session = await repository.FindAsync(1);

                Assert.Equal(1, session.Id);
                Assert.Equal("A1B2C3D", session.SessionKey);
                Assert.Equal("item 1", session.Items.First().Title);
            }
        }

        private async Task<DbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            return connection;
        }

        private async Task<IPlanningPokerContext> CreateContextAsync(DbConnection connection)
        {
            var builder = new DbContextOptionsBuilder<PlanningPokerContext>()
                              .UseSqlite(connection);

            var context = new PlanningPokerContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }
    }
}
