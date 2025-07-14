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
        [Parameter] public EventCallback OnAuditTrail { get; set; }
        [Parameter] public EventCallback<string> OnMoveToQuadrant { get; set; }

        [Inject] public IAppPreferences AppPreferences { get; set; } = default!;

        [Inject] private IEisenhowerTaskRepository TaskRepository { get; set; } = default!;

        private static readonly string[] Quadrants = new[] { "Do", "Schedule", "Delegate", "Delete" };

        private int OverdueThresholdDays => AppPreferences?.GetInt("OverdueThresholdDays", 2) ?? 2;

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
    }
}
