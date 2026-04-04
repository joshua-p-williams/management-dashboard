using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface IImageCaptureService
    {
        /// <summary>
        /// Captures an image from the device camera
        /// </summary>
        /// <returns>Image stream or null if cancelled/failed</returns>
        Task<Stream?> CaptureFromCameraAsync();

        /// <summary>
        /// Allows user to select an image from device gallery
        /// </summary>
        /// <returns>Image stream or null if cancelled/failed</returns>
        Task<Stream?> SelectFromGalleryAsync();

        /// <summary>
        /// Processes and validates image for OCR (size, format checks)
        /// </summary>
        /// <param name="imageStream">Original image stream</param>
        /// <returns>Processed image data ready for OCR</returns>
        Task<byte[]?> ProcessImageForOCRAsync(Stream imageStream);

        /// <summary>
        /// Checks if camera permissions are granted
        /// </summary>
        /// <returns>True if camera access is available</returns>
        Task<bool> IsCameraAvailableAsync();

        /// <summary>
        /// Checks if photo gallery access is available
        /// </summary>
        /// <returns>True if gallery access is available</returns>
        Task<bool> IsGalleryAvailableAsync();
    }
}