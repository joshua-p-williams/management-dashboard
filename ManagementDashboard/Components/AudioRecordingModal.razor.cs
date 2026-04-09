using Microsoft.AspNetCore.Components;
using ManagementDashboard.Core.Contracts;
using System;
using System.Threading.Tasks;

namespace ManagementDashboard.Components
{
    public partial class AudioRecordingModal : ComponentBase, IDisposable
    {
        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<byte[]> OnRecordingComplete { get; set; }

        private bool isProcessing = false;
        private string? errorMessage = null;

        protected override void OnInitialized()
        {
            // Subscribe to audio capture service events
            AudioCaptureService.RecordingStateChanged += OnRecordingStateChanged;
            AudioCaptureService.AudioLevelChanged += OnAudioLevelChanged;
        }

        private async void OnRecordingStateChanged(object? sender, bool isRecording)
        {
            await InvokeAsync(StateHasChanged);
        }

        private async void OnAudioLevelChanged(object? sender, float level)
        {
            await InvokeAsync(StateHasChanged);
        }

        protected async Task StartRecording()
        {
            try
            {
                isProcessing = true;
                errorMessage = null;
                StateHasChanged();

                var success = await AudioCaptureService.StartRecordingAsync();
                if (!success)
                {
                    errorMessage = "Failed to start recording. Please try again.";
                }
            }
            catch (InvalidOperationException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = $"Unexpected error starting recording: {ex.Message}";
            }
            finally
            {
                isProcessing = false;
                StateHasChanged();
            }
        }

        protected async Task StopRecording()
        {
            try
            {
                isProcessing = true;
                errorMessage = null;
                StateHasChanged();

                var audioData = await AudioCaptureService.StopRecordingAsync();

                if (audioData != null && audioData.Length > 0 && OnRecordingComplete.HasDelegate)
                {
                    await OnRecordingComplete.InvokeAsync(audioData);
                }
                else if (audioData == null || audioData.Length == 0)
                {
                    errorMessage = "No audio data captured. Please try recording again and speak clearly into your microphone.";
                }
            }
            catch (InvalidOperationException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = $"Unexpected error stopping recording: {ex.Message}";
            }
            finally
            {
                isProcessing = false;
                StateHasChanged();
            }
        }

        protected async Task HandleCancel()
        {
            try
            {
                // Stop any ongoing recording
                if (AudioCaptureService.IsRecording)
                {
                    await AudioCaptureService.CancelRecordingAsync();
                }

                if (OnCancel.HasDelegate)
                {
                    await OnCancel.InvokeAsync();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error canceling recording: {ex.Message}";
                StateHasChanged();
            }
        }

        protected string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalHours >= 1)
            {
                return duration.ToString(@"hh\:mm\:ss");
            }
            else
            {
                return duration.ToString(@"mm\:ss");
            }
        }

        protected string GetAudioLevelColor()
        {
            var level = AudioCaptureService.AudioLevel;
            
            if (level < 0.2f)
                return "bg-secondary";  // Very low - gray
            else if (level < 0.4f)
                return "bg-info";       // Low - blue
            else if (level < 0.7f)
                return "bg-success";    // Good - green
            else if (level < 0.9f)
                return "bg-warning";    // High - yellow
            else
                return "bg-danger";     // Too high - red
        }

        public void Dispose()
        {
            // Unsubscribe from events to prevent memory leaks
            AudioCaptureService.RecordingStateChanged -= OnRecordingStateChanged;
            AudioCaptureService.AudioLevelChanged -= OnAudioLevelChanged;
        }
    }
}