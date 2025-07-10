# 📋 Feature Definition: Scrum Call Summary Dashboard (Revised)

## 🎯 Purpose

The **Scrum Call Summary Dashboard** provides a dynamic, accurate interface for daily stand-up meetings. It answers three core Scrum questions by pulling from both structured tasks and unstructured work capture notes:

1. What did I work on yesterday?
2. What am I working on today?
3. What blockers do I currently have?

---

## 🧩 Functional Scope (Revised)

### 🗓️ Core Functionalities

| Feature                        | Description                                                                 |
| ------------------------------ | --------------------------------------------------------------------------- |
| Work Capture                   | Users can log any work, research, or event as a note, optionally link to task |
| Auto-Pull Task Updates         | Lists tasks updated or completed on a given date                             |
| Auto-Pull Work Capture Notes   | Lists all work capture notes for a given date                                |
| Manual Entry / Overrides       | User can add/edit/delete work capture notes                                  |
| Save Daily Summary (Virtual)   | Scrum summary is a dynamic report, not a stored summary text                 |
| View Past Entries              | Allows browsing summaries for previous days                                  |
| Edit Work Capture Notes        | Supports editing/deleting any note                                           |
| Markdown Export (Future)       | Export summary to Markdown format for Slack/Email/etc.                       |

---

## 🧠 Data Source Strategy (Revised)

* **Yesterday** →
  - All `WorkCaptureNotes` with `CreatedAt` on yesterday's date
  - All tasks with `UpdatedAt` or `CompletedAt` on yesterday's date
* **Today** →
  - All tasks not deleted, ordered by Quadrant (Do, Schedule, Delegate, Delete), then by Priority (high to low), then by CreatedAt (oldest first)
* **Blockers** →
  - All tasks where `IsBlocked = true`

---

## 🧾 Data Model

See `feature-scrum-summary-database.md` for the full schema of `WorkCaptureNotes` and `Tasks`.

---

## 📊 UI/UX Components (via Bootstrap 5)

| Component                  | Role                                                |
| -------------------------- | --------------------------------------------------- |
| `Grid` + `Sidebar`         | Page layout and sidebar                             |
| `Card`                     | Display of daily summary entries                    |
| `Modal`                    | Add/edit work capture note                          |
| `Dropdown` or `Navbar`     | Menu for actions (edit, delete, etc.)               |
| `Badge`                    | Status indicators                                   |
| `Tooltip`                  | Show audit metadata                                 |
| `Toast`                    | Feedback for user actions                           |

---

## 🔄 Navigation & State

* Located at `/scrum-summary`
* Loads summary for current day automatically (virtual, not stored)
* Allows adding/editing/deleting work capture notes
* Calls `ScrumSummaryService` to:
  * Pull and summarize data from `WorkCaptureNotes` and `Tasks`

---

## 💡 Behavioral Rules

| Behavior                    | Description                                                   |
| --------------------------- | ------------------------------------------------------------- |
| New Day Entry               | Shows all work capture notes and task updates for the day      |
| Edit Work Capture           | Users can add/edit/delete notes for any day                    |
| Prevent Duplicate Notes     | (Optional) Warn if duplicate notes for same task/date          |
| Override Auto-Pulled Values | Manual edits are always allowed                                |

---

## 🧪 Validation Rules

* At least one work capture note or task update must exist to show a summary
* `CreatedAt` defaults to now on new note

---

## 📦 Example Entry

**WorkCaptureNotes:**
- "Met with product team to clarify requirements" (no task)
- "Refactored TaskService for dependency injection" (linked to Task #12)

**Tasks Updated/Completed:**
- Task #15: "Completed login UI for Dashboard" (Do quadrant, completed yesterday)
- Task #22: "Fixed bug in audit trail" (updated yesterday)

---

## 🧩 Future Enhancements

| Feature                 | Description                                            |
| ----------------------- | ------------------------------------------------------ |
| Markdown Export         | Button to copy/export summary in Slack-friendly format |
| Voice-to-Text Input     | Speak entries directly (MAUI-dependent)                |
| Sprint Rollup Dashboard | Show multiple days’ notes in one view                  |
| Tag Linkage             | Tasks tagged `#scrum` show up as priority candidates   |

---

## 🔗 Integration with Eisenhower Matrix

- Work capture notes can be linked to tasks, but are not required to be.
- Task updates and completions are always included in the summary for the relevant day.
- Manual notes and summaries unrelated to Eisenhower tasks are fully supported.

---

## 🏗️ Build Order & Feature Dependencies

1. **Work Capture Notes**: Implement generic work capture and editing.
2. **Scrum Summary Dashboard**: Implement dynamic summary/reporting logic.
3. **Eisenhower-to-Scrum Integration**: Enable linking and auto-pulling of task updates.

---

## 🗃️ Database Schema

See [Scrum Summary Database Definition](feature-scrum-summary-database.md) for the full schema and rationale for this feature's data model.

---

## 🖌️ UI/UX Design Considerations (To Be Determined)

The following aspects are open for UI/UX and product design input. These decisions will shape the usability and overall experience of the Scrum Summary Dashboard:

- **Work Capture Entry Point:**
  - Where and how should users enter "work capture" notes? (e.g., always-visible input at the top, floating action button, modal dialog, inline add in a list, etc.)
  - Should adding a note be a quick action or require a full form?

- **Presentation of the Three Scrum Questions:**
  - Should the "What did I work on yesterday?", "What am I working on today?", and "What blockers do I currently have?" sections be presented as:
    - A single long form (vertical stack)?
    - Side-by-side columns (responsive grid)?
    - Tabbed interface (one question per tab)?
    - Accordion or collapsible panels?
  - How can we best support both desktop and mobile layouts?

- **Editing and Reviewing Past Entries:**
  - How should users navigate to previous days' summaries?
  - Should editing be inline, via modal, or on a separate page?

- **Highlighting Auto-Pulled vs. Manual Content:**
  - How should the UI distinguish between auto-pulled task updates and manually entered work capture notes?
  - Should there be visual cues, icons, or color coding?

- **Feedback and Validation:**
  - How should the UI provide feedback for successful saves, errors, or validation issues?
  - Should we use Bootstrap toasts, inline alerts, or both?

- **Extensibility:**
  - How can the design accommodate future features (e.g., Markdown export, voice input, sprint rollup)?

> **UI/UX designers and product owners are encouraged to propose wireframes, user flows, and interaction patterns for these open questions.**

