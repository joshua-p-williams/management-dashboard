using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Core.Extensions;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Components
{
    public partial class TaskSummary : ComponentBase
    {
        [Parameter] public EisenhowerTask Task { get; set; } = default!;
        [Parameter] public DateTime Date { get; set; }

        [Inject] public IAppPreferences AppPreferences { get; set; } = default!;

        protected IEnumerable<string> Events => Task?.SummarizeEvents(Date) ?? Enumerable.Empty<string>();
        protected IEnumerable<string> State => Task?.SummarizedState(AppPreferences?.GetInt("OverdueThresholdDays", 2) ?? 2) ?? Enumerable.Empty<string>();

        private int OverdueThresholdDays => AppPreferences?.GetInt("OverdueThresholdDays", 2) ?? 2;

        private bool ShowDetails = false;

        private void ToggleDetails()
        {
            ShowDetails = !ShowDetails;
        }

        protected RenderFragment GetTaskStatusBadge(EisenhowerTask task) => builder =>
        {
            var status = task.GetStatus();
            var (badgeClass, iconClass) = status switch
            {
                "Done" => ("badge bg-success", "bi-check-circle"),
                "Blocked" => ("badge bg-danger", "bi-exclamation-triangle"),
                "Removed" => ("badge bg-dark", "bi-x-circle"),
                "In Progress" => ("badge bg-warning text-dark", "bi-arrow-repeat"),
                "Unknown" => ("badge bg-secondary", "bi-question-circle"),
                _ => ("badge bg-secondary", "bi-question-circle")
            };
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", badgeClass);
            builder.OpenElement(2, "i");
            builder.AddAttribute(3, "class", $"bi {iconClass} me-1");
            builder.CloseElement();
            builder.AddContent(4, status);
            builder.CloseElement();
        };
    }
}
