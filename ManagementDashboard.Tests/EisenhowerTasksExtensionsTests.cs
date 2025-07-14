using System;
using System.Linq;
using System.Collections.Generic;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Core.Extensions;
using Xunit;

namespace ManagementDashboard.Tests
{
    public class EisenhowerTasksExtensionsTests
    {
        [Fact]
        public void SummarizeEvents_ReturnsCreatedEvent()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date, UpdatedAt = date };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Contains("Picked up as new task", events);
        }

        [Fact]
        public void SummarizeEvents_ReturnsCompletedEvent()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date.AddDays(-1), CompletedAt = date, UpdatedAt = date };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Contains("Completed", events);
        }

        [Fact]
        public void SummarizeEvents_ReturnsBlockedEventWithReason()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date.AddDays(-2), BlockedAt = date, BlockerReason = "Waiting for review", UpdatedAt = date };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Contains("Became blocked - Waiting for review", events);
        }

        [Fact]
        public void SummarizeEvents_ReturnsUnblockedEvent()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date.AddDays(-3), BlockedAt = date.AddDays(-1), UnblockedAt = date, UpdatedAt = date };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Contains("Blocker resolved", events);
        }

        [Fact]
        public void SummarizeEvents_ReturnsUpdatedEventOnlyIfDifferentFromCreated()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date.AddDays(-1), UpdatedAt = date };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Contains("Continued to work on this task", events);
        }

        [Fact]
        public void SummarizedState_ReturnsDelegatedAndQuadrantAndPriority()
        {
            var task = new EisenhowerTask {
                DelegatedTo = "Alice",
                Quadrant = "Do",
                Priority = PriorityLevel.High,
                CompletedAt = null,
                BlockedAt = null,
                DeletedAt = null
            };
            var state = task.SummarizedState().ToList();
            Assert.Contains("Delegated to Alice", state);
            Assert.Contains("Quadrant: Do", state);
            Assert.Contains("Priority: High", state);
        }

        [Fact]
        public void SummarizedState_ReturnsCompletedAndBlockedAndDeleted()
        {
            var task = new EisenhowerTask {
                DelegatedTo = "Bob",
                Quadrant = "Schedule",
                Priority = PriorityLevel.Medium,
                CompletedAt = DateTime.Now,
                BlockedAt = DateTime.Now,
                BlockerReason = "Waiting for review",
                DeletedAt = DateTime.Now
            };
            var state = task.SummarizedState().ToList();
            Assert.Contains("Completed", state);
            Assert.Contains("Currently blocked - Waiting for review", state);
            Assert.Contains("Removed from active tasks", state);
        }

        [Fact]
        public void SummarizedState_ReturnsOverdueIfPastDue()
        {
            var overdueDays = 2;
            var task = new EisenhowerTask {
                CreatedAt = DateTime.Now.AddDays(-overdueDays - 1),
                Priority = PriorityLevel.High
            };
            var state = task.SummarizedState(overdueDays).ToList();
            Assert.Contains("Overdue", state);
        }
        
        [Fact]
        public void SummarizeEvents_ReturnsNoEventsForOtherDates()
        {
            var date = new DateTime(2025, 7, 11);
            var task = new EisenhowerTask { CreatedAt = date.AddDays(-5), UpdatedAt = date.AddDays(-5) };
            var events = task.SummarizeEvents(date).ToList();
            Assert.Empty(events);
        }
    [Fact]
    public void GetStatus_ReturnsBlockedIfBlocked()
    {
        var task = new EisenhowerTask { BlockedAt = DateTime.Now };
        Assert.Equal("Blocked", task.GetStatus());
    }

    [Fact]
    public void GetStatus_ReturnsDoneIfCompleted()
    {
        var task = new EisenhowerTask { CompletedAt = DateTime.Now };
        Assert.Equal("Done", task.GetStatus());
    }

    [Fact]
    public void GetStatus_ReturnsRemovedIfDeleted()
    {
        var task = new EisenhowerTask { DeletedAt = DateTime.Now };
        Assert.Equal("Removed", task.GetStatus());
    }

    [Fact]
    public void GetStatus_ReturnsInProgressIfNone()
    {
        var task = new EisenhowerTask();
        Assert.Equal("In Progress", task.GetStatus());
    }
    }
}
