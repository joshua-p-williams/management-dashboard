using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public partial class ImagePreviewModal : ComponentBase
    {
        [Parameter]
        public byte[]? ImageData { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<byte[]> OnProcess { get; set; }

        protected string? ImageDataUrl { get; set; }

        protected override void OnParametersSet()
        {
            if (ImageData != null)
            {
                // Convert byte array to data URL for display
                var base64String = Convert.ToBase64String(ImageData);
                ImageDataUrl = $"data:image/jpeg;base64,{base64String}";
            }
            else
            {
                ImageDataUrl = null;
            }
        }

        protected async Task HandleCancel()
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync();
            }
        }

        protected async Task HandleProcess()
        {
            if (ImageData != null && OnProcess.HasDelegate)
            {
                await OnProcess.InvokeAsync(ImageData);
            }
        }
    }
}