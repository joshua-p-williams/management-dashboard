# 🎤 Iterative Task List: Speech Note Capture Feature

See docs\plan-speechNoteCapture.prompt.md and docs\feature-speech-note-capture.md.

This extends the **Management Dashboard Platform** with Azure AI Speech Services capabilities to capture spoken words, voice memos, and audio discussions and convert them into Tasks or Work Capture Notes.

Each task will be:

* Scoped to fit within a session or Pomodoro (20–60 min)
* End in a build/test/commit checkpoint
* Named clearly for versioning or commit messages
* Build incrementally on existing infrastructure

---

## 🎤 Phase 7: Speech Note Capture Feature

### [7.1] Dependencies & Settings Infrastructure

* [x] 📦 Install `Microsoft.CognitiveServices.Speech` NuGet package to Core project
* [x] 📦 Verify `Microsoft.Maui.Essentials` is available for microphone permissions
* [x] 🔧 Add Azure Speech settings properties to `ISettingsService` (SubscriptionKey, Region, Language)
* [x] 🔐 Implement secure storage for Azure Speech Subscription Key using MAUI SecureStorage
* [x] 🌍 Add region selection enum/constants for Azure Speech regions
* [x] 🧪 Build & test settings read/write with dummy values

> ✅ **Checkpoint:** Azure Speech NuGet installed, settings infrastructure ready, SecureStorage working for speech credentials.

---

### [7.2] Core Speech Service Implementation

* [x] 🏗️ Create `IAzureSpeechService` contract in `ManagementDashboard.Core/Contracts/`
* [x] 🛠️ Implement `AzureSpeechService` in `ManagementDashboard.Core/Services/`
* [x] 🔌 Register `AzureSpeechService` in `MauiProgram.cs` DI container
* [x] 🎯 Add configuration validation and connection testing methods
* [x] 🧪 Add unit test with mock speech recognition response
* [x] 🧪 Build & test service instantiation (no Azure calls yet)

> ✅ **Checkpoint:** Speech service skeleton ready, DI wired, basic tests pass.

---

### [7.3] Basic Audio Capture Service

* [x] 🏗️ Create `IAudioCaptureService` contract for microphone operations
* [x] 📱 Implement `AudioCaptureService` using MAUI platform-specific audio recording
* [x] 🔐 Add microphone permission handling for MAUI platforms
* [x] ⏯️ Implement start/stop/pause recording controls
* [x] 📊 Add recording duration tracking and audio level monitoring
* [x] 🔌 Register `AudioCaptureService` in DI container
* [x] 🧪 Test microphone permission requests on Windows (mock for now)

> ✅ **Checkpoint:** Audio capture service ready, permissions handled, microphone access working.

---

### [7.4] Audio Recording Modal Component

* [x] 🧱 Create `AudioRecordingModal.razor` component in `/Components/`
* [x] 🎨 Build Bootstrap modal with record/stop buttons and audio visualization
* [x] ⏱️ Add recording timer and progress indicators
* [x] 📊 Add visual audio level meters during recording
* [x] 🔄 Add EventCallback parameters for OnRecordingComplete/OnCancel
* [x] 🧪 Create test page to display modal with sample recording controls
* [x] 🧪 Test modal open/close and recording button interactions

> ✅ **Checkpoint:** Audio recording modal renders and behaves correctly with recording controls.

---

### [7.5] Speech Results Modal Component

* [ ] 🧱 Create `SpeechResultsModal.razor` for transcribed text editing (similar to OcrResultsModal)
* [ ] 🎨 Add textarea for speech text with edit capability
* [ ] 🔘 Add routing buttons: "Create Task" vs "Save as Work Note"
* [ ] 🔄 Wire EventCallbacks for routing decisions
* [ ] ✨ Add confidence indicators for transcription quality (optional)
* [ ] 🧪 Test modal with sample transcribed text

> ✅ **Checkpoint:** Speech results modal displays text and routing options work.

---

