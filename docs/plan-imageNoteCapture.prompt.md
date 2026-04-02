# Plan: Image Note Capture Implementation

**TL;DR:** Implement Azure Computer Vision OCR to extract text from captured images and route to either Eisenhower Tasks or Work Capture Notes. Build incrementally using existing infrastructure: extend WorkCaptureNote model, add new services following established DI patterns, create modal components matching Bootstrap conventions, and integrate with current Settings architecture. Focus on memory-only processing with cross-platform MAUI support.

See docs\feature-image-note-capture.md for the feature definition.

**Steps**

1. **Add Dependencies & Settings Infrastructure** - Install Azure Computer Vision NuGet, extend SettingsService with Azure credentials, add secure storage for API key, test configuration validation
2. **Create Core Services** - Implement IAzureVisionService with OCR functionality, build IImageCaptureService for MAUI camera/gallery access, create text processing utilities for task/note routing  
3. **Build Image Capture UI** - Create ImageCaptureModal.razor for camera/gallery selection, add ImagePreviewModal.razor for review, implement OcrResultsModal.razor for text editing and routing decisions
4. **Extend WorkCaptureNote Integration** - Add OCR-related properties to model, update repository methods, create database migration for new fields, extend existing UI components
5. **Add Task Creation from OCR** - Extend TaskService with CreateTaskFromOcrText method, integrate with existing TaskEditor modal, add smart text parsing to suggest quadrants and extract titles
6. **Settings Panel Integration** - Add Azure Computer Vision configuration section to existing Settings page, implement feature gating when credentials not configured, add validation and testing UI
7. **Cross-Platform Camera Access** - Implement platform-specific camera permissions, add MAUI MediaPicker integration, handle file selection from gallery/documents, test on Windows and Android
8. **Error Handling & Polish** - Add comprehensive error handling for network/OCR failures, implement loading states and progress indicators, add user-friendly error messages, test offline scenarios

**Verification**

Run the app, configure Azure credentials in Settings, capture image via camera or gallery, verify OCR text extraction, create both Task and Work Note from extracted text, confirm data persists correctly, test error scenarios and loading states.

**Decisions**

- **Azure Computer Vision**: Chose cloud OCR for professional accuracy over local Tesseract complexity
- **Memory-Only Processing**: Images processed in-memory and discarded, no file system storage needed  
- **Cross-Platform**: Build for both Windows and Android simultaneously using MAUI abstractions
- **Existing Infrastructure**: Leverage current WorkCaptureNote model, Settings patterns, and Bootstrap modal conventions
