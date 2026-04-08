# 🎤 Feature Definition: Speech Note Capture

## 🎯 Purpose

The **Speech Note Capture** feature leverages **Azure AI Speech Services** to convert spoken words into text for quick note-taking and task creation. Users can record voice memos, meeting discussions, or brainstorming sessions and have the audio transcribed into text that can be intelligently processed and directed to either create new **Eisenhower Matrix Tasks** or capture **Work Notes** for scrum summaries. This enables hands-free note capture and bridges the gap between verbal communication and digital task management.

---

## 🧩 Functional Scope

### 🎛️ Core Functionalities

| Feature                    | Description                                                                 |
| -------------------------- | --------------------------------------------------------------------------- |
| **Audio Recording**        | Record audio directly within the MAUI app using device microphone          |
| **Real-time STT**          | Convert speech to text using Azure Speech Services in real-time             |
| **Batch STT**              | Process recorded audio files for speech-to-text conversion                  |
| **Text Review & Edit**     | Preview transcribed text and manually edit before processing                |
| **Smart Routing**          | Choose to create a new Task or Work Capture Note from transcribed text      |
| **Task Creation**          | Convert speech text to Eisenhower Task with title, description, and quadrant|
| **Work Note Creation**     | Save speech text as Work Capture Note with timestamp                        |
| **Azure Configuration**    | Settings panel for Azure Speech endpoint and API key configuration          |
| **Offline Handling**       | Graceful degradation when Azure credentials not configured                  |
| **Recording Feedback**     | Visual indicators for recording state and audio levels                       |
| **Pause/Resume**           | Control recording with pause and resume functionality                        |

---

## 🔧 Azure AI Speech Services Integration

### 📋 Technical Requirements

| Component                  | Implementation                                                              |
| -------------------------- | --------------------------------------------------------------------------- |
| **NuGet Package**          | `Microsoft.CognitiveServices.Speech` (latest)                              |
| **Authentication**         | Subscription key authentication with service region                         |
| **API Method**             | `SpeechRecognizer` for continuous speech recognition                        |
| **Recognition Mode**       | Continuous recognition with real-time results                               |
| **Supported Formats**      | WAV, MP3, FLAC (16kHz, 16-bit, mono recommended)                          |
| **Languages**              | Configurable language support (default: en-US)                             |
| **Custom Models**          | Support for custom speech models (future enhancement)                       |

### 🗃️ Configuration Settings

| Setting Name              | Description                                | Storage Location        | Required |
| ------------------------- | ------------------------------------------ | ----------------------- | -------- |
| `AzureSpeech.SubscriptionKey` | Azure Speech Services subscription key  | Secure app storage      | Yes      |
| `AzureSpeech.Region`      | Azure Speech Services region (e.g., eastus)| App Preferences         | Yes      |
| `AzureSpeech.Language`    | Recognition language (e.g., en-US)         | App Preferences         | No       |
| `AudioCapture.MaxDurationMin` | Maximum recording duration (minutes)   | App Preferences         | No       |
| `AudioCapture.Quality`    | Audio quality setting (Low/Medium/High)    | App Preferences         | No       |

---

## 🎨 UI/UX Components (via Bootstrap 5)

| Component                  | Role                                                                        |
| -------------------------- | --------------------------------------------------------------------------- |
| **Record Button**          | FAB or button in main dashboard for quick voice recording                   |
| **Recording Modal**        | Display recording controls, audio visualization, and status                 |
| **Speech Results Modal**   | Show transcribed text with edit capability and routing options             |
| **Routing Decision**       | Choice buttons: "Create Task" vs "Save as Work Note"                       |
| **Task Creation Form**     | Pre-populated task form with speech text, quadrant selection               |
| **Work Note Form**         | Simple form to save speech text as work capture note                       |
| **Settings Panel**         | Azure Speech credential configuration within existing Settings page         |
| **Recording Indicators**   | Visual feedback for recording state, audio levels, and duration            |
| **Error Messages**         | User-friendly error handling for API failures and permission issues        |

---

## 🗃️ Data Model Extensions

### New Models Required

```csharp
// No new data models required - feature leverages existing infrastructure
// Tasks created from speech use existing EisenhowerTask model
// Work notes from speech use existing WorkCaptureNote model

// Optional: Add audio source tracking
public enum CaptureSource
{
    Manual,
    Image,
    Speech
}
```

