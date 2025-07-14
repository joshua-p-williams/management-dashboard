using ManagementDashboard.Data.Models;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Services;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components
{
    public partial class NextTasksList : ComponentBase
    {
        [Inject]
        private ITaskService TaskService { get; set; } = default!;

        private int? SelectedCount = 5;
        private List<EisenhowerTask>? Tasks;
        private bool IsLoading = true;
        private string? ErrorMessage;

        protected override async Task OnInitializedAsync()
        {
            await LoadTasksAsync();
        }

        private async Task OnCountChanged(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (value == "all")
                SelectedCount = null;
            else if (int.TryParse(value, out var n))
                SelectedCount = n;
            await LoadTasksAsync();
        }

        private async Task LoadTasksAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            StateHasChanged();
            try
            {
                Tasks = await TaskService.GetNextTasksToWorkOnAsync(SelectedCount);
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load tasks.";
            }
            IsLoading = false;
            StateHasChanged();
        }

        private string? IsSelected(int? count) => SelectedCount == count ? "selected" : null;
    }
}
