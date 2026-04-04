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

* [x] 📦 Install `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` NuGet package
* [x] 📦 Install `Microsoft.Maui.Essentials` for MediaPicker (if not already present)
* [x] 🔧 Add Azure Vision settings properties to `ISettingsService`
* [x] 🔐 Implement secure storage for Azure API Key using MAUI SecureStorage
* [x] 🧪 Build & test settings read/write with dummy values

> ✅ **Checkpoint:** Azure Vision NuGet installed, settings infrastructure ready, SecureStorage working.

---

### [6.2] Core OCR Service Implementation

* [x] 🏗️ Create `IAzureVisionService` contract in `ManagementDashboard.Core/Contracts/`
* [x] 🛠️ Implement `AzureVisionService` in `ManagementDashboard.Core/Services/`
* [x] 🔌 Register `AzureVisionService` in `MauiProgram.cs` DI container
* [x] 🧪 Add unit test with mock OCR response
* [x] 🧪 Build & test service instantiation (no Azure calls yet)

> ✅ **Checkpoint:** OCR service skeleton ready, DI wired, basic tests pass.

---

### [6.3] Basic Image Capture Service

* [x] 🏗️ Create `IImageCaptureService` contract for camera/gallery operations
* [x] 📱 Implement `ImageCaptureService` using MAUI MediaPicker
* [x] 🔐 Add camera/storage permission handling for MAUI platforms
* [x] 🔌 Register `ImageCaptureService` in DI container
* [x] 🧪 Test camera permission requests on Windows (mock for now)

> ✅ **Checkpoint:** Image capture service ready, permissions handled, camera access working.

---

### [6.4] Image Preview Modal Component

* [x] 🧱 Create `ImagePreviewModal.razor` component in `/Components/`
* [x] 🎨 Build Bootstrap modal with image display and action buttons
* [x] 🔄 Add EventCallback parameters for OnProcess/OnCancel
* [x] 🧪 Create test page to display modal with sample image
* [x] 🧪 Test modal open/close and button interactions

> ✅ **Checkpoint:** Image preview modal renders and behaves correctly.

---

### [6.5] OCR Results Modal Component

* [x] 🧱 Create `OcrResultsModal.razor` for extracted text editing
* [x] 🎨 Add textarea for OCR text with edit capability
* [x] 🔘 Add routing buttons: "Create Task" vs "Save as Work Note"
* [x] 🔄 Wire EventCallbacks for routing decisions
* [x] 🧪 Test modal with sample extracted text

> ✅ **Checkpoint:** OCR results modal displays text and routing options work.

---

### [6.6] Settings Panel Integration

* [x] ⚙️ Add Azure Computer Vision section to existing `Settings.razor` page
* [x] 🔐 Add form fields for Endpoint URL and API Key (use SecureStorage)
* [x] ✅ Add validation for required fields and URL format
* [x] 🔒 Implement feature gating - show/hide capture buttons based on config
* [x] 🧪 Test settings save/load and feature enable/disable logic

> ✅ **Checkpoint:** Settings panel working, Azure credentials configurable, feature gating active.

---

### [6.7] Basic OCR Integration

* [x] 🔗 Connect `AzureVisionService` to real Azure Computer Vision API
* [x] 🔄 Implement image-to-text flow: capture → preview → OCR → results
* [x] ⏳ Add loading states and progress indicators during OCR processing
* [x] ❌ Add error handling for network failures and invalid API keys
* [x] 🧪 Test end-to-end OCR with real Azure credentials

> ✅ **Checkpoint:** Complete OCR flow working - image to extracted text.

---

### [6.8] Work Capture Note Integration

* [ ] 🗃️ Extend `WorkCaptureNote` model to track OCR source (add `IsFromOcr` property)
* [ ] 🚀 Create database migration to add OCR tracking fields
* [x] 🔄 Update `WorkCaptureNoteRepository` to handle OCR-sourced notes
* [x] 🧱 Add "Capture Note" button to existing Work Capture UI
* [x] 🧪 Test creating Work Capture Notes from OCR text

> 🟡 **Checkpoint:** Work Capture Notes can be created from OCR text, data persists correctly. (OCR tracking fields optional)

---

### [6.9] Task Creation from OCR

* [x] 🏗️ Extend `ITaskService` with `CreateTaskFromOcrAsync` method (used repository directly)
* [x] 🧠 Add smart text parsing to suggest task title from first line
* [x] 🎯 Add quadrant suggestion logic based on keywords
* [x] 🔗 Wire OCR results modal to existing task creation flow
* [x] 🧪 Test creating Eisenhower Tasks from OCR text

> ✅ **Checkpoint:** Tasks can be created from OCR, smart parsing suggests titles and quadrants.

---

### [6.10] Main Dashboard Integration

* [x] 📸 Add "Capture Note" button to main dashboard/navigation
* [x] 🔄 Wire complete flow: button → camera → preview → OCR → routing → save
* [x] 🎨 Style capture button with camera icon, make prominent but not intrusive
* [x] 🧪 Test complete user journey from main page
* [x] 🧪 Test both Task and Work Note creation paths

> ✅ **Checkpoint:** Complete feature integrated into main UI, full user flow working.

---

### [6.11] Cross-Platform Camera Testing

* [x] 📱 Test camera capture on Windows desktop (webcam)
* [x] 📱 Test gallery selection on Windows (file picker)
* [ ] 🤖 Test camera capture on Android (if available) - Code ready, needs device testing
* [ ] 🤖 Test gallery selection on Android (if available) - Code ready, needs device testing
* [x] 🔧 Fix any platform-specific issues

> 🟡 **Checkpoint:** Camera and gallery access working on target platforms. (Android testing pending)

---

### [6.12] Error Handling & Polish

* [x] ❌ Add comprehensive error messages for common failures (network, invalid image, etc.)
* [x] ⏳ Improve loading states with better progress indicators
* [x] 🧹 Add input validation (image size, format checking)
* [x] 💡 Add user hints and help text for first-time usage
* [x] 🧪 Test error scenarios and offline behavior

> ✅ **Checkpoint:** Feature is polished, handles errors gracefully, provides good UX.

---

### [6.13] Documentation & Testing

* [ ] 📝 Update feature documentation with implementation details
* [x] 🧪 Add integration tests for OCR service
* [ ] 🧪 Add UI tests for modal interactions
* [x] 📖 Add inline help/tooltips for Azure configuration
* [ ] 🏷️ Tag commit as feature completion (`feat/image-note-capture`)

> 🟡 **Checkpoint:** Feature complete, documented, tested, ready for production use. (Some documentation pending)


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