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

        private string Title(int maxLength)
        {
            // If we have a task, show the truncated title of the task, else show the truncated note text
            if (Note.Task != null)
            {
                return Truncate(Note.Task.Title, maxLength);
            }
            else
            {
                return Truncate(Note.Notes, maxLength);
            }
        }

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
