using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.Data.Sqlite;
using ManagementDashboard.Data.Migrations;
using System.IO;

namespace ManagementDashboard.Tests
{
    public class EisenhowerTaskRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly EisenhowerTaskRepository _repository;
        private readonly string _dbName;
        private readonly string _migrationsPath;

        public EisenhowerTaskRepositoryTests()
        {
            // Use a shared in-memory SQLite DB for all connections in this test class
            _dbName = Guid.NewGuid().ToString();
            var connString = $"Data Source={_dbName};Mode=Memory;Cache=Shared";
            _connection = new SqliteConnection(connString);
            _connection.Open(); // Keep open for test lifetime

            // Find the migrations path in the Data project's output directory
            _migrationsPath = Path.Combine(
                Path.GetDirectoryName(typeof(EisenhowerTaskRepository).Assembly.Location)!,
                "Migrations"
            );
            var runner = new MigrationRunner(_migrationsPath, () => new SqliteConnection(connString));
            runner.RunMigrations();

            // Verify Tasks table exists and matches expected schema
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(Tasks);";
                using var reader = cmd.ExecuteReader();
                var columns = new HashSet<string>();
                while (reader.Read())
                {
                    columns.Add(reader.GetString(1));
                }
                var expected = new[] {
                    "Id", "Title", "Description", "Quadrant", "Priority", "CompletedAt", "BlockerReason", "BlockedAt", "UnblockedAt", "DelegatedTo", "CreatedAt", "UpdatedAt", "DeletedAt"
                };
                foreach (var col in expected)
                {
                    Assert.Contains(col, columns);
                }
            }

            // Setup configuration for Formula.SimpleRepo
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", connString }
            });
            _configuration = configBuilder.Build();

            _repository = new EisenhowerTaskRepository(_configuration);
        }

        [Fact]
        public async Task AddTaskAsync_InsertsTask()
        {
            var task = new EisenhowerTask
            {
                Title = "Test Task",
                Description = "Test Desc",
                Quadrant = "Do",
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _repository.InsertAsync(task);
            var tasks = await _repository.GetTasksByQuadrantAsync("Do");
            Assert.Contains(tasks, t => t.Title == "Test Task");
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesTask()
        {
            var task = new EisenhowerTask
            {
                Title = "To Update",
                Description = "Desc",
                Quadrant = "Schedule",
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var id = await _repository.InsertAsync(task);
            // Fetch the inserted task to get its Id
            var tasks = await _repository.GetTasksByQuadrantAsync("Schedule");
            var inserted = Assert.Single(tasks);
            Assert.True(inserted.Id > 0); // Ensure Id is set
            Assert.Equal(id, inserted.Id);
            inserted.IsCompleted = true;
            inserted.UpdatedAt = DateTime.Now;
            var count = await _repository.UpdateAsync(inserted);
            Assert.Equal(1, count);
            var updated = await _repository.GetTasksByQuadrantAsync("Schedule");
            Assert.True(updated is not null && System.Linq.Enumerable.First(updated).IsCompleted);
        }

        [Fact]
        public async Task GetTasksByQuadrantAsync_ReturnsCorrectTasks()
        {
            var task1 = new EisenhowerTask
            {
                Title = "Task 1",
                Quadrant = "Do",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var task2 = new EisenhowerTask
            {
                Title = "Task 2",
                Quadrant = "Delegate",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _repository.InsertAsync(task1);
            await _repository.InsertAsync(task2);
            var doTasks = await _repository.GetTasksByQuadrantAsync("Do");
            var delegateTasks = await _repository.GetTasksByQuadrantAsync("Delegate");
            Assert.Single(doTasks);
            Assert.Single(delegateTasks);
        }

        public void Dispose()
        {
            _connection?.Dispose(); // This will destroy the in-memory DB
        }
    }
}
