using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;
using Microsoft.Maui.Storage;
using System.Diagnostics;

#if WINDOWS
using NAudio.Wave;
#endif

namespace ManagementDashboard.Services
{
    public class AudioCaptureService : IAudioCaptureService
    {
        private readonly ISettingsService _settingsService;
        private readonly Stopwatch _recordingStopwatch;
        private readonly Timer? _audioLevelTimer;
        private bool _isRecording;
        private bool _isPaused;
        private byte[]? _currentRecordingData;

#if WINDOWS
        private WaveInEvent? _waveIn;
        private MemoryStream? _recordingStream;
        private WaveFileWriter? _waveFileWriter;
#endif

        public AudioCaptureService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _recordingStopwatch = new Stopwatch();
        }

        public TimeSpan RecordingDuration => _recordingStopwatch.Elapsed;

        public float AudioLevel { get; private set; } = 0.0f;

        public bool IsRecording => _isRecording;

        public bool IsPaused => _isPaused;

        public event EventHandler<bool>? RecordingStateChanged;
        public event EventHandler<float>? AudioLevelChanged;

        public async Task<bool> StartRecordingAsync()
        {
            try
            {
                if (!await IsMicrophoneAvailableAsync())
                    return false;

                if (_isRecording)
                    return false;

                _isRecording = true;
                _isPaused = false;
                _recordingStopwatch.Start();
                
                // Start mock recording - in a real implementation, this would start platform-specific audio capture
                await StartPlatformRecordingAsync();
                
                RecordingStateChanged?.Invoke(this, true);
                return true;
            }
            catch (Exception)
            {
                _isRecording = false;
                return false;
            }
        }

        public async Task<byte[]?> StopRecordingAsync()
        {
            try
            {
                if (!_isRecording)
                    return null;

                _isRecording = false;
                _isPaused = false;
                _recordingStopwatch.Stop();

                // Stop platform-specific recording and get the audio data
                var audioData = await StopPlatformRecordingAsync();
                
                RecordingStateChanged?.Invoke(this, false);
                _recordingStopwatch.Reset();
                
                return audioData;
            }
            catch (Exception)
            {
                await CancelRecordingAsync();
                return null;
            }
        }

