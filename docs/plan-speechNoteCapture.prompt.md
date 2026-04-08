# Plan: Speech Note Capture Implementation

**TL;DR:** Implement Azure AI Speech Services to convert speech to text and route to either Eisenhower Tasks or Work Capture Notes. Build incrementally using existing infrastructure: follow the same patterns as image capture feature, extend Settings with Azure Speech credentials, add new services following established DI patterns, create modal components matching Bootstrap conventions, and integrate with current task/note creation flows. Focus on in-memory audio processing with cross-platform MAUI support.

See docs\feature-speech-note-capture.md for the feature definition.

**Steps**

1. **Add Dependencies & Settings Infrastructure** - Install Azure Speech Services NuGet, extend SettingsService with Azure Speech credentials (subscription key and region), add secure storage for API key, test configuration validation and regional endpoints
2. **Create Core Services** - Implement IAzureSpeechService with speech-to-text functionality, build IAudioCaptureService for MAUI microphone access, create audio processing utilities for task/note routing, add real-time recognition capabilities
3. **Build Audio Capture UI** - Create AudioRecordingModal.razor for microphone recording controls, add audio visualization and recording status indicators, implement SpeechResultsModal.razor for transcribed text editing and routing decisions
4. **Extend WorkCaptureNote Integration** - Add speech source tracking to existing model (optional), update repository methods to handle speech-sourced notes, leverage existing WorkCaptureNoteModal for speech text input
5. **Add Task Creation from Speech** - Extend TaskService with CreateTaskFromSpeechText method, integrate with existing TaskEditor modal, add smart text parsing to suggest quadrants and extract titles from spoken content
6. **Settings Panel Integration** - Add Azure Speech Services configuration section to existing Settings page, implement feature gating when credentials not configured, add validation, region selection, and connection testing UI
7. **Cross-Platform Audio Access** - Implement platform-specific microphone permissions, add MAUI audio recording capabilities, handle real-time audio streaming, test on Windows and Android platforms
8. **Real-time Recognition & Polish** - Add continuous speech recognition for longer sessions, implement comprehensive error handling for network/audio failures, add loading states and audio level indicators, test offline scenarios and permission handling

**Verification**

Run the app, configure Azure Speech credentials in Settings, record audio via microphone, verify speech-to-text transcription, create both Task and Work Note from transcribed text, confirm data persists correctly, test real-time recognition, test error scenarios and recording controls.

**Decisions**

- **Azure AI Speech Services**: Chose cloud speech recognition for professional accuracy and language support over local speech APIs
- **In-Memory Processing**: Audio processed in real-time and discarded after transcription, no file system storage needed for privacy
- **Cross-Platform**: Build for both Windows and Android simultaneously using MAUI audio abstractions and platform-specific implementations
- **Existing Infrastructure**: Leverage current WorkCaptureNote model, Settings patterns, Bootstrap modal conventions, and task creation flows from image capture feature
- **Real-time Recognition**: Support both batch (record then transcribe) and continuous (real-time) recognition modes
- **Security First**: Audio data never persisted locally, subscription keys in secure storage, explicit microphone permissions

**Architecture Alignment**

Follow identical patterns from image capture implementation:
- Service interfaces in ManagementDashboard.Core/Contracts/
- Service implementations in ManagementDashboard.Core/Services/ 
- DI registration in MauiProgram.cs
- Settings integration in existing SettingsService
- Modal components in ManagementDashboard/Components/
- Main dashboard integration in Home.razor/Home.razor.cs
- Cross-platform service implementation in ManagementDashboard/Services/