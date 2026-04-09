using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Services;
using Microsoft.AspNetCore.Components;

namespace ManagementDashboard.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] public ISettingsService SettingsService { get; set; } = default!;
        [Inject] public IAzureVisionService AzureVisionService { get; set; } = default!;
        [Inject] public IImageCaptureService ImageCaptureService { get; set; } = default!;
        [Inject] public IAzureSpeechService SpeechService { get; set; } = default!;
        [Inject] public IAudioCaptureService AudioCaptureService { get; set; } = default!;
        [Inject] public IWorkCaptureNoteRepository WorkCaptureRepository { get; set; } = default!;
        [Inject] public IEisenhowerTaskRepository TaskRepository { get; set; } = default!;

        private bool showTaskEditor = false;
        private bool showWorkCaptureModal = false;
        private bool showImagePreview = false;
        private bool showOcrResults = false;
        private bool isProcessingOcr = false;
        private bool showAudioRecording = false;
        private bool showSpeechResults = false;
        private bool isProcessingSpeech = false;
        private string? errorMessage = null;

        protected WorkCaptureNote NewWorkCaptureNote { get; set; } = new();
        private int nextTasksListKey = 0;

        private byte[]? capturedImageData;
        private string extractedText = string.Empty;
        private byte[]? capturedAudioData;
        private string transcribedText = string.Empty;
        private float? transcriptionConfidence = null;

        protected bool IsImageCaptureEnabled { get; set; } = false;
        protected bool IsSpeechCaptureEnabled { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsImageCaptureEnabled = await SettingsService.IsAzureVisionConfiguredAsync();
            IsSpeechCaptureEnabled = await SettingsService.IsAzureSpeechConfiguredAsync();
            StateHasChanged();
        }

        private void OnAddWorkCaptureClicked()
        {
            NewWorkCaptureNote = new WorkCaptureNote { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            showWorkCaptureModal = true;
        }

        protected void HandleWorkCaptureSave(WorkCaptureNote note)
        {
            showWorkCaptureModal = false;
        }

        protected void HandleWorkCaptureCancel()
        {
            showWorkCaptureModal = false;
        }

        private void OnAddTaskClicked()
        {
            showTaskEditor = true;
        }

        private void CloseTaskEditor()
        {
            showTaskEditor = false;
        }

        private void OnTaskSaved()
        {
            showTaskEditor = false;
            nextTasksListKey++; // Force NextTasksList to reload
            StateHasChanged();
        }

        // Image Capture Methods
        private async Task OnCaptureFromCameraClicked()
        {
            try
            {
                var imageStream = await ImageCaptureService.CaptureFromCameraAsync();
                if (imageStream != null)
                {
                    await ProcessCapturedImage(imageStream);
                }
            }
            catch (Exception)
            {
                // Handle error - in production, show user-friendly error message
            }
        }

        private async Task OnSelectFromGalleryClicked()
        {
            try
            {
                var imageStream = await ImageCaptureService.SelectFromGalleryAsync();
                if (imageStream != null)
                {
                    await ProcessCapturedImage(imageStream);
                }
            }
            catch (Exception)
            {
                // Handle error - in production, show user-friendly error message
            }
        }

        private async Task ProcessCapturedImage(Stream imageStream)
        {
            try
            {
                capturedImageData = await ImageCaptureService.ProcessImageForOCRAsync(imageStream);
                if (capturedImageData != null)
                {
                    showImagePreview = true;
                    StateHasChanged();
                }
            }
            catch (Exception)
            {
                // Handle error
            }
            finally
            {
                imageStream?.Dispose();
            }
        }

        private void HandleImagePreviewCancel()
        {
            showImagePreview = false;
            capturedImageData = null;
            StateHasChanged();
        }

        private async Task HandleImageProcess(byte[] imageData)
        {
            showImagePreview = false;
            isProcessingOcr = true;
            StateHasChanged();

            try
            {
                extractedText = await AzureVisionService.ExtractTextFromImageAsync(imageData);
                isProcessingOcr = false;
                showOcrResults = true;
                StateHasChanged();
            }
            catch (Exception)
            {
                isProcessingOcr = false;
                extractedText = string.Empty;
                // Handle error - show user message
                StateHasChanged();
            }
        }

        private void HandleOcrResultsCancel()
        {
            showOcrResults = false;
            extractedText = string.Empty;
            capturedImageData = null;
            StateHasChanged();
        }

        private async Task HandleCreateTaskFromOcr(string text)
        {
            showOcrResults = false;

            // Create a new task with the OCR text
            var newTask = new EisenhowerTask
            {
                Title = ExtractTaskTitle(text),
                Description = text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Quadrant = null, //SuggestQuadrant(text).ToString(), // Convert enum to string
                Priority = PriorityLevel.Medium // Default priority
            };

            await TaskRepository.InsertAsync(newTask);

            // Refresh the UI
            nextTasksListKey++;
            StateHasChanged();
        }

        private async Task HandleCreateWorkNoteFromOcr(string text)
        {
            showOcrResults = false;

            // Create a new work capture note with the OCR text
            var note = new WorkCaptureNote
            {
                Notes = text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await WorkCaptureRepository.InsertAsync(note);

            StateHasChanged();
        }

        private string ExtractTaskTitle(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "Task from OCR";

            // Use the first line as title, limit to reasonable length
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var title = lines.FirstOrDefault()?.Trim() ?? "Task from OCR";

            return title.Length > 100 ? title.Substring(0, 97) + "..." : title;
        }

        private ManagementDashboard.Core.EisenhowerQuadrant SuggestQuadrant(string text)
        {
            var lowercaseText = text.ToLower();

            // Simple keyword-based quadrant suggestion
            if (lowercaseText.Contains("urgent") || lowercaseText.Contains("asap") || lowercaseText.Contains("emergency"))
            {
                return ManagementDashboard.Core.EisenhowerQuadrant.Do; // Urgent & Important
            }

            if (lowercaseText.Contains("important") || lowercaseText.Contains("critical"))
            {
                return ManagementDashboard.Core.EisenhowerQuadrant.Schedule; // Important, not urgent
            }

            if (lowercaseText.Contains("quick") || lowercaseText.Contains("small") || lowercaseText.Contains("minor"))
            {
                return ManagementDashboard.Core.EisenhowerQuadrant.Delegate; // Urgent, not important
            }

            // Default to "Schedule" for captured tasks
            return ManagementDashboard.Core.EisenhowerQuadrant.Schedule;
        }

        // Speech Capture Methods (following same pattern as image capture)
        private async Task OnRecordNoteClicked()
        {
            try
            {
                errorMessage = null;
                showAudioRecording = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to open recording dialog: {ex.Message}";
                StateHasChanged();
            }
        }

        private void HandleAudioRecordingCancel()
        {
            showAudioRecording = false;
            capturedAudioData = null;
            StateHasChanged();
        }

        private async Task HandleAudioRecordingComplete(byte[] audioData)
        {
            showAudioRecording = false;
            isProcessingSpeech = true;
            capturedAudioData = audioData;
            errorMessage = null;
            transcriptionConfidence = null;
            StateHasChanged();

            try
            {
                if (audioData == null || audioData.Length == 0)
                {
                    throw new ArgumentException("No audio data received. Please try recording again.");
                }

                var speechResult = await SpeechService.TranscribeSpeechWithConfidenceAsync(audioData);

                if (!speechResult.IsSuccess)
                {
                    throw new InvalidOperationException(speechResult.ErrorMessage ?? "Unknown speech recognition error occurred.");
                }

                if (string.IsNullOrWhiteSpace(speechResult.Text))
                {
                    throw new InvalidOperationException("No speech was recognized in the audio. Please speak more clearly and try again.");
                }

                transcribedText = speechResult.Text;
                transcriptionConfidence = speechResult.Confidence;
                isProcessingSpeech = false;
                showSpeechResults = true;
                StateHasChanged();
            }
            catch (UnauthorizedAccessException ex)
            {
                isProcessingSpeech = false;
                errorMessage = $"Authentication Error: {ex.Message}";
                StateHasChanged();
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                isProcessingSpeech = false;
                errorMessage = $"Network Error: {ex.Message}";
                StateHasChanged();
            }
            catch (TimeoutException ex)
            {
                isProcessingSpeech = false;
                errorMessage = $"Timeout Error: {ex.Message}";
                StateHasChanged();
            }
            catch (ArgumentException ex)
            {
                isProcessingSpeech = false;
                errorMessage = ex.Message;
                StateHasChanged();
            }
            catch (InvalidOperationException ex)
            {
                isProcessingSpeech = false;
                errorMessage = ex.Message;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isProcessingSpeech = false;
                errorMessage = $"Unexpected error during speech processing: {ex.Message}";
                StateHasChanged();
            }
        }

        private void HandleSpeechResultsCancel()
        {
            showSpeechResults = false;
            transcribedText = string.Empty;
            transcriptionConfidence = null;
            capturedAudioData = null;
            errorMessage = null;
            StateHasChanged();
        }

        private async Task HandleCreateTaskFromSpeech(string text)
        {
            showSpeechResults = false;

            // Create a new task with the speech text (same logic as OCR)
            var newTask = new EisenhowerTask
            {
                Title = ExtractTaskTitle(text),
                Description = text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Quadrant = null, // SuggestQuadrant(text).ToString(),
                Priority = PriorityLevel.Medium // Default priority
            };

            await TaskRepository.InsertAsync(newTask);

            // Refresh the UI
            nextTasksListKey++;
            StateHasChanged();
        }

        private async Task HandleCreateWorkNoteFromSpeech(string text)
        {
            showSpeechResults = false;

            // Create a new work capture note with the speech text (same logic as OCR)
            var note = new WorkCaptureNote
            {
                Notes = text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await WorkCaptureRepository.InsertAsync(note);

            StateHasChanged();
        }
    }
}