### Settings Storage

```csharp
// Extensions to existing ISettingsService
public interface IAzureSpeechSettings
{
    string? AzureSpeechSubscriptionKey { get; set; }
    string? AzureSpeechRegion { get; set; }
    string AzureSpeechLanguage { get; set; }
    int MaxRecordingDurationMinutes { get; set; }
    bool IsAzureSpeechConfigured { get; }
}
```

---

## 🏗️ Technical Implementation

### 🔌 Service Architecture

```csharp
// Core services to implement
public interface IAzureSpeechService
{
    Task<bool> IsConfiguredAsync();
    Task<string> RecognizeSpeechAsync(Stream audioStream);
    Task<SpeechRecognizer> CreateContinuousRecognizerAsync();
    Task<string> RecognizeContinuousAsync(TimeSpan duration);
}

public interface IAudioCaptureService  
{
    Task<bool> IsMicrophoneAvailableAsync();
    Task<Stream> StartRecordingAsync();
    Task StopRecordingAsync();
    Task<byte[]> GetRecordingDataAsync();
    bool IsRecording { get; }
    TimeSpan RecordingDuration { get; }
}

// Integration with existing services
public interface ITaskService // Extend existing
{
    Task<EisenhowerTask> CreateTaskFromSpeechAsync(string text, string? suggestedQuadrant = null);
}

public interface IWorkCaptureService // Use existing or extend
{
    Task<WorkCaptureNote> CreateNoteFromSpeechAsync(string text);
}
```

### 📱 MAUI Integration

| Platform Feature          | Implementation                                                              |
| -------------------------- | --------------------------------------------------------------------------- |
| **Microphone Access**      | `Microsoft.Maui.Essentials` for microphone permissions                     |
| **Audio Recording**        | Platform-specific audio recording APIs via MAUI handlers                   |
| **Permissions**            | Microphone permissions handled via MAUI platform APIs                      |
| **Audio Processing**       | Temporary in-memory processing, optional local caching                      |
| **Real-time Feedback**     | Audio level visualization during recording                                   |

---

## 🚀 User Workflows

### 🎤 Primary Flow: Voice Recording → Task Creation

1. User taps "Record Note" button from dashboard
2. Microphone permission check/request
3. Recording modal opens with record button
4. User speaks their task or note while recording indicator shows
5. User stops recording or reaches maximum duration
6. Azure Speech Services transcribes audio (with loading indicator)
7. Text preview modal shows transcribed content
8. User reviews/edits text, selects "Create Task"
9. Task creation form pre-populates with speech text
10. User selects quadrant, sets priority, due date
11. New task appears in Eisenhower Matrix

### 📋 Alternative Flow: Voice Recording → Work Note

1. User selects "Record Voice Note" from capture options
2. Records meeting discussion or daily standup notes
3. Speech-to-text processing extracts conversation points
4. User reviews text, selects "Save as Work Note"
5. Work note captures content with timestamp and audio source
6. Note appears in today's Scrum Summary data

### 🔄 Real-time Flow: Continuous Recognition

1. User starts continuous recording mode
2. Speech is transcribed in real-time as user speaks
3. Partial results shown during recording
4. Final results processed when recording stops
5. User can immediately route to task or note creation

---

## ⚙️ Settings Integration

### 🔧 Azure Speech Configuration Panel

Add to existing Settings page:

```html
<!-- Azure Speech Configuration Section -->
<div class="card mb-3">
    <div class="card-header">
        <h5><i class="bi bi-mic"></i> Azure AI Speech Services</h5>
    </div>
    <div class="card-body">
        <div class="mb-3">
            <label class="form-label">Subscription Key</label>
            <input type="password" class="form-control" 
                   placeholder="Your Azure Speech subscription key"
                   @bind="AzureSpeechSubscriptionKey" />
        </div>
        <div class="mb-3">
            <label class="form-label">Region</label>
            <select class="form-select" @bind="AzureSpeechRegion">
                <option value="eastus">East US</option>
                <option value="westus2">West US 2</option>
                <option value="westeurope">West Europe</option>
                <!-- Additional regions -->
            </select>
        </div>
        <div class="mb-3">
            <label class="form-label">Recognition Language</label>
            <select class="form-select" @bind="AzureSpeechLanguage">
                <option value="en-US">English (US)</option>
                <option value="en-GB">English (UK)</option>
                <option value="es-ES">Spanish</option>
                <!-- Additional languages -->
            </select>
        </div>
        <div class="mb-3">
            <label class="form-label">Max Recording Duration (minutes)</label>
            <input type="number" class="form-control" 
                   placeholder="5" min="1" max="30"
                   @bind="MaxRecordingDurationMinutes" />
        </div>
    </div>
</div>
```

