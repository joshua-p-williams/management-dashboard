using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public partial class WorkCaptureNoteModal : ComponentBase
    {
        [Inject]
        protected IEisenhowerTaskRepository TaskRepository { get; set; } = default!;


        [Parameter]
        public WorkCaptureNote Note { get; set; } = new WorkCaptureNote();

        [Parameter]
        public bool IsEditMode { get; set; } = false;

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        protected IWorkCaptureNoteRepository NoteRepository { get; set; } = default!;

        protected bool IsAssociateTaskMode { get; set; } = false;
        protected List<EisenhowerTask> OpenTasks { get; set; } = new List<EisenhowerTask>();

        protected async Task EnableAssociateTask()
        {
            IsAssociateTaskMode = true;
            var openTasks = await TaskRepository.GetOpenTasksAsync();
            OpenTasks = openTasks.ToList();
            StateHasChanged();
        }

        protected async Task HandleSave()
        {
            await NoteRepository.InsertAsync(Note);
            await OnCancel.InvokeAsync(); // Close modal after save
        }

        protected async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
