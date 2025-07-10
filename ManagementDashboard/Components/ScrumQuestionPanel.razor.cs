using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class ScrumQuestionPanel : ComponentBase
    {
        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public string Icon { get; set; } = string.Empty;
        [Parameter] public string Tooltip { get; set; } = string.Empty;
        [Parameter] public IEnumerable<object> Entries { get; set; } = Enumerable.Empty<object>();
        [Parameter] public EventCallback OnAdd { get; set; }
        [Parameter] public string? Highlight { get; set; }
    }
}
