namespace PlanningPoker.Services.Tests
{
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using PlanningPoker.Entities;
    using PlanningPoker.Services;
    using PlanningPoker.Shared;
    using Xunit;

    public class UserRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_creates_new_User()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new UserRepository(context);
                var dto = this.CreateDummyUserDTO();

                var user = await repository.CreateAsync(dto);

                Assert.Equal(1, user.Id);

                var entity = await context.Users.FindAsync(1);

                Assert.Equal("Dummy", entity.Nickname);
                Assert.Equal("dummyuser@planningpoker.com", entity.Email);
                Assert.True(entity.IsHost);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_returns_created_User()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new UserRepository(context);
                var dto = this.CreateDummyUserDTO();

                var user = await repository.CreateAsync(dto);

                Assert.Equal(1, user.Id);
                Assert.Equal("Dummy", user.Nickname);
                Assert.Equal("dummyuser@planningpoker.com", user.Email);
                Assert.True(user.IsHost);
            }
        }

        [Fact]
        public async Task FindAsync_given_id_exists_returns_dto()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummyUserEntity();

                context.Users.Add(entity);
                context.SaveChanges();

                var repository = new UserRepository(context);

                var user = await repository.FindAsync(1);

                Assert.Equal(1, user.Id);
                Assert.Equal("Dummy", user.Nickname);
                Assert.Equal("dummyuser@planningpoker.com", user.Email);
                Assert.True(user.IsHost);
            }
        }

        [Fact]
        public async Task Read_returns_projection_of_all_Users()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummyUserEntity();
                context.Users.AddRange(entity);
                context.SaveChanges();

                var repository = new UserRepository(context);

                var users = repository.Read();

                var user = await users.SingleAsync();

                Assert.Equal(1, user.Id);
                Assert.Equal("Dummy", user.Nickname);
                Assert.Equal("dummyuser@planningpoker.com", user.Email);
                Assert.True(user.IsHost);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_dto_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new UserRepository(context);
                var dto = this.CreateDummyUserDTO();
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
                context.Users.Add(this.CreateDummyUserEntity());
                context.SaveChanges();

                var repository = new UserRepository(context);
                var dto = this.CreateDummyUserDTO();
                dto.Id = 1;
                dto.Nickname = "Dummy++";

                var updated = await repository.UpdateAsync(dto);

                Assert.True(updated);

                var entity = await context.Users.FindAsync(1);

                Assert.Equal("Dummy++", entity.Nickname);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_existing_UserId_deletes_and_returns_true()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var entity = this.CreateDummyUserEntity();
                context.Users.Add(entity);
                context.SaveChanges();

                var id = entity.Id;

                var repository = new UserRepository(context);

                var deleted = await repository.DeleteAsync(id);

                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_non_existing_UserId_returns_false()
        {
            using (var connection = await this.CreateConnectionAsync())
            using (var context = await this.CreateContextAsync(connection))
            {
                var repository = new UserRepository(context);

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

        private User CreateDummyUserEntity()
        {
            return new User
            {
                IsHost = true,
                Email = "dummyuser@planningpoker.com",
                Nickname = "Dummy"
            };
        }

        private UserCreateUpdateDTO CreateDummyUserDTO()
        {
            return new UserCreateUpdateDTO
            {
                IsHost = true,
                Email = "dummyuser@planningpoker.com",
                Nickname = "Dummy"
            };
        }
    }
}
