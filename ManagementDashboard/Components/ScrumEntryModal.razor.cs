using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class ScrumEntryModal : ComponentBase
    {
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public bool IsEdit { get; set; }
        [Parameter] public bool ShowBlockerField { get; set; }
        [Parameter] public string TaskTitle { get; set; } = string.Empty;
        [Parameter] public string NoteText { get; set; } = string.Empty;
        [Parameter] public string BlockerDetails { get; set; } = string.Empty;
        [Parameter] public EventCallback OnSave { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
    }
}
