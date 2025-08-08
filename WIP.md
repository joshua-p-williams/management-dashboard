Analyze the README.md and follow the links within it to the contents within the ./docs/ subfolder reading those markdown documents to get an understanding of the project that we are working on.  Then analalyze the solution itself to get an idea of what has already been built.  Then start looking at the ./docs/tasks.md as we will need to pick back up on the next unfinished tasks.  After you've done all this analysis report back to me if you agree we are in a state to continue.

Also familiarize yourself with the layout of the solution and it's various projects.

ManagementDashboard = MAUI Blazor Hybrid App
ManagementDashboard.Data = Data Access Layer holding all data models repositories and sql migrations
ManagementDashboard.Core = Where to store common business logic and services created in a testable way
ManagementDashboard.Tests = The unit testing project.

I want you to serve the role of a Software Engineer implementing various features for me.

----------------

I want to work on making the "Overdue" concept more robust and fully implemented.  I see some initial work has been done but it is not fully implemented.  
I want you to analyze the current state of the code and document what has been done so far and what is still missing.  
Then we can create a plan to finish this feature.  Here is some context on how it currently works.  I want you to get familiar with the current implementation and then we can discuss next steps.

# Overdue Concept Inventory & Usage

## 1. Database Layer
- **Tasks Table**: Each task (`EisenhowerTask`) has a `CreatedAt` column (datetime) which is used as the basis for overdue calculations.
- **No explicit "DueDate" or "Overdue" column**: Overdue status is computed, not stored.

## 2. Configuration / Settings
- **SettingsService** (`ManagementDashboard.Core.Services.SettingsService`):
  - Exposes `OverdueThresholdDays` (default: 2 days) as a configurable property.
  - Value is persisted via `IAppPreferences` and can be changed in the app UI.
- **Settings UI** (`ManagementDashboard/Components/Pages/Settings.razor`):
  - Users can set the "Overdue Threshold (days)" via a number input.
  - This updates the value in `SettingsService` and persists it.

## 3. Business Logic
- **EisenhowerTask** (`ManagementDashboard.Data.Models.EisenhowerTask`):
  - Method `IsPastDue(int overdueThresholdDays)` returns `true` if the task is not completed and the current date is more than the threshold days after `CreatedAt`.
- **TaskCard Component** (`ManagementDashboard/Components/TaskCard.razor`):
  - Calls `Task.IsPastDue(OverdueThresholdDays)` to determine if a task is overdue.
  - If overdue, displays a red "Overdue" badge on the task card.
  - The threshold is fetched from preferences (`AppPreferences.GetInt("OverdueThresholdDays", 2)`).

## 4. Unit Tests
- **SettingsServiceTests**:
  - Tests default and custom values for `OverdueThresholdDays`.
  - Ensures persistence and retrieval of the threshold setting.
- **TaskServiceTests**:
  - While not directly testing overdue logic, these tests ensure tasks are ordered and filtered correctly, which may interact with overdue status in the UI.

## 5. Other Notes
- **No direct overdue filtering in repositories/services**: Overdue is a UI/business logic concern, not a DB query.
- **No overdue notifications/alerts**: Overdue status is visual only (badge on task card).

---

### Summary Table

| Area                | Usage/Implementation                                                                 |
|---------------------|--------------------------------------------------------------------------------------|
| Database            | `CreatedAt` column in `Tasks` table; no explicit "overdue" field                    |
| Settings            | `OverdueThresholdDays` (default 2) in `SettingsService`, configurable in UI          |
| Business Logic      | `EisenhowerTask.IsPastDue(int)` method; used in UI to check overdue status           |
| UI                  | TaskCard shows "Overdue" badge if `IsPastDue` returns true                          |
| Unit Tests          | SettingsServiceTests cover threshold config; TaskServiceTests cover task ordering     |


----------------------


# 🚩 Product Definition: Optional Task Due Date

### Overview
Introduce an optional "Due Date" for Eisenhower tasks. This enables users to set a specific due date per task, which will be used (if present) for overdue calculations and UI display. The feature includes database migration, model/repository refactor, UI updates, and test coverage.

---### 🛠️ Phase 1: Database Migration

- [x] **Create Migration Script**
  - Add a nullable `DueDate` column (`DATETIME` or `TEXT`) to the `Tasks` table.
  - Place migration in `ManagementDashboard.Data/Migrations/`.
  - Example: `ALTER TABLE Tasks ADD COLUMN DueDate DATETIME NULL;`
- [x] **Test Migration**
  - Ensure migration runs without data loss or errors on existing DBs.

---

### 🧩 Phase 2: Data Model & Repository Refactor

- [ ] **Update Model**
  - Add a nullable `DueDate` property to `EisenhowerTask` in `ManagementDashboard.Data.Models`.
- [ ] **Update Repository**
  - Ensure all CRUD operations in `EisenhowerTaskRepository` handle the new `DueDate` field.
  - Update mapping, inserts, and updates to support `DueDate`.
- [ ] **Update Interfaces**
  - Update any relevant interfaces or DTOs to include `DueDate`.

---

### 🎨 Phase 3: UI/UX Changes

- [ ] **Task Editor UI**
  - Update `TaskEditor.razor` and code-behind to allow users to set, update, or clear the due date.
  - Use a date picker input (Blazor/Bootstrap) for due date selection.
  - Show the due date in the editor if already set.
- [ ] **Task Display**
  - Optionally display the due date on `TaskCard.razor` if present.
  - Consider visual cues for tasks with due dates approaching or past.

---

### 🧠 Phase 4: Overdue Logic Refactor

- [ ] **Update Overdue Calculation**
  - Refactor `IsPastDue(int overdueThresholdDays)` in `EisenhowerTask`:
    - If `DueDate` is set, use it for overdue calculation.
    - If not, fall back to `CreatedAt + threshold`.
- [ ] **Update UI Logic**
  - Ensure overdue badge and logic in `TaskCard` and related components use the new logic.

---

### 🧪 Phase 5: Unit Tests

- [ ] **Model Tests**
  - Add/modify tests to cover overdue logic with and without due dates.
  - Test edge cases: due date in past, today, future, and null.
- [ ] **Repository Tests**
  - Ensure CRUD tests cover tasks with/without due dates.
- [ ] **UI Tests**
  - (If applicable) Add/modify UI tests to verify due date display and editing.

---

### 🧹 Phase 6: Documentation & Cleanup

- [ ] **Update Docs**
  - Document the new due date feature in README and relevant docs.
  - Add usage notes and screenshots if possible.
- [ ] **Code Cleanup**
  - Remove obsolete code, update comments, and ensure consistency.

---

### ✅ Completion Criteria

- Migration runs cleanly and adds `DueDate` to all environments.
- Users can set, update, and clear due dates in the UI.
- Overdue logic works as expected for both due date and threshold-based tasks.
- All tests pass and cover new logic.
- Documentation is up to date.

---

This phased plan can be checked off step-by-step by a developer or Copilot AI agent.

---------------------------------------------------------------
