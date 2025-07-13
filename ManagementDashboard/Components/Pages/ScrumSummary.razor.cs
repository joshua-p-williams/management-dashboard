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
            ShowWorkCaptureModal = false;
            await LoadEntriesAsync();
            StateHasChanged();
        }

        protected Task HandleWorkCaptureEdit(WorkCaptureNote note)
        {
            // Use reference to existing note for editing
            NewWorkCaptureNote = note;
            ShowWorkCaptureModal = true;
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected async Task HandleWorkCaptureDelete(WorkCaptureNote note)
        {
            if (note.Id > 0)
            {
                await NoteRepository.DeleteAsync(note.Id);
                await LoadEntriesAsync();
            }
            StateHasChanged();
        }

        protected void HandleWorkCaptureCancel()
        {
            ShowWorkCaptureModal = false;
        }

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
    }
}