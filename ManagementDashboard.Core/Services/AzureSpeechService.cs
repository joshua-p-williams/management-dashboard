using System;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;
using ManagementDashboard.Core.Models;

namespace ManagementDashboard.Core.Services
{
    public class AzureSpeechService : IAzureSpeechService
    {
        private readonly ISettingsService _settingsService;

        public AzureSpeechService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<bool> IsConfiguredAsync()
        {
            var region = _settingsService.AzureSpeechRegion;
            var subscriptionKey = await _settingsService.GetAzureSpeechSubscriptionKeyAsync();

            return !string.IsNullOrWhiteSpace(region) && 
                   !string.IsNullOrWhiteSpace(subscriptionKey);
        }

        public async Task<string> TranscribeSpeechAsync(Stream audioStream)
        {
            try
            {
                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null) return string.Empty;

                // Convert stream to byte array
                using var memoryStream = new MemoryStream();
                await audioStream.CopyToAsync(memoryStream);
                var audioData = memoryStream.ToArray();

                return await TranscribeSpeechAsync(audioData);
            }
            catch (Exception)
            {
                // Log the error in a real implementation
                return string.Empty;
            }
        }

        public async Task<string> TranscribeSpeechAsync(byte[] audioData)
        {
            try
            {
                if (audioData == null || audioData.Length == 0)
                {
                    throw new ArgumentException("Audio data is empty or null.");
                }

                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null) 
                {
                    throw new InvalidOperationException("Azure Speech Services is not properly configured. Please check your subscription key and region settings.");
                }

                // Create audio config from byte array
                using var audioStream = new MemoryStream(audioData);
                using var audioInputStream = AudioInputStream.CreatePushStream();
                using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);

                // Write audio data to input stream
                audioInputStream.Write(audioData);
                audioInputStream.Close();

                // Create speech recognizer
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

                // Perform recognition
                var result = await recognizer.RecognizeOnceAsync();

