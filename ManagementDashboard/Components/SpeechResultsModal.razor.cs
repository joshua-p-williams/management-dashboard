using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public partial class SpeechResultsModal : ComponentBase
    {
        [Parameter]
        public string TranscribedText { get; set; } = string.Empty;

        [Parameter]
        public float? TranscriptionConfidence { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<string> OnCreateTask { get; set; }

        [Parameter]
        public EventCallback<string> OnCreateWorkNote { get; set; }

        protected string EditableText { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            EditableText = TranscribedText ?? string.Empty;
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

        protected string GetConfidenceBadgeClass()
        {
            if (!TranscriptionConfidence.HasValue) return "bg-secondary";

            var confidence = TranscriptionConfidence.Value;
            return confidence switch
            {
                >= 0.9f => "bg-success",      // Excellent (90%+)
                >= 0.8f => "bg-primary",      // Good (80-89%)
                >= 0.7f => "bg-warning",      // Fair (70-79%)
                _ => "bg-danger"               // Poor (<70%)
            };
        }

        protected string GetConfidenceProgressClass()
        {
            if (!TranscriptionConfidence.HasValue) return "bg-secondary";

            var confidence = TranscriptionConfidence.Value;
            return confidence switch
            {
                >= 0.9f => "bg-success",      // Excellent (90%+)
                >= 0.8f => "bg-primary",      // Good (80-89%)
                >= 0.7f => "bg-warning",      // Fair (70-79%)
                _ => "bg-danger"               // Poor (<70%)
            };
        }

        protected string GetConfidenceText()
        {
            if (!TranscriptionConfidence.HasValue) return "Unknown";

            var confidence = TranscriptionConfidence.Value;
            var percentage = (confidence * 100).ToString("0");
            
            return confidence switch
            {
                >= 0.9f => $"Excellent ({percentage}%)",
                >= 0.8f => $"Good ({percentage}%)",
                >= 0.7f => $"Fair ({percentage}%)",
                _ => $"Poor ({percentage}%)"
            };
        }
    }
}