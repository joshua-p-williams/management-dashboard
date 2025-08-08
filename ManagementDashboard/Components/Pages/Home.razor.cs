using ManagementDashboard.Data.Models;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components.Pages
{
    public partial class Home : ComponentBase
    {
        private bool showTaskEditor = false;
        private bool showWorkCaptureModal = false;
        protected WorkCaptureNote NewWorkCaptureNote { get; set; } = new();
        private int nextTasksListKey = 0;
        
        private void OnAddWorkCaptureClicked()
        {
            NewWorkCaptureNote = new WorkCaptureNote { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            showWorkCaptureModal = true;
        }

        protected void HandleWorkCaptureSave(WorkCaptureNote note)
        {
            showWorkCaptureModal = false;
        }

        protected void HandleWorkCaptureCancel()
        {
            showWorkCaptureModal = false;
        }

        private void OnAddTaskClicked()
        {
            showTaskEditor = true;
        }

        private void CloseTaskEditor()
        {
            showTaskEditor = false;
        }

        private void OnTaskSaved()
        {
            showTaskEditor = false;
            nextTasksListKey++; // Force NextTasksList to reload
            StateHasChanged();
        }
    }
}
