# 📸 Feature Definition: Image Note Capture

## 🎯 Purpose

The **Image Note Capture** feature leverages **Azure Computer Vision OCR** to extract text from photos of handwritten notes, sticky notes, whiteboards, meeting notes, and other visual content. The extracted text can be intelligently processed and directed to either create new **Eisenhower Matrix Tasks** or capture **Work Notes** for scrum summaries. This bridges the gap between physical note-taking and digital task management.

---

## 🧩 Functional Scope

### 🎛️ Core Functionalities

| Feature                    | Description                                                                 |
| -------------------------- | --------------------------------------------------------------------------- |
| **Camera Capture**         | Take photos directly within the MAUI app using device camera                |
| **Image Import**           | Select existing images from device gallery/file system                      |
| **OCR Processing**         | Extract text from images using Azure Computer Vision Read API               |
| **Text Review & Edit**     | Preview extracted text and manually edit before processing                  |
| **Smart Routing**          | Choose to create a new Task or Work Capture Note from extracted text        |
| **Task Creation**          | Convert OCR text to Eisenhower Task with title, description, and quadrant   |
| **Work Note Creation**     | Save OCR text as Work Capture Note with timestamp                           |
| **Azure Configuration**    | Settings panel for Azure endpoint and API key configuration                 |
| **Offline Handling**       | Graceful degradation when Azure credentials not configured                  |
| **Processing Feedback**    | Loading states and error handling for OCR operations                        |

---

## 🔧 Azure Computer Vision Integration

### 📋 Technical Requirements

| Component                  | Implementation                                                              |
| -------------------------- | --------------------------------------------------------------------------- |
| **NuGet Package**          | `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` (latest)          |
| **Authentication**         | API Key authentication with endpoint URL                                     |
| **API Method**             | `ReadAsync()` for text extraction from images                               |
| **Result Processing**      | Poll `GetReadResultAsync()` until completion                                |
| **Supported Formats**      | JPEG, PNG, BMP, PDF (images within PDFs)                                   |
| **Text Types**             | Printed text, handwritten text, mixed content                               |

### 🗃️ Configuration Settings

| Setting Name              | Description                                | Storage Location        | Required |
| ------------------------- | ------------------------------------------ | ----------------------- | -------- |
| `AzureVision.Endpoint`    | Azure Computer Vision resource endpoint     | App Preferences         | Yes      |
| `AzureVision.ApiKey`      | Azure Computer Vision API key              | Secure app storage      | Yes      |
| `ImageCapture.MaxSizeMB`  | Maximum image size for processing (MB)     | App Preferences         | No       |

---

## 🎨 UI/UX Components (via Bootstrap 5)

| Component                  | Role                                                                        |
| -------------------------- | --------------------------------------------------------------------------- |
| **Camera Button**          | FAB or button in main dashboard for quick image capture                     |
| **Image Preview Modal**    | Display captured/selected image with processing options                     |
| **OCR Results Modal**      | Show extracted text with edit capability and routing options                |
| **Routing Decision**       | Choice buttons: "Create Task" vs "Save as Work Note"                       |
| **Task Creation Form**     | Pre-populated task form with OCR text, quadrant selection                  |
| **Work Note Form**         | Simple form to save OCR text as work capture note                          |
| **Settings Panel**         | Azure credential configuration within existing Settings page                |
| **Progress Indicators**    | Loading spinners for OCR processing                                         |
| **Error Messages**         | User-friendly error handling for API failures                               |

---

## 🗃️ Data Model Extensions

### New Models Required

```csharp
// No new data models required - feature leverages existing infrastructure
// Tasks created from OCR use existing EisenhowerTask model
// Work notes from OCR use existing WorkCaptureNote model
```

### Settings Storage

```csharp
// Extensions to existing ISettingsService
public interface IAzureVisionSettings
{
    string? AzureVisionEndpoint { get; set; }
    string? AzureVisionApiKey { get; set; }
    int MaxImageSizeMB { get; set; }
    bool IsAzureVisionConfigured { get; }
}
```

---

## 🏗️ Technical Implementation

### 🔌 Service Architecture

```csharp
// Core services to implement
public interface IAzureVisionService
{
    Task<bool> IsConfiguredAsync();
    Task<string> ExtractTextFromImageAsync(Stream imageStream);
    Task<string> ExtractTextFromImageAsync(byte[] imageData);
}

public interface IImageCaptureService  
{
    Task<Stream> CaptureFromCameraAsync();
    Task<Stream> SelectFromGalleryAsync();
    Task<byte[]> ProcessImageForOCRAsync(Stream imageStream);
}

// Integration with existing services
public interface ITaskService // Extend existing
{
    Task<EisenhowerTask> CreateTaskFromOCRAsync(string text, string? suggestedQuadrant = null);
}

public interface IWorkCaptureService // Use existing or extend
{
    Task<WorkCaptureNote> CreateNoteFromOCRAsync(string text);
}
```

### 📱 MAUI Integration

