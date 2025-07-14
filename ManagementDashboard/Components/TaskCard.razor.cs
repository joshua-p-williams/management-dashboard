using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Storage;

namespace ManagementDashboard.Components
{
    public partial class TaskCard : ComponentBase
    {
        [Parameter] public EisenhowerTask Task { get; set; } = default!;
        [Parameter] public EventCallback OnEdit { get; set; }
        [Parameter] public EventCallback OnDelete { get; set; }
        [Parameter] public EventCallback OnComplete { get; set; }
        [Parameter] public EventCallback<string> OnMoveToQuadrant { get; set; }

        [Inject] public IAppPreferences AppPreferences { get; set; } = default!;

        [Inject] private IEisenhowerTaskRepository TaskRepository { get; set; } = default!;

        private static readonly string[] Quadrants = new[] { "Do", "Schedule", "Delegate", "Delete" };

        private int OverdueThresholdDays => AppPreferences?.GetInt("OverdueThresholdDays", 2) ?? 2;

        private bool showAuditTrail = false;
        private bool showDeleteConfirm = false;
        private bool showTaskEditor = false;
        private string? taskEditorQuadrant = null;

        private void RequestAuditTrail()
        {
            showAuditTrail = true;
        }

        private void CloseAuditTrail()
        {
            showAuditTrail = false;
        }

        private void RequestDelete()
        {
            showDeleteConfirm = true;
        }

        private async Task ConfirmDeleteAsync()
        {
            // Call repository to delete
            await TaskRepository.DeleteAsync(Task.Id);
            showDeleteConfirm = false;
            // Optionally, raise an event to parent for refresh
            if (OnDelete.HasDelegate)
            {
                await OnDelete.InvokeAsync();
            }
        }

        private void CancelDelete()
        {
            showDeleteConfirm = false;
        }

        private async Task OnMoveTo(string quadrant)
        {
            await TaskRepository.MoveTaskToQuadrantAsync(Task, quadrant);
            if (OnMoveToQuadrant.HasDelegate)
            {
                await OnMoveToQuadrant.InvokeAsync(quadrant);
            }
        }

        private async Task OnCompleteTask()
        {
            await TaskRepository.CompleteTaskAsync(Task);
            if (OnComplete.HasDelegate)
            {
                await OnComplete.InvokeAsync();
            }
        }

        private void RequestEdit()
        {
            taskEditorQuadrant = Task.Quadrant;
            showTaskEditor = true;
        }

        private void CloseTaskEditor()
        {
            showTaskEditor = false;
            taskEditorQuadrant = null;
        }

        private async Task OnTaskSaved()
        {
            showTaskEditor = false;
            taskEditorQuadrant = null;
            if (OnEdit.HasDelegate)
            {
                await OnEdit.InvokeAsync();
            }
        }

    }
}
