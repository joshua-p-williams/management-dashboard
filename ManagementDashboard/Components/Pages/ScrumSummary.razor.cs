using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;

namespace ManagementDashboard.Components.Pages
{
    public partial class ScrumSummary : ComponentBase
    {
        protected DateTime SelectedDate { get; set; } = DateTime.Today;
        protected List<WorkCaptureNote> YesterdayEntries { get; set; } = new();
        protected List<WorkCaptureNote> TodayEntries { get; set; } = new();
        protected List<WorkCaptureNote> BlockerEntries { get; set; } = new();

        // Modal state
        protected bool IsModalOpen { get; set; }
        protected bool IsEdit { get; set; }
        protected string ModalSection { get; set; } = string.Empty;
        protected string ModalTaskTitle { get; set; } = string.Empty;
        protected string ModalNoteText { get; set; } = string.Empty;
        protected string ModalBlockerDetails { get; set; } = string.Empty;

        [Inject]
        protected IWorkCaptureNoteRepository? WorkCaptureNoteRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadEntriesForDate(SelectedDate);
        }

        protected async Task LoadEntriesForDate(DateTime date)
        {
            if (WorkCaptureNoteRepository == null) return;
            var notes = await WorkCaptureNoteRepository.GetNotesByDateAsync(date);
            YesterdayEntries = new List<WorkCaptureNote>();
            TodayEntries = new List<WorkCaptureNote>();
            BlockerEntries = new List<WorkCaptureNote>();
            // For now, all notes go to Today; later, filter by section if needed
            TodayEntries.AddRange(notes);
            StateHasChanged();
        }

        protected void OnAddYesterday() { OpenAddModal("Yesterday"); }
        protected void OnAddToday() { OpenAddModal("Today"); }
        protected void OnAddBlockers() { OpenAddModal("Blockers"); }

        protected void OpenAddModal(string section)
        {
            ModalSection = section;
            IsEdit = false;
            ModalTaskTitle = string.Empty;
            ModalNoteText = string.Empty;
            ModalBlockerDetails = string.Empty;
            IsModalOpen = true;
            StateHasChanged();
        }

        protected void CloseModal()
        {
            IsModalOpen = false;
            StateHasChanged();
        }

        protected async Task SaveModalAsync()
        {
            var entry = new WorkCaptureNote
            {
                Notes = ModalNoteText,
                TaskId = null,
                CreatedAt = SelectedDate,
                UpdatedAt = DateTime.Now
            };
            if (WorkCaptureNoteRepository == null) return;
            await WorkCaptureNoteRepository.InsertAsync(entry);
            await LoadEntriesForDate(SelectedDate);
            IsModalOpen = false;
            StateHasChanged();
        }

        // Update SaveModal to call SaveModalAsync
        protected void SaveModal()
        {
            _ = SaveModalAsync();
        }
    }
}
