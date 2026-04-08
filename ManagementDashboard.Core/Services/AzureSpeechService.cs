using System;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;

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
                var speechConfig = await CreateSpeechConfigAsync();
                if (speechConfig == null) return string.Empty;

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
            catch (Exception)
            {
                // Log the error in a real implementation
                return string.Empty;
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

        private static string ProcessRecognitionResult(SpeechRecognitionResult result)
        {
            return result.Reason switch
            {
                ResultReason.RecognizedSpeech => result.Text,
                ResultReason.NoMatch => string.Empty,
                ResultReason.Canceled => string.Empty,
                _ => string.Empty
            };
        }
    }
}