using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;
using ManagementDashboard.Core.Extensions;

namespace ManagementDashboard.Components.Pages
{
    public enum ScrumTab
    {
        Yesterday,
        Today,
        Blockers
    }

    public partial class ScrumSummary : ComponentBase
    {
        [Inject] protected IEisenhowerTaskRepository TaskRepository { get; set; } = default!;
        [Inject] protected IWorkCaptureNoteRepository NoteRepository { get; set; } = default!;

        private DateTime _selectedDate = DateTime.Today;
        protected DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    _ = LoadEntriesAsync();
                }
            }
        }
        protected ScrumTab ActiveTab { get; set; } = ScrumTab.Today;
        protected bool ShowWorkCaptureModal { get; set; } = false;
        protected bool showHelpModal { get; set; } = false;

        protected List<EisenhowerTask> YesterdayTasks { get; set; } = new();
        protected List<EisenhowerTask> TodayTasks { get; set; } = new();
        protected List<EisenhowerTask> BlockedTasks { get; set; } = new();
        protected List<WorkCaptureNote> YesterdayNotes { get; set; } = new();
        protected List<WorkCaptureNote> TodayNotes { get; set; } = new();

        protected List<EisenhowerTask> OpenTasks { get; set; } = new();
        protected WorkCaptureNote NewWorkCaptureNote { get; set; } = new();

        protected void SelectTab(ScrumTab tab)
        {
            ActiveTab = tab;
        }

        protected void OpenWorkCaptureModal()
        {
            NewWorkCaptureNote = new WorkCaptureNote { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            ShowWorkCaptureModal = true;
        }


        protected async Task HandleWorkCaptureSave(WorkCaptureNote note)
        {
            await NoteRepository.InsertAsync(note);
            ShowWorkCaptureModal = false;
            await LoadEntriesAsync();
            StateHasChanged();
        }

        protected void HandleWorkCaptureCancel()
        {
            ShowWorkCaptureModal = false;
        }


        protected async Task LoadOpenTasksAsync()
        {
            // Get open tasks: not deleted, not completed before today
            var openTasks = await TaskRepository.GetTasksByQuadrantAsync(null);
            OpenTasks = openTasks.ToList();
        }

        // Remove duplicate OnInitializedAsync if present elsewhere in partial class

        protected void OpenHelpModal()
        {
            showHelpModal = true;
        }

        protected void CloseHelpModal()
        {
            showHelpModal = false;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadEntriesAsync();
        }

        protected async Task LoadEntriesAsync()
        {
            // Yesterday
            var yesterday = SelectedDate.AddDays(-1);
            YesterdayTasks = (await TaskRepository.GetWorkedOnByDateAsync(yesterday.Date)).ToList();
            YesterdayNotes = (await NoteRepository.GetNotesByDateAsync(yesterday.Date)).ToList();
            await AttachTasksToNotes(YesterdayNotes);

            // Today
            TodayTasks = (await TaskRepository.GetWorkedOnByDateAsync(SelectedDate.Date)).ToList();
            TodayNotes = (await NoteRepository.GetNotesByDateAsync(SelectedDate.Date)).ToList();
            await AttachTasksToNotes(TodayNotes);

            // Blockers
            BlockedTasks = (await TaskRepository.GetBlockedAsync()).ToList();

            StateHasChanged();
        }

        protected async Task AttachTasksToNotes(List<WorkCaptureNote> notes)
        {
            foreach (var note in notes)
            {
                if (note.TaskId.HasValue)
                {
                    var task = await TaskRepository.GetAsync(note.TaskId.Value);
                    note.Task = task;
                }
            }
        }

        protected async Task OnDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var newDate))
            {
                SelectedDate = newDate;
                await LoadEntriesAsync();
            }
        }

        protected RenderFragment GetTaskStatusBadge(EisenhowerTask task) => builder =>
        {
            var status = task.GetStatus();
            var badgeClass = status switch
            {
                "Done" => "badge bg-success",
                "Blocked" => "badge bg-danger",
                "Removed" => "badge bg-dark",
                "In Progress" => "badge bg-warning text-dark",
                "Unknown" => "badge bg-secondary",
                _ => "badge bg-secondary"
            };
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", badgeClass);
            builder.AddContent(2, status);
            builder.CloseElement();
        };
    }
}