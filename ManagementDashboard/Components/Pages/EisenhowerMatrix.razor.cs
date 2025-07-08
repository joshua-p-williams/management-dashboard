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
        private bool isLoading = true;

        // Modal state for TaskEditor
        private bool showTaskEditor = false;
        private string? newTaskQuadrant = null;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            await LoadTasks();
            isLoading = false;
        }

        private async Task LoadTasks()
        {
            DoTasks = (await TaskRepository.GetTasksByQuadrantAsync("Do")).ToList();
            ScheduleTasks = (await TaskRepository.GetTasksByQuadrantAsync("Schedule")).ToList();
            DelegateTasks = (await TaskRepository.GetTasksByQuadrantAsync("Delegate")).ToList();
            DeleteTasks = (await TaskRepository.GetTasksByQuadrantAsync("Delete")).ToList();
        }

        private void OpenAddTaskModalDo() => OpenAddTaskModal("Do");
        private void OpenAddTaskModalSchedule() => OpenAddTaskModal("Schedule");
        private void OpenAddTaskModalDelegate() => OpenAddTaskModal("Delegate");
        private void OpenAddTaskModalDelete() => OpenAddTaskModal("Delete");

        private void OpenAddTaskModal(string quadrant)
        {
            newTaskQuadrant = quadrant;
            showTaskEditor = true;
        }

        private void CloseTaskEditor()
        {
            showTaskEditor = false;
            newTaskQuadrant = null;
        }

        private async Task OnTaskSaved()
        {
            showTaskEditor = false;
            newTaskQuadrant = null;
            await LoadTasks();
            StateHasChanged();
        }

        private void EditTask(EisenhowerTask task) { /* TODO: Show TaskEditor modal for edit */ }
        private void DeleteTask(EisenhowerTask task) { /* TODO: Confirm and delete */ }
        private void CompleteTask(EisenhowerTask task) { /* TODO: Mark as complete */ }
        private void ShowAuditTrail(EisenhowerTask task) { /* TODO: Show TaskAuditTrail modal */ }
    }
}
