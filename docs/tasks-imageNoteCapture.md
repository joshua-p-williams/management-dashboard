# 📸 Iterative Task List: Image Note Capture Feature

See docs\plan-imageNoteCapture.prompt.md and docs\feature-image-note-capture.md.

This extends the **Management Dashboard Platform** with Azure Computer Vision OCR capabilities to capture handwritten notes, whiteboards, and documents and convert them into Tasks or Work Capture Notes.

Each task will be:

* Scoped to fit within a session or Pomodoro (20–60 min)
* End in a build/test/commit checkpoint
* Named clearly for versioning or commit messages
* Build incrementally on existing infrastructure

---

## 📸 Phase 6: Image Note Capture Feature

### [6.1] Dependencies & Settings Infrastructure

* [ ] 📦 Install `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` NuGet package
* [ ] 📦 Install `Microsoft.Maui.Essentials` for MediaPicker (if not already present)
* [ ] 🔧 Add Azure Vision settings properties to `ISettingsService`
* [ ] 🔐 Implement secure storage for Azure API Key using MAUI SecureStorage
* [ ] 🧪 Build & test settings read/write with dummy values

> 💬 **Checkpoint:** Azure Vision NuGet installed, settings infrastructure ready, SecureStorage working.

---

### [6.2] Core OCR Service Implementation

* [ ] 🏗️ Create `IAzureVisionService` contract in `ManagementDashboard.Core/Contracts/`
* [ ] 🛠️ Implement `AzureVisionService` in `ManagementDashboard.Core/Services/`
* [ ] 🔌 Register `AzureVisionService` in `MauiProgram.cs` DI container
* [ ] 🧪 Add unit test with mock OCR response
* [ ] 🧪 Build & test service instantiation (no Azure calls yet)

> 💬 **Checkpoint:** OCR service skeleton ready, DI wired, basic tests pass.

---

### [6.3] Basic Image Capture Service

* [ ] 🏗️ Create `IImageCaptureService` contract for camera/gallery operations
* [ ] 📱 Implement `ImageCaptureService` using MAUI MediaPicker
* [ ] 🔐 Add camera/storage permission handling for MAUI platforms
* [ ] 🔌 Register `ImageCaptureService` in DI container
* [ ] 🧪 Test camera permission requests on Windows (mock for now)

> 💬 **Checkpoint:** Image capture service ready, permissions handled, camera access working.

---

### [6.4] Image Preview Modal Component

* [ ] 🧱 Create `ImagePreviewModal.razor` component in `/Components/`
* [ ] 🎨 Build Bootstrap modal with image display and action buttons
* [ ] 🔄 Add EventCallback parameters for OnProcess/OnCancel
* [ ] 🧪 Create test page to display modal with sample image
* [ ] 🧪 Test modal open/close and button interactions

> 💬 **Checkpoint:** Image preview modal renders and behaves correctly.

---

### [6.5] OCR Results Modal Component

* [ ] 🧱 Create `OcrResultsModal.razor` for extracted text editing
* [ ] 🎨 Add textarea for OCR text with edit capability
* [ ] 🔘 Add routing buttons: "Create Task" vs "Save as Work Note"
* [ ] 🔄 Wire EventCallbacks for routing decisions
* [ ] 🧪 Test modal with sample extracted text

> 💬 **Checkpoint:** OCR results modal displays text and routing options work.

---

### [6.6] Settings Panel Integration

* [ ] ⚙️ Add Azure Computer Vision section to existing `Settings.razor` page
* [ ] 🔐 Add form fields for Endpoint URL and API Key (use SecureStorage)
* [ ] ✅ Add validation for required fields and URL format
* [ ] 🔒 Implement feature gating - show/hide capture buttons based on config
* [ ] 🧪 Test settings save/load and feature enable/disable logic

> 💬 **Checkpoint:** Settings panel working, Azure credentials configurable, feature gating active.

---

### [6.7] Basic OCR Integration

* [ ] 🔗 Connect `AzureVisionService` to real Azure Computer Vision API
* [ ] 🔄 Implement image-to-text flow: capture → preview → OCR → results
* [ ] ⏳ Add loading states and progress indicators during OCR processing
* [ ] ❌ Add error handling for network failures and invalid API keys
* [ ] 🧪 Test end-to-end OCR with real Azure credentials

