using Android.Media;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System.Collections.Concurrent;
using Android.Content.PM;
using static Android.Media.AudioRecord;

namespace ManagementDashboard.Platforms.Android
{
    public class AndroidAudioCaptureService
    {
        private AudioRecord? _audioRecord;
        private bool _isRecording;
        private readonly ConcurrentQueue<byte[]> _audioDataQueue = new();
        private Task? _recordingTask;
        private CancellationTokenSource? _cancellationTokenSource;

        // Audio format constants matching Azure Speech requirements
        private const int SAMPLE_RATE = 16000;
        private const ChannelIn CHANNEL_CONFIG = ChannelIn.Mono;
        private const Encoding AUDIO_FORMAT = Encoding.Pcm16bit;

        public async Task<bool> StartRecordingAsync()
        {
            try
            {
                if (_isRecording)
                    return false;

                // Check permissions
                if (!HasMicrophonePermission())
                {
                    return false;
                }

                // Calculate buffer size
                var bufferSize = AudioRecord.GetMinBufferSize(SAMPLE_RATE, CHANNEL_CONFIG, AUDIO_FORMAT);
                if (bufferSize < 0) // Negative values indicate errors
                {
                    return false;
                }

                // Create AudioRecord instance
                _audioRecord = new AudioRecord(
                    AudioSource.Mic,
                    SAMPLE_RATE,
                    CHANNEL_CONFIG,
                    AUDIO_FORMAT,
                    bufferSize * 2 // Use double buffer size for safety
                );

                if (_audioRecord.State != State.Initialized)
                {
                    _audioRecord?.Release();
                    _audioRecord = null;
                    return false;
                }

                // Start recording
                _audioRecord.StartRecording();
                _isRecording = true;

                // Start background recording task
                _cancellationTokenSource = new CancellationTokenSource();
                _recordingTask = RecordAudioAsync(_cancellationTokenSource.Token);

                return true;
            }
            catch (Exception)
            {
                await StopRecordingAsync();
                return false;
            }
        }

        public async Task<byte[]?> StopRecordingAsync()
        {
            if (!_isRecording || _audioRecord == null)
                return null;

            try
            {
                _isRecording = false;

                // Cancel the recording task
                _cancellationTokenSource?.Cancel();

                // Wait for recording task to complete
                if (_recordingTask != null)
                {
                    await _recordingTask;
                }

                // Stop and release AudioRecord
                _audioRecord.Stop();
                _audioRecord.Release();
                _audioRecord = null;

                // Combine all recorded audio data
                var allAudioData = new List<byte>();
                while (_audioDataQueue.TryDequeue(out var audioChunk))
                {
                    allAudioData.AddRange(audioChunk);
                }

                return allAudioData.Count > 0 ? allAudioData.ToArray() : null;
            }
            catch (Exception)
            {
                _audioRecord?.Release();
                _audioRecord = null;
                return null;
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _recordingTask = null;
            }
        }

        public async Task CancelRecordingAsync()
        {
            if (!_isRecording)
                return;

            try
            {
                _isRecording = false;

                _cancellationTokenSource?.Cancel();

                if (_recordingTask != null)
                {
                    await _recordingTask;
                }

                _audioRecord?.Stop();
                _audioRecord?.Release();
                _audioRecord = null;

                // Clear any queued audio data
                while (_audioDataQueue.TryDequeue(out _)) { }
            }
            catch (Exception)
            {
                // Swallow exceptions during cancel
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _recordingTask = null;
            }
        }

        private async Task RecordAudioAsync(CancellationToken cancellationToken)
        {
            if (_audioRecord == null)
                return;

            var buffer = new byte[1024];

            try
            {
                while (_isRecording && !cancellationToken.IsCancellationRequested)
                {
                    var bytesRead = _audioRecord.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        var audioData = new byte[bytesRead];
                        Array.Copy(buffer, audioData, bytesRead);
                        _audioDataQueue.Enqueue(audioData);
                    }

                    // Small delay to prevent busy waiting
                    await Task.Delay(10, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception)
            {
                // Log error or handle as needed
                _isRecording = false;
            }
        }

        private bool HasMicrophonePermission()
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;

            if (context == null)
                return false;

            return ContextCompat.CheckSelfPermission(context, global::Android.Manifest.Permission.RecordAudio) == Permission.Granted;
        }

        public bool IsRecording => _isRecording;
    }
}