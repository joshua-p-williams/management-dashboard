using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface IAzureVisionService
    {
        /// <summary>
        /// Checks if Azure Computer Vision is properly configured with endpoint and API key
        /// </summary>
        Task<bool> IsConfiguredAsync();

        /// <summary>
        /// Extracts text from an image stream using Azure Computer Vision OCR
        /// </summary>
        /// <param name="imageStream">The image stream to process</param>
        /// <returns>Extracted text or empty string if extraction fails</returns>
        Task<string> ExtractTextFromImageAsync(Stream imageStream);

        /// <summary>
        /// Extracts text from image byte array using Azure Computer Vision OCR
        /// </summary>
        /// <param name="imageData">The image data as byte array</param>
        /// <returns>Extracted text or empty string if extraction fails</returns>
        Task<string> ExtractTextFromImageAsync(byte[] imageData);
    }
}