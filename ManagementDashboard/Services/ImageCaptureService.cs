using ManagementDashboard.Core.Contracts;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace ManagementDashboard.Services
{
    public class ImageCaptureService : IImageCaptureService
    {
        private readonly ISettingsService _settingsService;

        public ImageCaptureService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<Stream?> CaptureFromCameraAsync()
        {
            try
            {
                if (!await IsCameraAvailableAsync())
                    return null;

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return null;

                return await photo.OpenReadAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Stream?> SelectFromGalleryAsync()
        {
            try
            {
                if (!await IsGalleryAvailableAsync())
                    return null;

                var photo = await MediaPicker.PickPhotoAsync();
                if (photo == null)
                    return null;

                return await photo.OpenReadAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]?> ProcessImageForOCRAsync(Stream imageStream)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await imageStream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                // Check file size against configured maximum
                var maxSizeBytes = _settingsService.MaxImageSizeMB * 1024 * 1024;
                if (imageData.Length > maxSizeBytes)
                {
                    // In a real implementation, we could resize the image here
                    return null;
                }

                return imageData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsCameraAvailableAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }

                return status == PermissionStatus.Granted && MediaPicker.IsCaptureSupported;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsGalleryAvailableAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Photos>();
                }

                return status == PermissionStatus.Granted;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}