> 💬 **Checkpoint:** Complete OCR flow working - image to extracted text.

---

### [6.8] Work Capture Note Integration

* [ ] 🗃️ Extend `WorkCaptureNote` model to track OCR source (add `IsFromOcr` property)
* [ ] 🚀 Create database migration to add OCR tracking fields
* [ ] 🔄 Update `WorkCaptureNoteRepository` to handle OCR-sourced notes
* [ ] 🧱 Add "Capture Note" button to existing Work Capture UI
* [ ] 🧪 Test creating Work Capture Notes from OCR text

> 💬 **Checkpoint:** Work Capture Notes can be created from OCR text, data persists correctly.

---

### [6.9] Task Creation from OCR

* [ ] 🏗️ Extend `ITaskService` with `CreateTaskFromOcrAsync` method
* [ ] 🧠 Add smart text parsing to suggest task title from first line
* [ ] 🎯 Add quadrant suggestion logic based on keywords
* [ ] 🔗 Wire OCR results modal to existing task creation flow
* [ ] 🧪 Test creating Eisenhower Tasks from OCR text

> 💬 **Checkpoint:** Tasks can be created from OCR, smart parsing suggests titles and quadrants.

---

### [6.10] Main Dashboard Integration

* [ ] 📸 Add "Capture Note" button to main dashboard/navigation
* [ ] 🔄 Wire complete flow: button → camera → preview → OCR → routing → save
* [ ] 🎨 Style capture button with camera icon, make prominent but not intrusive
* [ ] 🧪 Test complete user journey from main page
* [ ] 🧪 Test both Task and Work Note creation paths

> 💬 **Checkpoint:** Complete feature integrated into main UI, full user flow working.

---

### [6.11] Cross-Platform Camera Testing

* [ ] 📱 Test camera capture on Windows desktop (webcam)
* [ ] 📱 Test gallery selection on Windows (file picker)
* [ ] 🤖 Test camera capture on Android (if available)
* [ ] 🤖 Test gallery selection on Android (if available)
* [ ] 🔧 Fix any platform-specific issues

> 💬 **Checkpoint:** Camera and gallery access working on target platforms.

---

### [6.12] Error Handling & Polish

* [ ] ❌ Add comprehensive error messages for common failures (network, invalid image, etc.)
* [ ] ⏳ Improve loading states with better progress indicators
* [ ] 🧹 Add input validation (image size, format checking)
* [ ] 💡 Add user hints and help text for first-time usage
* [ ] 🧪 Test error scenarios and offline behavior

> 💬 **Checkpoint:** Feature is polished, handles errors gracefully, provides good UX.

---

### [6.13] Documentation & Testing

* [ ] 📝 Update feature documentation with implementation details
* [ ] 🧪 Add integration tests for OCR service
* [ ] 🧪 Add UI tests for modal interactions
* [ ] 📖 Add inline help/tooltips for Azure configuration
* [ ] 🏷️ Tag commit as feature completion (`feat/image-note-capture`)

> 💬 **Checkpoint:** Feature complete, documented, tested, ready for production use.

---

## 🔮 Phase 6+: Future Enhancements

* [ ] 🧠 Add Azure OpenAI integration for smart text parsing and task categorization
* [ ] 📱 Add batch image processing capabilities
* [ ] 🔄 Add offline OCR fallback using local libraries
* [ ] 🎯 Add template recognition for common note formats
* [ ] 🗣️ Add voice annotation capabilities before OCR processing

---

## 🧪 Testing Strategy

### Per-Phase Testing
- **Unit Tests**: Each service method with mocked dependencies
- **Integration Tests**: End-to-end OCR flow with test images
- **UI Tests**: Modal interactions and form validation
- **Cross-Platform**: Camera/gallery on Windows and Android

### Sample Test Images
- Handwritten TODO list
- Whiteboard meeting notes
- Typed document screenshot
- Mixed printed/handwritten content

### Error Scenarios
- Network connectivity issues
- Invalid Azure credentials
- Unsupported image formats
- Large image files
- Empty/unclear images