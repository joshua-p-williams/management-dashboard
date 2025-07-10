using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components.Pages
{
    public partial class EisenhowerMatrix : ComponentBase
    {
        [Inject] private IEisenhowerTaskRepository TaskRepository { get; set; } = default!;
        private List<EisenhowerTask> DoTasks = new();
        private List<EisenhowerTask> ScheduleTasks = new();
        private List<EisenhowerTask> DelegateTasks = new();
        private List<EisenhowerTask> DeleteTasks = new();
        private List<EisenhowerTask> InboxTasks = new();
        private bool isLoading = true;

        // Modal state for TaskEditor
        private bool showTaskEditor = false;
        private string? taskEditorQuadrant = null;
        private EisenhowerTask? taskToEdit = null;

        // Modal state for TaskAuditTrail
        private bool showAuditTrail = false;
        private EisenhowerTask? auditTrailTask = null;

        // Modal state for delete confirmation
        private bool showDeleteConfirm = false;
        private EisenhowerTask? taskToDelete = null;

        private static readonly string[] Quadrants = new[] { "Do", "Schedule", "Delegate", "Delete" };

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            await LoadTasks();
            isLoading = false;
        }

        private async Task LoadTasks()
        {
            InboxTasks = (await TaskRepository.GetTasksByQuadrantAsync(null))
                .Where(t => string.IsNullOrEmpty(t.Quadrant) || t.Quadrant == "Uncategorized")
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
            DoTasks = (await TaskRepository.GetTasksByQuadrantAsync("Do"))
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
            ScheduleTasks = (await TaskRepository.GetTasksByQuadrantAsync("Schedule"))
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
            DelegateTasks = (await TaskRepository.GetTasksByQuadrantAsync("Delegate"))
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
            DeleteTasks = (await TaskRepository.GetTasksByQuadrantAsync("Delete"))
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
        }

        private void OpenAddTaskModalDo() => OpenAddTaskModal("Do");
        private void OpenAddTaskModalSchedule() => OpenAddTaskModal("Schedule");
        private void OpenAddTaskModalDelegate() => OpenAddTaskModal("Delegate");
        private void OpenAddTaskModalDelete() => OpenAddTaskModal("Delete");

        private void OpenAddTaskModal(string quadrant)
        {
            taskEditorQuadrant = quadrant;
            taskToEdit = null;
            showTaskEditor = true;
        }

        private void CloseTaskEditor()
        {
            showTaskEditor = false;
            taskEditorQuadrant = null;
            taskToEdit = null;
        }

        private async Task OnTaskSaved()
        {
            showTaskEditor = false;
            taskEditorQuadrant = null;
            taskToEdit = null;
            await LoadTasks();
            StateHasChanged();
        }

        private void EditTask(EisenhowerTask task)
        {
            taskToEdit = task;
            taskEditorQuadrant = task.Quadrant;
            showTaskEditor = true;
        }

        private void RequestDeleteTask(EisenhowerTask task)
        {
            taskToDelete = task;
            showDeleteConfirm = true;
        }

        private void CancelDeleteTask()
        {
            showDeleteConfirm = false;
            taskToDelete = null;
        }

        private async Task ConfirmDeleteTaskAsync()
        {
            if (taskToDelete != null)
            {
                await TaskRepository.DeleteAsync(taskToDelete.Id);
                await LoadTasks();
                StateHasChanged();
            }
            showDeleteConfirm = false;
            taskToDelete = null;
        }

        private async Task CompleteTask(EisenhowerTask task)
        {
            await TaskRepository.CompleteTaskAsync(task);
            await LoadTasks();
            StateHasChanged();
        }
        private void ShowAuditTrail(EisenhowerTask task)
        {
            auditTrailTask = task;
            showAuditTrail = true;
        }

        private void CloseAuditTrail()
        {
            showAuditTrail = false;
            auditTrailTask = null;
        }

        private async Task MoveTaskToQuadrant(EisenhowerTask task, string targetQuadrant)
        {
            await TaskRepository.MoveTaskToQuadrantAsync(task, targetQuadrant);
            await LoadTasks();
            StateHasChanged();
        }

        private string GetQuadrantDisplayName(string quadrant)
        {
            return quadrant switch
            {
                "Do" => "Do Now",
                "Schedule" => "Schedule",
                "Delegate" => "Delegate",
                "Delete" => "Delete",
                _ => quadrant
            };
        }
    }
}
