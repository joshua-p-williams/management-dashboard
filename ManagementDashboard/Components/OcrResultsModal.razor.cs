using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public partial class OcrResultsModal : ComponentBase
    {
        [Parameter]
        public string ExtractedText { get; set; } = string.Empty;

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<string> OnCreateTask { get; set; }

        [Parameter]
        public EventCallback<string> OnCreateWorkNote { get; set; }

        protected string EditableText { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            EditableText = ExtractedText ?? string.Empty;
        }

        protected async Task HandleCancel()
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync();
            }
        }

        protected async Task HandleCreateTask()
        {
            if (OnCreateTask.HasDelegate && !string.IsNullOrWhiteSpace(EditableText))
            {
                await OnCreateTask.InvokeAsync(EditableText);
            }
        }

        protected async Task HandleCreateWorkNote()
        {
            if (OnCreateWorkNote.HasDelegate && !string.IsNullOrWhiteSpace(EditableText))
            {
                await OnCreateWorkNote.InvokeAsync(EditableText);
            }
        }
    }
}