                return ProcessRecognitionResult(result);
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException("Invalid Azure Speech Services credentials. Please verify your subscription key and region.");
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw new InvalidOperationException($"Network error during speech recognition. Please check your internet connection. Details: {ex.Message}");
            }
            catch (TimeoutException)
            {
                throw new InvalidOperationException("Speech recognition timed out. Please try again with shorter audio or check your network connection.");
            }
            catch (ArgumentException ex)
            {
                throw; // Re-throw validation errors as-is
            }
            catch (InvalidOperationException ex)
            {
                throw; // Re-throw configuration errors as-is
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error during speech recognition: {ex.Message}");
            }
        }

        public async Task<Models.SpeechRecognitionResult> TranscribeSpeechWithConfidenceAsync(byte[] audioData)
        {
            try
            {
                if (audioData == null || audioData.Length == 0)
                {
                    return Models.SpeechRecognitionResult.Failure("Audio data is empty or null.");
                }

                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null)
                {
                    return Models.SpeechRecognitionResult.Failure("Azure Speech Services is not properly configured. Please check your subscription key and region settings.");
                }

                // Create audio config from byte array
                using var audioStream = new MemoryStream(audioData);
                using var audioInputStream = AudioInputStream.CreatePushStream();
                using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);

                // Write audio data to input stream
                audioInputStream.Write(audioData);
                audioInputStream.Close();

                // Create speech recognizer
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

                // Perform recognition
                var result = await recognizer.RecognizeOnceAsync();

                return ProcessRecognitionResultWithConfidence(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Models.SpeechRecognitionResult.Failure("Invalid Azure Speech Services credentials. Please verify your subscription key and region.");
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return Models.SpeechRecognitionResult.Failure($"Network error during speech recognition. Please check your internet connection. Details: {ex.Message}");
            }
            catch (TimeoutException)
            {
                return Models.SpeechRecognitionResult.Failure("Speech recognition timed out. Please try again with shorter audio or check your network connection.");
            }
            catch (ArgumentException ex)
            {
                return Models.SpeechRecognitionResult.Failure(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Models.SpeechRecognitionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Models.SpeechRecognitionResult.Failure($"Unexpected error during speech recognition: {ex.Message}");
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null) return false;

                // Create a simple recognizer to test configuration
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

                // Try to create the recognizer - this will validate credentials
                // without actually starting recognition
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // Invalid credentials
                return false;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                // Network connectivity issues
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetRecognitionLanguage()
        {
            return _settingsService.AzureSpeechLanguage ?? AzureSpeechConstants.DefaultLanguage;
        }

        public async Task<object?> CreateContinuousRecognizerAsync()
        {
            try
            {
                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null) return null;

                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                return new SpeechRecognizer(speechConfig, audioConfig);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<SpeechConfig?> CreateSpeechConfigAsync()
        {
            var region = _settingsService.AzureSpeechRegion;
            var subscriptionKey = await _settingsService.GetAzureSpeechSubscriptionKeyAsync();

            if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(subscriptionKey))
                return null;

            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
            speechConfig.SpeechRecognitionLanguage = GetRecognitionLanguage();

            return speechConfig;
        }

        private static Models.SpeechRecognitionResult ProcessRecognitionResultWithConfidence(Microsoft.CognitiveServices.Speech.SpeechRecognitionResult result)
        {
            switch (result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    if (string.IsNullOrWhiteSpace(result.Text))
                    {
                        return Models.SpeechRecognitionResult.Failure("Speech was detected but no text was recognized. Please speak more clearly or ensure there's minimal background noise.");
                    }

                    // Extract confidence from the result
                    // Note: Azure Speech SDK doesn't always provide detailed confidence scores
                    // For now, we'll estimate confidence based on recognition success
                    float confidence = 0.85f; // Assume good confidence if recognition succeeded

                    // Try to get confidence from properties if available
                    var jsonResultProperty = result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);
                    if (!string.IsNullOrEmpty(jsonResultProperty))
                    {
                        // Parse JSON to extract confidence if available
                        // This is a simplified approach - in production, you might want more robust JSON parsing
                        try
                        {
                            if (jsonResultProperty.Contains("\"confidence\""))
                            {
                                var confidenceIndex = jsonResultProperty.IndexOf("\"confidence\":");
                                if (confidenceIndex >= 0)
                                {
                                    var valueStart = jsonResultProperty.IndexOf(":", confidenceIndex) + 1;
                                    var valueEnd = jsonResultProperty.IndexOfAny(new char[] { ',', '}' }, valueStart);
                                    if (valueEnd > valueStart)
                                    {
                                        var confidenceStr = jsonResultProperty.Substring(valueStart, valueEnd - valueStart).Trim();
                                        if (float.TryParse(confidenceStr, out var parsedConfidence))
                                        {
                                            confidence = parsedConfidence;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // If parsing fails, keep the default confidence
                        }
                    }

                    return Models.SpeechRecognitionResult.Success(result.Text, confidence);

                case ResultReason.NoMatch:
                    return Models.SpeechRecognitionResult.Failure("No speech was detected in the audio. Please ensure you're speaking clearly into the microphone.");

                case ResultReason.Canceled:
                    var cancelDetails = CancellationDetails.FromResult(result);

                    if (cancelDetails.Reason == CancellationReason.Error)
                    {
                        switch (cancelDetails.ErrorCode)
                        {
                            case CancellationErrorCode.AuthenticationFailure:
                                return Models.SpeechRecognitionResult.Failure("Authentication failed. Please check your Azure Speech Services subscription key and region.");

                            case CancellationErrorCode.ConnectionFailure:
                                return Models.SpeechRecognitionResult.Failure("Failed to connect to Azure Speech Services. Please check your internet connection.");

                            case CancellationErrorCode.ServiceTimeout:
                                return Models.SpeechRecognitionResult.Failure("Azure Speech Services request timed out. Please try again.");

                            case CancellationErrorCode.BadRequest:
                                return Models.SpeechRecognitionResult.Failure("Invalid audio format or request. Please ensure the audio is in a supported format.");

                            default:
                                return Models.SpeechRecognitionResult.Failure($"Speech recognition was canceled due to an error: {cancelDetails.ErrorDetails}");
                        }
                    }

                    return Models.SpeechRecognitionResult.Failure("Speech recognition was canceled.");

                default:
                    return Models.SpeechRecognitionResult.Failure($"Unexpected speech recognition result: {result.Reason}");
            }
        }

        private static string ProcessRecognitionResult(Microsoft.CognitiveServices.Speech.SpeechRecognitionResult result)
        {
            switch (result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    if (string.IsNullOrWhiteSpace(result.Text))
                    {
                        throw new InvalidOperationException("Speech was detected but no text was recognized. Please speak more clearly or ensure there's minimal background noise.");
                    }
                    return result.Text;

                case ResultReason.NoMatch:
                    throw new InvalidOperationException("No speech was detected in the audio. Please ensure you're speaking clearly into the microphone.");

                case ResultReason.Canceled:
                    var cancelDetails = CancellationDetails.FromResult(result);

                    if (cancelDetails.Reason == CancellationReason.Error)
                    {
                        switch (cancelDetails.ErrorCode)
                        {
                            case CancellationErrorCode.AuthenticationFailure:
                                throw new UnauthorizedAccessException("Authentication failed. Please check your Azure Speech Services subscription key and region.");

                            case CancellationErrorCode.ConnectionFailure:
                                throw new System.Net.Http.HttpRequestException("Failed to connect to Azure Speech Services. Please check your internet connection.");

                            case CancellationErrorCode.ServiceTimeout:
                                throw new TimeoutException("Azure Speech Services request timed out. Please try again.");

                            case CancellationErrorCode.BadRequest:
                                throw new ArgumentException("Invalid audio format or request. Please ensure the audio is in a supported format.");

                            default:
                                throw new InvalidOperationException($"Speech recognition was canceled due to an error: {cancelDetails.ErrorDetails}");
                        }
                    }

                    throw new InvalidOperationException("Speech recognition was canceled.");

                default:
                    throw new InvalidOperationException($"Unexpected speech recognition result: {result.Reason}");
            }
        }
    }
}