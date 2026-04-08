using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface IAzureSpeechService
    {
        /// <summary>
        /// Checks if Azure AI Speech Services is properly configured with region and subscription key
        /// </summary>
        Task<bool> IsConfiguredAsync();

        /// <summary>
        /// Transcribes speech from an audio stream using Azure Speech Services
        /// </summary>
        /// <param name="audioStream">The audio stream to process</param>
        /// <returns>Transcribed text or empty string if transcription fails</returns>
        Task<string> TranscribeSpeechAsync(Stream audioStream);

        /// <summary>
        /// Transcribes speech from audio byte array using Azure Speech Services
        /// </summary>
        /// <param name="audioData">The audio data as byte array</param>
        /// <returns>Transcribed text or empty string if transcription fails</returns>
        Task<string> TranscribeSpeechAsync(byte[] audioData);

        /// <summary>
        /// Tests the Azure Speech Services connection with current configuration
        /// </summary>
        /// <returns>True if connection test succeeds, false otherwise</returns>
        Task<bool> TestConnectionAsync();

        /// <summary>
        /// Gets the current speech recognition language
        /// </summary>
        /// <returns>Language code (e.g., "en-US")</returns>
        string GetRecognitionLanguage();

        /// <summary>
        /// Creates a continuous speech recognizer for real-time transcription
        /// </summary>
        /// <returns>Speech recognizer instance or null if not configured</returns>
        Task<object?> CreateContinuousRecognizerAsync();
    }
}