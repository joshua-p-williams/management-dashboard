using ManagementDashboard.Data.Models;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class TaskAuditTrail : ComponentBase
    {
        [Parameter] public EisenhowerTask? Task { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
    }
}