        public async Task<bool> PauseRecordingAsync()
        {
            try
            {
                if (!_isRecording || _isPaused)
                    return false;

                _isPaused = true;
                _recordingStopwatch.Stop();
                
                // Pause platform-specific recording
                await PausePlatformRecordingAsync();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ResumeRecordingAsync()
        {
            try
            {
                if (!_isRecording || !_isPaused)
                    return false;

                _isPaused = false;
                _recordingStopwatch.Start();
                
                // Resume platform-specific recording
                await ResumePlatformRecordingAsync();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CancelRecordingAsync()
        {
            try
            {
                if (_isRecording)
                {
                    _isRecording = false;
                    _isPaused = false;
                    _recordingStopwatch.Stop();
                    _recordingStopwatch.Reset();
                    
                    // Cancel platform-specific recording
                    await CancelPlatformRecordingAsync();
                    
                    RecordingStateChanged?.Invoke(this, false);
                }
            }
            catch (Exception)
            {
                // Swallow exceptions during cancel
            }
        }

        public async Task<bool> IsMicrophoneAvailableAsync()
        {
            try
            {
                // For Windows, we'll handle permissions differently to avoid AppxManifest issues
#if WINDOWS
                // On Windows, microphone access is typically available by default
                // We can try to access it directly and handle exceptions if permission is denied
                return await CheckWindowsMicrophoneAccessAsync();
#else
                var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Microphone>();
                }

                return status == PermissionStatus.Granted;
#endif
            }
            catch (Exception ex)
            {
                var note = $"Error checking microphone permission: {ex.Message}";
                return false;
            }
        }

#if WINDOWS
        private async Task<bool> CheckWindowsMicrophoneAccessAsync()
        {
            try
            {
                // For now, assume microphone is available on Windows
                // In a production app, you might want to try to actually access the microphone device
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }
#endif

        public async Task<byte[]?> ProcessAudioForSpeechAsync(byte[] audioData)
        {
            try
            {
                if (audioData == null || audioData.Length == 0)
                    return null;

                // Check if audio data meets minimum requirements
                if (audioData.Length < GetMinimumAudioDataSize())
                    return null;

                // For now, return the audio data as-is
                // In a real implementation, we might convert sample rate, bit depth, etc.
                return await Task.FromResult(audioData);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Platform-Specific Methods

        private async Task StartPlatformRecordingAsync()
        {
#if WINDOWS
            await StartWindowsRecordingAsync();
#else
            // Mock implementation for other platforms
            await Task.Delay(100); // Simulate initialization delay
            StartAudioLevelMonitoring();
#endif
        }

        private async Task<byte[]?> StopPlatformRecordingAsync()
        {
#if WINDOWS
            return await StopWindowsRecordingAsync();
#else
            // Mock implementation for other platforms
            StopAudioLevelMonitoring();
            await Task.Delay(100); // Simulate processing delay
            return new byte[1024]; // Mock audio data
#endif
        }

        private async Task PausePlatformRecordingAsync()
        {
#if WINDOWS
            await PauseWindowsRecordingAsync();
#else
            // Mock implementation
            StopAudioLevelMonitoring();
            await Task.Delay(50);
#endif
        }

        private async Task ResumePlatformRecordingAsync()
        {
#if WINDOWS
            await ResumeWindowsRecordingAsync();
#else
            // Mock implementation
            StartAudioLevelMonitoring();
            await Task.Delay(50);
#endif
        }

        private async Task CancelPlatformRecordingAsync()
        {
#if WINDOWS
            await CancelWindowsRecordingAsync();
#else
            // Mock implementation
            StopAudioLevelMonitoring();
            await Task.Delay(50);
#endif
        }

#if WINDOWS
        private async Task StartWindowsRecordingAsync()
        {
            try
            {
                // Configure audio format for Azure Speech Services
                // 16kHz, 16-bit, mono PCM format
                var waveFormat = new WaveFormat(
                    AzureSpeechConstants.DefaultSampleRate,
                    AzureSpeechConstants.DefaultBitDepth,
                    AzureSpeechConstants.DefaultChannels
                );

                _waveIn = new WaveInEvent
                {
                    WaveFormat = waveFormat,
                    DeviceNumber = 0, // Default microphone
                    BufferMilliseconds = 100 // 100ms buffer
                };

                _recordingStream = new MemoryStream();
                _waveFileWriter = new WaveFileWriter(_recordingStream, waveFormat);

                // Subscribe to data available event
                _waveIn.DataAvailable += OnWindowsAudioDataAvailable;
                _waveIn.RecordingStopped += OnWindowsRecordingStopped;

                // Start recording
                _waveIn.StartRecording();

                // Start audio level monitoring
                StartWindowsAudioLevelMonitoring();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to start Windows audio recording: {ex.Message}", ex);
            }
        }

        private async Task<byte[]?> StopWindowsRecordingAsync()
        {
            try
            {
                if (_waveIn != null)
                {
                    _waveIn.StopRecording();

                    // Wait a bit for the recording to fully stop
                    await Task.Delay(200);

                    // Clean up
                    _waveIn.DataAvailable -= OnWindowsAudioDataAvailable;
                    _waveIn.RecordingStopped -= OnWindowsRecordingStopped;
                    _waveIn.Dispose();
                    _waveIn = null;
                }

                StopWindowsAudioLevelMonitoring();

                // Get the recorded audio data BEFORE disposing the writer
                byte[]? audioData = null;
                if (_waveFileWriter != null)
                {
                    // Flush any remaining data to the stream
                    _waveFileWriter.Flush();

                    // Get audio data before disposing (WaveFileWriter owns and will dispose the stream)
                    if (_recordingStream != null && _recordingStream.Length > 0)
                    {
                        audioData = _recordingStream.ToArray();
                    }

                    // Now dispose the writer (this also disposes the underlying stream)
                    _waveFileWriter.Dispose();
                    _waveFileWriter = null;
                }

                // Stream is disposed by the writer, so just set to null
                _recordingStream = null;

                return audioData;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to stop Windows audio recording: {ex.Message}", ex);
            }
        }

        private async Task PauseWindowsRecordingAsync()
        {
            if (_waveIn != null)
            {
                _waveIn.StopRecording();
                StopWindowsAudioLevelMonitoring();
            }
            await Task.CompletedTask;
        }

        private async Task ResumeWindowsRecordingAsync()
        {
            if (_waveIn != null)
            {
                _waveIn.StartRecording();
                StartWindowsAudioLevelMonitoring();
            }
            await Task.CompletedTask;
        }

        private async Task CancelWindowsRecordingAsync()
        {
            try
            {
                if (_waveIn != null)
                {
                    _waveIn.StopRecording();
                    _waveIn.DataAvailable -= OnWindowsAudioDataAvailable;
                    _waveIn.RecordingStopped -= OnWindowsRecordingStopped;
                    _waveIn.Dispose();
                    _waveIn = null;
                }

                StopWindowsAudioLevelMonitoring();

                _waveFileWriter?.Dispose();
                _waveFileWriter = null;

                _recordingStream?.Dispose();
                _recordingStream = null;
            }
            catch (Exception)
            {
                // Swallow exceptions during cancellation
            }

            await Task.CompletedTask;
        }

        private void OnWindowsAudioDataAvailable(object? sender, WaveInEventArgs e)
        {
            try
            {
                if (_waveFileWriter != null && e.Buffer != null && e.BytesRecorded > 0)
                {
                    _waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

                    // Calculate audio level for visual feedback
                    var level = CalculateAudioLevel(e.Buffer, e.BytesRecorded);
                    AudioLevel = level;
                    AudioLevelChanged?.Invoke(this, level);
                }
            }
            catch (Exception)
            {
                // Handle audio data processing errors gracefully
            }
        }

        private void OnWindowsRecordingStopped(object? sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                // Log the exception or handle it appropriately
            }
        }

        private float CalculateAudioLevel(byte[] buffer, int bytesRecorded)
        {
            try
            {
                if (buffer == null || bytesRecorded <= 0)
                    return 0.0f;

                // Convert bytes to 16-bit samples and calculate RMS
                var sum = 0.0;
                var sampleCount = bytesRecorded / 2; // 16-bit samples = 2 bytes each

                for (int i = 0; i < bytesRecorded - 1; i += 2)
                {
                    var sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                    sum += sample * sample;
                }

                var rms = Math.Sqrt(sum / sampleCount);
                var level = (float)(rms / 32768.0); // Normalize to 0-1 range

                return Math.Min(1.0f, level * 10); // Amplify for better visual feedback
            }
            catch
            {
                return 0.0f;
            }
        }

        private Timer? _windowsLevelTimer;

        private void StartWindowsAudioLevelMonitoring()
        {
            // Audio level is now calculated in real-time from actual audio data
            // This timer is just for fallback if needed
            _windowsLevelTimer = new Timer(_ => { }, null, 0, AzureSpeechConstants.AudioLevelUpdateIntervalMs);
        }

        private void StopWindowsAudioLevelMonitoring()
        {
            _windowsLevelTimer?.Dispose();
            _windowsLevelTimer = null;
            AudioLevel = 0.0f;
            AudioLevelChanged?.Invoke(this, 0.0f);
        }
#endif

        #endregion

        #region Audio Level Monitoring

        private Timer? _levelTimer;

        private void StartAudioLevelMonitoring()
        {
            _levelTimer = new Timer(UpdateAudioLevel, null, 0, AzureSpeechConstants.AudioLevelUpdateIntervalMs);
        }

        private void StopAudioLevelMonitoring()
        {
            _levelTimer?.Dispose();
            _levelTimer = null;
            AudioLevel = 0.0f;
            AudioLevelChanged?.Invoke(this, 0.0f);
        }

        private void UpdateAudioLevel(object? state)
        {
            if (!_isRecording || _isPaused)
                return;

            // Mock audio level simulation - in reality, this would read actual microphone levels
            var random = new Random();
            var newLevel = (float)(random.NextDouble() * 0.8 + 0.1); // Simulate levels between 0.1 and 0.9
            
            AudioLevel = newLevel;
            AudioLevelChanged?.Invoke(this, newLevel);
        }

        #endregion

        #region Helper Methods

        private int GetMinimumAudioDataSize()
        {
            // Calculate minimum data size for 1 second of audio
            // Sample Rate * Channels * (Bit Depth / 8) * Minimum Duration
            return AzureSpeechConstants.DefaultSampleRate * 
                   AzureSpeechConstants.DefaultChannels * 
                   (AzureSpeechConstants.DefaultBitDepth / 8) * 
                   AzureSpeechConstants.MinRecordingDurationSeconds;
        }

        #endregion

        public void Dispose()
        {
            _levelTimer?.Dispose();
            _recordingStopwatch?.Stop();

#if WINDOWS
            _windowsLevelTimer?.Dispose();
            _waveIn?.Dispose();
            _waveFileWriter?.Dispose();
            _recordingStream?.Dispose();
#endif
        }
    }
}