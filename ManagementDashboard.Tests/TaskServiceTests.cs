using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Data.Repositories;
using Moq;
using Xunit;

namespace ManagementDashboard.Tests
{
    public class TaskServiceTests
    {
        private List<EisenhowerTask> GetSampleTasks()
        {
            return new List<EisenhowerTask>
            {
                new EisenhowerTask { Id = 1, Title = "Do Task", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-5) },
                new EisenhowerTask { Id = 2, Title = "Schedule Task", Quadrant = "Schedule", Priority = PriorityLevel.Medium, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-4) },
                new EisenhowerTask { Id = 3, Title = "Delegate Task", Quadrant = "Delegate", Priority = PriorityLevel.Low, IsBlocked = true, CreatedAt = DateTime.Now.AddDays(-3) },
                new EisenhowerTask { Id = 4, Title = "Delete Task", Quadrant = "Delete", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-2) },
                new EisenhowerTask { Id = 5, Title = "Do Task 2", Quadrant = "Do", Priority = PriorityLevel.Medium, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-1) },
            };
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_ReturnsCorrectNumberOfTasks()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(GetSampleTasks());
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(3);

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_ReturnsTasksInCorrectOrder()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(GetSampleTasks());
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(5);

            // Should be ordered by Quadrant (Do, Schedule, Delegate, Delete), then Priority, then IsBlocked, then CreatedAt
            Assert.Equal("Do Task", result[0].Title);
            Assert.Equal("Do Task 2", result[1].Title);
            Assert.Equal("Schedule Task", result[2].Title);
            Assert.Equal("Delegate Task", result[3].Title);
            Assert.Equal("Delete Task", result[4].Title);
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_ReturnsEmptyListIfNoTasks()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(new List<EisenhowerTask>());
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_OrdersBlockedTasksAfterUnblocked()
        {
            var tasks = new List<EisenhowerTask>
            {
                new EisenhowerTask { Id = 1, Title = "Do Task", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new EisenhowerTask { Id = 2, Title = "Do Task 2", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-1) },
            };
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(tasks);
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(2);
            Assert.Equal("Do Task 2", result[0].Title); // Unblocked first
            Assert.Equal("Do Task", result[1].Title);   // Blocked second
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_HandlesUnexpectedQuadrantValues()
        {
            var tasks = new List<EisenhowerTask>
            {
                new EisenhowerTask { Id = 1, Title = "Unknown Quadrant", Quadrant = "Other", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-1) },
                new EisenhowerTask { Id = 2, Title = "Do Task", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-2) },
            };
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(tasks);
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(2);
            Assert.Equal("Do Task", result[0].Title); // Known quadrant first
            Assert.Equal("Unknown Quadrant", result[1].Title); // Unknown quadrant last
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_OrdersByCreatedDateOnTieBreaker()
        {
            var tasks = new List<EisenhowerTask>
            {
                new EisenhowerTask { Id = 1, Title = "Older", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-2) },
                new EisenhowerTask { Id = 2, Title = "Newer", Quadrant = "Do", Priority = PriorityLevel.High, IsBlocked = false, CreatedAt = DateTime.Now.AddDays(-1) },
            };
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(tasks);
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(2);
            Assert.Equal("Older", result[0].Title); // Oldest first
            Assert.Equal("Newer", result[1].Title); // Newest second
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_RespectsDefaultCountParameter()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(GetSampleTasks());
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync();
            Assert.Equal(5, result.Count); // Default is 5
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_HandlesNullTaskList()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync((List<EisenhowerTask>)null);
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetNextTasksToWorkOnAsync_WithNullCount_ReturnsAllTasks()
        {
            var mockRepo = new Mock<IEisenhowerTaskRepository>();
            mockRepo.Setup(r => r.GetOpenTasksAsync()).ReturnsAsync(GetSampleTasks());
            var service = new TaskService(mockRepo.Object);

            var result = await service.GetNextTasksToWorkOnAsync(null);

            Assert.Equal(GetSampleTasks().Count, result.Count);
        }
    }
}
