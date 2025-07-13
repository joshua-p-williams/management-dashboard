using System;
using Microsoft.AspNetCore.Components;
using ManagementDashboard.Data.Models;

namespace ManagementDashboard.Components
{
    public partial class WorkCaptureCard : ComponentBase
    {
        [Parameter] public WorkCaptureNote Note { get; set; } = default!;
        [Parameter] public EventCallback<WorkCaptureNote> OnEdit { get; set; }
        [Parameter] public EventCallback<WorkCaptureNote> OnDelete { get; set; }
    }
}
