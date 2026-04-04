using System;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Core.Services
{
    public class AzureVisionService : IAzureVisionService
    {
        private readonly ISettingsService _settingsService;

        public AzureVisionService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<bool> IsConfiguredAsync()
        {
            var endpoint = _settingsService.AzureVisionEndpoint;
            var apiKey = await _settingsService.GetAzureVisionApiKeyAsync();

            return !string.IsNullOrWhiteSpace(endpoint) && 
                   !string.IsNullOrWhiteSpace(apiKey);
        }

        public async Task<string> ExtractTextFromImageAsync(Stream imageStream)
        {
            try
            {
                var client = await CreateComputerVisionClientAsync();
                if (client == null) return string.Empty;

                var operation = await client.ReadInStreamAsync(imageStream);
                var operationId = ExtractOperationId(operation.OperationLocation);

                // Poll for results
                ReadOperationResult results;
                do
                {
                    await Task.Delay(1000); // Wait 1 second between polls
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while (results.Status == OperationStatusCodes.Running || 
                       results.Status == OperationStatusCodes.NotStarted);

                if (results.Status == OperationStatusCodes.Succeeded)
                {
                    return ExtractTextFromResults(results);
                }

                return string.Empty;
            }
            catch (Exception)
            {
                // Log the error in a real implementation
                return string.Empty;
            }
        }

        public async Task<string> ExtractTextFromImageAsync(byte[] imageData)
        {
            using var stream = new MemoryStream(imageData);
            return await ExtractTextFromImageAsync(stream);
        }

        private async Task<ComputerVisionClient?> CreateComputerVisionClientAsync()
        {
            var endpoint = _settingsService.AzureVisionEndpoint;
            var apiKey = await _settingsService.GetAzureVisionApiKeyAsync();

            if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
                return null;

            return new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
            {
                Endpoint = endpoint
            };
        }

        private static string ExtractOperationId(string operationLocation)
        {
            // Operation location format: https://region.api.cognitive.microsoft.com/vision/v3.2/read/analyzeResults/{operationId}
            return operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);
        }

        private static string ExtractTextFromResults(ReadOperationResult results)
        {
            var extractedText = new StringBuilder();

            if (results.AnalyzeResult?.ReadResults != null)
            {
                foreach (var page in results.AnalyzeResult.ReadResults)
                {
                    if (page.Lines != null)
                    {
                        foreach (var line in page.Lines)
                        {
                            extractedText.AppendLine(line.Text);
                        }
                    }
                }
            }

            return extractedText.ToString().Trim();
        }
    }
}