### [7.6] Settings Panel Integration

* [ ] ⚙️ Add Azure AI Speech Services section to existing `Settings.razor` page
* [ ] 🔐 Add form fields for Subscription Key, Region, and Language selection
* [ ] 🌍 Implement region dropdown with common Azure regions
* [ ] 🗣️ Add language selection dropdown for recognition languages
* [ ] ✅ Add validation for required fields and subscription key format
* [ ] 🔒 Implement feature gating - show/hide speech buttons based on config
* [ ] 🧪 Test settings save/load and feature enable/disable logic

> ✅ **Checkpoint:** Settings panel working, Azure Speech credentials configurable, feature gating active.

---

### [7.7] Basic Speech Recognition Integration

* [ ] 🔗 Connect `AzureSpeechService` to real Azure AI Speech Services API
* [ ] 🔄 Implement audio-to-text flow: record → process → transcribe → results
* [ ] ⏳ Add loading states and progress indicators during transcription
* [ ] ❌ Add error handling for network failures and invalid credentials
* [ ] 🧪 Test end-to-end speech recognition with real Azure credentials
* [ ] 🎯 Add confidence scoring and quality feedback

> ✅ **Checkpoint:** Complete speech recognition flow working - audio to transcribed text.

---

### [7.8] Work Capture Note Integration

* [ ] 🗃️ Optionally extend `WorkCaptureNote` model to track speech source (add `CaptureSource` enum)
* [ ] 🚀 Create database migration for capture source tracking (optional)
* [ ] 🔄 Update `WorkCaptureNoteRepository` to handle speech-sourced notes
* [ ] 🧱 Add "Record Note" button to existing Work Capture UI
* [ ] 🧪 Test creating Work Capture Notes from speech text

> ✅ **Checkpoint:** Work Capture Notes can be created from speech text, data persists correctly.

---

### [7.9] Task Creation from Speech

* [ ] 🏗️ Extend task creation flow to handle speech-sourced content (reuse existing patterns)
* [ ] 🧠 Add smart text parsing to suggest task title from spoken content
* [ ] 🎯 Add quadrant suggestion logic based on speech keywords and context
* [ ] 🔗 Wire speech results modal to existing task creation flow
* [ ] 🧪 Test creating Eisenhower Tasks from speech text
* [ ] 💡 Add speech-specific parsing (handle spoken punctuation, filler words)

> ✅ **Checkpoint:** Tasks can be created from speech, smart parsing suggests titles and quadrants.

---

### [7.10] Main Dashboard Integration

* [ ] 🎤 Add "Record Note" button to main dashboard alongside image capture
* [ ] 🔄 Wire complete flow: button → recording modal → speech processing → routing → save
* [ ] 🎨 Style recording button with microphone icon, group with capture options
* [ ] 📱 Add quick access recording controls to navigation/header
* [ ] 🧪 Test complete user journey from main page
* [ ] 🧪 Test both Task and Work Note creation paths from speech

> ✅ **Checkpoint:** Complete feature integrated into main UI, full user flow working.

---

### [7.11] Real-time Speech Recognition

* [ ] 🔄 Implement continuous speech recognition for longer recording sessions
* [ ] ⚡ Add real-time transcription display during recording
* [ ] ⏸️ Add pause/resume functionality for long recordings
* [ ] 🎛️ Add recording quality controls (sample rate, format options)
* [ ] 📏 Implement maximum recording duration limits with warnings
* [ ] 🧪 Test real-time recognition accuracy and performance

> ✅ **Checkpoint:** Real-time speech recognition working, good user experience for longer sessions.

---

### [7.12] Cross-Platform Audio Testing

* [ ] 📱 Test microphone recording on Windows desktop (default microphone)
* [ ] 🎚️ Test audio level monitoring and visual feedback on Windows
* [ ] 🤖 Test microphone recording on Android (if available) - Code ready, needs device testing
* [ ] 🔊 Test audio quality and recognition accuracy on Android (if available)
* [ ] 🔧 Fix any platform-specific audio recording issues
* [ ] 🔐 Test microphone permissions flow on both platforms