| Platform Feature          | Implementation                                                              |
| -------------------------- | --------------------------------------------------------------------------- |
| **Camera Access**          | `Microsoft.Maui.Essentials.MediaPicker` for camera capture                 |
| **Photo Gallery**          | `Microsoft.Maui.Storage.FilePicker` for image selection                    |
| **Permissions**            | Camera and storage permissions handled via MAUI platform APIs              |
| **Image Processing**       | Temporary in-memory processing, no local storage required                   |

---

## 🚀 User Workflows

### 📸 Primary Flow: Camera Capture → Task Creation

1. User taps "Capture Note" button from dashboard
2. MAUI camera interface opens
3. User takes photo of handwritten note/whiteboard
4. Image preview shows with "Process" button
5. Azure OCR extracts text (with loading indicator)
6. Text preview modal shows extracted content
7. User reviews/edits text, selects "Create Task"
8. Task creation form pre-populates with OCR text
9. User selects quadrant, sets priority, due date
10. New task appears in Eisenhower Matrix

### 📋 Alternative Flow: Gallery Import → Work Note

1. User selects "Import Image" from capture options
2. File picker shows gallery/documents
3. User selects image containing meeting notes
4. OCR processing extracts discussion points
5. User reviews text, selects "Save as Work Note"
6. Work note captures content with timestamp
7. Note appears in today's Scrum Summary data

---

## ⚙️ Settings Integration

### 🔧 Azure Configuration Panel

Add to existing Settings page ([Settings.razor](Settings.razor)):

```html
<!-- Azure Vision Configuration Section -->
<div class="card mb-3">
    <div class="card-header">
        <h5><i class="bi bi-camera"></i> Azure Computer Vision</h5>
    </div>
    <div class="card-body">
        <div class="mb-3">
            <label class="form-label">Endpoint URL</label>
            <input type="url" class="form-control" 
                   placeholder="https://your-region.api.cognitive.microsoft.com/"
                   @bind="AzureVisionEndpoint" />
        </div>
        <div class="mb-3">
            <label class="form-label">API Key</label>
            <input type="password" class="form-control" 
                   placeholder="Your Azure Vision API key"
                   @bind="AzureVisionApiKey" />
        </div>
        <div class="mb-3">
            <label class="form-label">Max Image Size (MB)</label>
            <input type="number" class="form-control" 
                   placeholder="5" min="1" max="10"
                   @bind="MaxImageSizeMB" />
        </div>
    </div>
</div>
```

### 🔒 Feature Enabling Logic

```csharp
// Image capture features only enabled when properly configured
public bool IsImageCaptureEnabled => 
    !string.IsNullOrWhiteSpace(AzureVisionEndpoint) && 
    !string.IsNullOrWhiteSpace(AzureVisionApiKey);

// UI components conditionally rendered
@if (SettingsService.IsImageCaptureEnabled)
{
    <button class="btn btn-primary">
        <i class="bi bi-camera"></i> Capture Note
    </button>
}
else
{
    <div class="alert alert-info">
        Configure Azure Vision credentials in Settings to enable image capture.
    </div>
}
```

---

## 🎯 Success Metrics & Acceptance Criteria

### ✅ Functional Requirements

- [ ] Camera capture works on target MAUI platforms (Windows, Android)
- [ ] Azure OCR successfully extracts text from handwritten notes
- [ ] Extracted text can create both Tasks and Work Notes
- [ ] Settings panel properly stores and validates Azure credentials
- [ ] Feature is disabled when credentials not configured
- [ ] Error handling provides clear user feedback
- [ ] UI is responsive and follows existing design patterns

### 📊 Performance Requirements

- [ ] Image capture responds within 2 seconds
- [ ] OCR processing provides feedback during 5-15 second processing time
- [ ] Images under 5MB process successfully
- [ ] Extracted text accuracy >85% for printed text, >70% for handwriting

---

## 🔮 Future Enhancements

| Enhancement                | Description                                                                 |
| -------------------------- | --------------------------------------------------------------------------- |
| **Smart Text Parsing**    | Use Azure OpenAI to automatically detect tasks vs notes from extracted text |
| **Batch Processing**       | Process multiple images in sequence                                          |
| **Template Recognition**   | Recognize common note formats (meeting templates, task lists)               |
| **Offline OCR**           | Local OCR fallback using ML.NET or TensorFlow Lite                         |
| **Cloud Sync**            | Sync extracted text and created tasks/notes across devices                  |
| **Handwriting Training**   | Personal handwriting model training for improved accuracy                   |
| **Voice Annotations**     | Add voice notes to images before OCR processing                             |
| **Collaboration**         | Share extracted tasks directly to team members                              |

---

## 📚 Related Documentation

- [docs/feature-eisenhower-matrix.md](feature-eisenhower-matrix.md) - Task creation and management
- [docs/feature-scrum-summary.md](feature-scrum-summary.md) - Work capture note integration  
- [docs/database-definitions.md](database-definitions.md) - Existing data models
- [Azure Computer Vision Documentation](https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/) - API Reference