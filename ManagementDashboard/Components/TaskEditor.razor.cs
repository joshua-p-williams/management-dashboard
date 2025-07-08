using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class TaskEditor : ComponentBase
    {
        [Parameter] public string? Quadrant { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public EventCallback OnSaved { get; set; }
        [Inject] IEisenhowerTaskRepository TaskRepository { get; set; } = default!;
        private string ModalTitle => !string.IsNullOrEmpty(Quadrant) ? $"Add Task ({Quadrant})" : "Add Task";
        private string Title { get; set; } = string.Empty;
        private string? Description { get; set; }
        private bool isSaving = false;
        private string? error;

        private async Task SaveAsync()
        {
            error = null;
            if (string.IsNullOrWhiteSpace(Title) || Title.Length < 3)
            {
                error = "Title is required (min 3 chars).";
                return;
            }
            isSaving = true;
            var now = DateTime.UtcNow;
            var task = new EisenhowerTask
            {
                Title = Title.Trim(),
                Description = Description,
                Quadrant = Quadrant,
                IsCompleted = false,
                CreatedAt = now,
                UpdatedAt = now
            };
            await TaskRepository.InsertAsync(task);
            isSaving = false;
            await OnSaved.InvokeAsync();
        }
    }
}