> ✅ **Checkpoint:** Audio recording and speech recognition working on target platforms.

---

### [7.13] Error Handling & Polish

* [ ] ❌ Add comprehensive error messages for common failures (network, microphone, invalid credentials)
* [ ] ⏳ Improve loading states with better progress indicators during transcription
* [ ] 🔍 Add input validation (audio duration, format, silence detection)
* [ ] 💡 Add user hints and help text for first-time usage and best practices
* [ ] 🔇 Handle microphone access denied scenarios gracefully
* [ ] 🌐 Add offline behavior handling (no network connectivity)
* [ ] 🧪 Test error scenarios and permission edge cases

> ✅ **Checkpoint:** Feature is polished, handles errors gracefully, provides excellent UX.

---

### [7.14] Advanced Features & Optimization

* [ ] 🎯 Add confidence thresholds for transcription quality warnings
* [ ] 🔄 Implement retry mechanism for failed transcriptions
* [ ] 📊 Add audio preprocessing (noise reduction, volume normalization)
* [ ] 🗣️ Add multiple language support with auto-detection
* [ ] ⚡ Optimize audio compression for faster cloud processing
* [ ] 💾 Add optional local audio caching for retry scenarios
* [ ] 🧪 Performance testing with various audio conditions

> ✅ **Checkpoint:** Advanced features implemented, performance optimized.

---

### [7.15] Documentation & Testing

* [ ] 📝 Update feature documentation with implementation details
* [ ] 🧪 Add comprehensive integration tests for speech service
* [ ] 🧪 Add UI tests for modal interactions and recording controls
* [ ] 📖 Add inline help/tooltips for Azure Speech configuration
* [ ] 🎤 Create user guide for optimal speech recognition results
* [ ] 🔐 Document privacy and security considerations
* [ ] 🏷️ Tag commit as feature completion (`feat/speech-note-capture`)

> ✅ **Checkpoint:** Feature complete, documented, tested, ready for production use.

---

## 🧪 Testing Strategy

### Per-Phase Testing
- **Unit Tests**: Each service method with mocked Azure Speech dependencies
- **Integration Tests**: End-to-end speech recognition flow with test audio files
- **UI Tests**: Modal interactions, recording controls, and form validation
- **Cross-Platform**: Microphone recording on Windows and Android
- **Performance Tests**: Audio quality, transcription speed, and accuracy metrics

### Sample Test Audio
- Clear speech with task-oriented content
- Meeting discussion with multiple speakers (future)
- Noisy environment speech (background noise)
- Different accents and speaking speeds
- Technical terminology and proper names

### Error Scenarios
- Network connectivity issues during transcription
- Invalid Azure Speech credentials
- Microphone access denied by user
- Audio quality too low for recognition
- Maximum recording duration exceeded
- Silent or very quiet audio input

### Performance Benchmarks
- Recording start latency < 1 second
- Real-time transcription delay < 2 seconds
- Batch transcription for 1-minute audio < 10 seconds
- Transcription accuracy > 90% for clear speech
- Memory usage during recording < 50MB additional

---

## 🔄 Integration with Existing Features

### Shared Components
- Reuse existing task creation modal and flows
- Leverage current WorkCaptureNote infrastructure
- Follow same settings panel patterns as image capture
- Use consistent routing logic for task vs note creation
- Apply same error handling and loading state patterns

### UI Consistency
- Match button styling and placement with image capture
- Use same modal design patterns and Bootstrap classes
- Consistent icon usage (microphone icons throughout)
- Follow existing color scheme and spacing
- Maintain responsive design for all screen sizes

### Data Flow
- Same task creation process as manual and image-sourced tasks
- Identical work note storage and retrieval patterns
- Consistent priority and quadrant suggestion algorithms
- Same dashboard refresh and state management
- Unified search and filtering across all note sources