using ManagementDashboard.Data.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public class WorkCaptureNoteModalBase : ComponentBase
    {
        [Parameter]
        public WorkCaptureNote Note { get; set; } = new WorkCaptureNote();

        [Parameter]
        public bool IsEditMode { get; set; } = false;

        [Parameter]
        public List<EisenhowerTask> OpenTasks { get; set; } = new List<EisenhowerTask>();

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<WorkCaptureNote> OnSave { get; set; }

        protected async Task HandleSave()
        {
            await OnSave.InvokeAsync(Note);
        }

        protected async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
