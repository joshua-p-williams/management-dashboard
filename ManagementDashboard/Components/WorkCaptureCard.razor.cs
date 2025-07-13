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

        private bool ShowTaskDetails = false;

        private string Truncate(string? text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length > maxLength ? text.Substring(0, maxLength) + "…" : text;
        }

        private void ToggleTaskDetails()
        {
            ShowTaskDetails = !ShowTaskDetails;
        }

    }
}
