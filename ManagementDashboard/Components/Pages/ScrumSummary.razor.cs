using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using Microsoft.AspNetCore.Components;

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
        protected DateTime SelectedDate { get; set; } = DateTime.Today;
        protected ScrumTab ActiveTab { get; set; } = ScrumTab.Today;
        protected bool ShowWorkCaptureModal { get; set; } = false;
        protected bool showHelpModal { get; set; } = false;

        protected void SelectTab(ScrumTab tab)
        {
            ActiveTab = tab;
        }

        protected void OpenWorkCaptureModal()
        {
            ShowWorkCaptureModal = true;
        }

        protected void CloseWorkCaptureModal()
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
    }
}