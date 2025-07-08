using ManagementDashboard.Data.Models;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class TaskCard : ComponentBase
    {
        [Parameter] public EisenhowerTask Task { get; set; } = default!;
        [Parameter] public EventCallback OnEdit { get; set; }
        [Parameter] public EventCallback OnDelete { get; set; }
        [Parameter] public EventCallback OnComplete { get; set; }
        [Parameter] public EventCallback OnAuditTrail { get; set; }
    }
}
