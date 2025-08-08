using System;
using Xunit;
using ManagementDashboard.Data.Models;

namespace ManagementDashboard.Tests
{
    public class EisenhowerTaskTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData(1, false)]
        public void IsPastDue_ReturnsFalse_WhenNoDueDateOrFuture(int? daysOffset, bool expected)
        {
            var task = new EisenhowerTask
            {
                DueDate = daysOffset.HasValue ? DateTime.Now.AddDays(daysOffset.Value) : (DateTime?)null
            };
            Assert.Equal(expected, task.IsPastDue);
        }

        [Fact]
        public void IsPastDue_ReturnsTrue_WhenDueDateInPast()
        {
            var task = new EisenhowerTask { DueDate = DateTime.Now.AddDays(-1) };
            Assert.True(task.IsPastDue);
        }

        [Theory]
        [InlineData(3, 2, false)] // Due in 3 days, threshold 2 => false
        [InlineData(1, 2, true)] // Due in 1 day, threshold 2 => true
        [InlineData(0, 2, true)] // Due today, threshold 2 => true
        [InlineData(-1, 2, false)] // Past due, should be false
        [InlineData(5, 2, false)] // Due in 5 days, threshold 2 => false
        public void IsDueDateReminder_WorksCorrectly(int daysFromNow, int threshold, bool expected)
        {
            var task = new EisenhowerTask { DueDate = DateTime.Now.AddDays(daysFromNow) };
            Assert.Equal(expected, task.IsDueDateReminder(threshold));
        }

        [Theory]
        [InlineData(-3, "Past due")]
        [InlineData(0, "Due today")]
        [InlineData(2, "Due in 2 days")]
        [InlineData(null, "No due date")]
        public void DueDateSummary_ReturnsExpectedString(int? daysFromNow, string expectedStart)
        {
            var task = new EisenhowerTask { DueDate = daysFromNow.HasValue ? DateTime.Now.AddDays(daysFromNow.Value) : (DateTime?)null };
            Assert.StartsWith(expectedStart, task.DueDateSummary);
        }
    }
}
