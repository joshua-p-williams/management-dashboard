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
    }
}
