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
        [Parameter] public EisenhowerTask? TaskToEdit { get; set; }

        private string ModalTitle => !string.IsNullOrEmpty(Quadrant) ? $"Add Task ({Quadrant})" : "Add Task";
        private string Title { get; set; } = string.Empty;
        private string? Description { get; set; }
        private DateTime? CompletedAt { get; set; }
        private string? Priority { get; set; }
        private string? DelegatedTo { get; set; }
        private string Status { get; set; } = "Open";
        private string? BlockerReason { get; set; }
        private bool isSaving = false;
        private string? error;

        protected override void OnInitialized()
        {
            if (TaskToEdit != null)
            {
                Title = TaskToEdit.Title;
                Description = TaskToEdit.Description;
                Quadrant = TaskToEdit.Quadrant;
                CompletedAt = TaskToEdit.CompletedAt;
                Priority = null; // Not in model, placeholder for future
                DelegatedTo = TaskToEdit.DelegatedTo;
                Status = TaskToEdit.IsCompleted ? "Completed" : (TaskToEdit.IsBlocked ? "Blocked" : "Open");
                BlockerReason = TaskToEdit.BlockerReason;
            }
        }

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
            EisenhowerTask task;
            if (TaskToEdit != null)
            {
                task = TaskToEdit;
                task.Title = Title.Trim();
                task.Description = Description;
                task.Quadrant = Quadrant;
                task.DelegatedTo = DelegatedTo;
                task.UpdatedAt = now;
                if (CompletedAt.HasValue)
                    task.CompletedAt = CompletedAt;
                task.IsCompleted = Status == "Completed";
                task.IsBlocked = Status == "Blocked";
                task.BlockerReason = BlockerReason;
                if (task.IsBlocked && task.BlockedAt == null)
                    task.BlockedAt = now;
                if (!task.IsBlocked)
                    task.BlockedAt = null;
                await TaskRepository.UpdateAsync(task);
            }
            else
            {
                task = new EisenhowerTask
                {
                    Title = Title.Trim(),
                    Description = Description,
                    Quadrant = Quadrant,
                    IsCompleted = Status == "Completed",
                    IsBlocked = Status == "Blocked",
                    BlockerReason = BlockerReason,
                    DelegatedTo = DelegatedTo,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                if (CompletedAt.HasValue)
                    task.CompletedAt = CompletedAt;
                if (task.IsBlocked)
                    task.BlockedAt = now;
                await TaskRepository.InsertAsync(task);
            }
            isSaving = false;
            await OnSaved.InvokeAsync();
        }
    }
}
