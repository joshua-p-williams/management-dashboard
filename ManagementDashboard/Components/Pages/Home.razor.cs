using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components.Pages
{
    public partial class Home : ComponentBase
    {
        protected void GenerateError()
        {
            throw new InvalidOperationException("This is a test error from the Home page.");
        }
    }
}
