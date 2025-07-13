using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Core.Extensions;

namespace ManagementDashboard.Components
{
    public partial class TaskSummary : ComponentBase
    {
        [Parameter] public EisenhowerTask Task { get; set; } = default!;
        [Parameter] public DateTime Date { get; set; }

        protected IEnumerable<string> Events => Task?.SummarizeEvents(Date) ?? Enumerable.Empty<string>();
        protected IEnumerable<string> State => Task?.SummarizedState() ?? Enumerable.Empty<string>();

        protected RenderFragment GetTaskStatusBadge(EisenhowerTask task) => builder =>
        {
            var status = task.GetStatus();
            var badgeClass = status switch
            {
                "Done" => "badge bg-success",
                "Blocked" => "badge bg-danger",
                "Removed" => "badge bg-dark",
                "In Progress" => "badge bg-warning text-dark",
                "Unknown" => "badge bg-secondary",
                _ => "badge bg-secondary"
            };
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", badgeClass);
            builder.AddContent(2, status);
            builder.CloseElement();
        };
    }
}
