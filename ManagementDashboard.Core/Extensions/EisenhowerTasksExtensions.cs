using System;
using System.Collections.Generic;
using ManagementDashboard.Data.Models;
using System;
using System.Collections.Generic;

namespace ManagementDashboard.Core.Extensions
{
public static class EisenhowerTasksExtensions
{
    /// <summary>
    /// Returns a list of event descriptions for the given EisenhowerTask that occurred on the specified date.
    /// </summary>
    public static IEnumerable<string> SummarizeEvents(this EisenhowerTask task, DateTime date)
    {
        var events = new List<string>();
        var d = date.Date;

        // Date tracked events to summarize
        if (task.CreatedAt.Date == d)
            events.Add("Picked up as new task");
        if (task.UpdatedAt.Date == d && task.UpdatedAt != task.CreatedAt)
            events.Add("Continued to work on this task");
        if (task.CompletedAt.HasValue && task.CompletedAt.Value.Date == d)
            events.Add("Completed");
        if (task.BlockedAt.HasValue && task.BlockedAt.Value.Date == d)
        {
            var reason = string.IsNullOrWhiteSpace(task.BlockerReason) ? "" : $" - {task.BlockerReason}";
            events.Add($"Became blocked{reason}");
        }
        if (task.UnblockedAt.HasValue && task.UnblockedAt.Value.Date == d)
            events.Add("Blocker resolved");
        if (task.DeletedAt.HasValue && task.DeletedAt.Value.Date == d)
            events.Add("Removed from active tasks");

        return events;
    }

    /// <summary>
    /// Returns a list of general state summaries for the given EisenhowerTask.
    /// </summary>
    public static IEnumerable<string> SummarizedState(this EisenhowerTask task)
    {
        var state = new List<string>();

        if (!string.IsNullOrWhiteSpace(task.DelegatedTo))
            state.Add($"Delegated to {task.DelegatedTo}");
        if (task.IsCompleted)
            state.Add("Completed");
        if (task.IsBlocked)
        {
            var reason = string.IsNullOrWhiteSpace(task.BlockerReason) ? "" : $" - {task.BlockerReason}";
            state.Add($"Currently blocked{reason}");
        }
        if (task.IsDeleted)
            state.Add("Removed from active tasks");
        if (string.IsNullOrWhiteSpace(task.Quadrant))
            state.Add("Uncategorized");
        else
            state.Add($"Quadrant: {task.Quadrant}");
        state.Add($"Priority: {task.Priority}");

        return state;
    }

    /// <summary>
    /// Returns a status string for the given EisenhowerTask.
    /// </summary>
    public static string GetStatus(this EisenhowerTask task)
    {
        if (task == null) return "Unknown";
        if (task.IsDeleted) return "Removed";
        if (task.IsBlocked) return "Blocked";
        if (task.IsCompleted) return "Done";
        return "In Progress";
    }
    }
}
