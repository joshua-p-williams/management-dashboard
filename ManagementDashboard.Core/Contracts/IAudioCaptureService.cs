using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface IAudioCaptureService
    {
        /// <summary>
        /// Starts recording audio from the device microphone
        /// </summary>
        /// <returns>True if recording started successfully</returns>
        Task<bool> StartRecordingAsync();

        /// <summary>
        /// Stops the current audio recording and returns the audio data
        /// </summary>
        /// <returns>Audio data as byte array or null if failed/cancelled</returns>
        Task<byte[]?> StopRecordingAsync();

        /// <summary>
        /// Pauses the current audio recording
        /// </summary>
        /// <returns>True if successfully paused</returns>
        Task<bool> PauseRecordingAsync();

        /// <summary>
        /// Resumes a paused audio recording
        /// </summary>
        /// <returns>True if successfully resumed</returns>
        Task<bool> ResumeRecordingAsync();

        /// <summary>
        /// Cancels the current recording and discards audio data
        /// </summary>
        Task CancelRecordingAsync();

        /// <summary>
        /// Gets the current recording duration
        /// </summary>
        TimeSpan RecordingDuration { get; }

        /// <summary>
        /// Gets the current audio level (0.0 to 1.0)
        /// </summary>
        float AudioLevel { get; }

        /// <summary>
        /// Indicates if currently recording
        /// </summary>
        bool IsRecording { get; }

        /// <summary>
        /// Indicates if recording is paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Checks if microphone permissions are granted
        /// </summary>
        /// <returns>True if microphone access is available</returns>
        Task<bool> IsMicrophoneAvailableAsync();

        /// <summary>
        /// Processes and validates audio data for speech recognition
        /// </summary>
        /// <param name="audioData">Original audio data</param>
        /// <returns>Processed audio data ready for Azure Speech Services</returns>
        Task<byte[]?> ProcessAudioForSpeechAsync(byte[] audioData);

        /// <summary>
        /// Event fired when recording state changes
        /// </summary>
        event EventHandler<bool>? RecordingStateChanged;

        /// <summary>
        /// Event fired when audio level changes during recording
        /// </summary>
        event EventHandler<float>? AudioLevelChanged;
    }
}