### 🔒 Feature Enabling Logic

```csharp
// Speech capture features only enabled when properly configured
public bool IsSpeechCaptureEnabled => 
    !string.IsNullOrWhiteSpace(AzureSpeechSubscriptionKey) && 
    !string.IsNullOrWhiteSpace(AzureSpeechRegion);

// UI components conditionally rendered
@if (SettingsService.IsSpeechCaptureEnabled)
{
    <button class="btn btn-success">
        <i class="bi bi-mic"></i> Record Note
    </button>
}
else
{
    <div class="alert alert-info">
        Configure Azure Speech credentials in Settings to enable voice capture.
    </div>
}
```

---

## 🎯 Success Metrics & Acceptance Criteria

### ✅ Functional Requirements

- [ ] Microphone recording works on target MAUI platforms (Windows, Android)
- [ ] Azure Speech Services successfully transcribes spoken words
- [ ] Transcribed text can create both Tasks and Work Notes
- [ ] Settings panel properly stores and validates Azure Speech credentials
- [ ] Feature is disabled when credentials not configured
- [ ] Recording provides real-time visual feedback (duration, levels)
- [ ] Error handling provides clear user feedback
- [ ] UI is responsive and follows existing design patterns

### 📊 Performance Requirements

- [ ] Recording starts within 1 second of button press
- [ ] Real-time transcription provides feedback during speech
- [ ] Batch transcription completes within 5-10 seconds for typical recordings
- [ ] Transcription accuracy >90% for clear speech in quiet environment
- [ ] Maximum recording duration of 30 minutes supported

---

## 🔮 Future Enhancements

| Enhancement                | Description                                                                 |
| -------------------------- | --------------------------------------------------------------------------- |
| **Custom Speech Models**   | Train custom models for technical terminology and names                     |
| **Speaker Identification** | Identify different speakers in meeting recordings                           |
| **Keyword Spotting**       | Automatically detect task-related keywords during speech                    |
| **Voice Commands**         | Voice control for app navigation and task management                        |
| **Multi-language Support** | Automatic language detection and switching                                  |
| **Audio Compression**      | Compress audio before sending to reduce bandwidth usage                     |
| **Offline Recognition**    | Local speech recognition fallback using device capabilities                 |
| **Meeting Integration**    | Integration with calendar apps to auto-capture meeting notes                |
| **Voice Profiles**         | Personal voice profile training for improved accuracy                       |
| **Audio Editing**          | Basic audio editing capabilities (trim, noise reduction)                    |

---

## 🔐 Security & Privacy Considerations

| Consideration              | Implementation                                                              |
| -------------------------- | --------------------------------------------------------------------------- |
| **Audio Storage**          | Audio processed in-memory only, no persistent storage                       |
| **Credential Security**    | Azure keys stored in platform secure storage                               |
| **Privacy Controls**       | Clear user consent for microphone access and cloud processing              |
| **Data Retention**         | No audio data retained after transcription                                  |
| **Network Security**       | HTTPS-only communication with Azure services                               |

---

## 📚 Related Documentation

- [docs/feature-eisenhower-matrix.md](feature-eisenhower-matrix.md) - Task creation and management
- [docs/feature-scrum-summary.md](feature-scrum-summary.md) - Work capture note integration  
- [docs/feature-image-note-capture.md](feature-image-note-capture.md) - Similar capture feature pattern
- [docs/database-definitions.md](database-definitions.md) - Existing data models
- [Azure AI Speech Services Documentation](https://learn.microsoft.com/en-us/azure/ai-services/speech-service/) - API Reference
- [Speech SDK for .NET](https://learn.microsoft.com/en-us/azure/ai-services/speech-service/quickstarts/setup-platform?pivots=programming-language-csharp) - Setup Guide