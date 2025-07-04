# 📋 Feature Definition: Scrum Call Summary Dashboard

## 🎯 Purpose

The **Scrum Call Summary Dashboard** provides a simple interface to prepare for daily stand-up meetings. It answers three core Scrum questions based on user activity:

1. What did I work on yesterday?
2. What am I working on today?
3. What blockers do I currently have?

This utility helps generate structured summaries from persisted task activity in the Eisenhower Matrix and allows manual edits before saving or presenting the summary.

---

## 🧩 Functional Scope

### 🗓️ Core Functionalities

| Feature                    | Description                                                           |
| -------------------------- | --------------------------------------------------------------------- |
| Auto-Pull Yesterday’s Work | Lists tasks marked complete between yesterday 12:00am–11:59pm         |
| Auto-Pull Today’s Plan     | Lists all incomplete, unblocked tasks in “Do” or “Schedule” quadrants |
| Auto-Pull Current Blockers | Lists all currently blocked tasks                                     |
| Manual Entry / Overrides   | User can add custom notes or replace any of the pulled content        |
| Save Daily Summary         | Saves summary to `ScrumNotes` table with date-based uniqueness        |
| View Past Entries          | Allows navigating or browsing past scrum entries                      |
| Edit Existing Entry        | Supports editing previously saved daily summaries                     |
| Markdown Export (Future)   | Export scrum summary to Markdown format for Slack/Email/etc.          |

---

## 🧠 Data Source Strategy

* **Yesterday** → `Tasks` where `CompletedAt` is yesterday’s date
* **Today** → `Tasks` where `IsCompleted = false AND IsBlocked = false AND Quadrant IN ('Do', 'Schedule')`
* **Blockers** → `Tasks` where `IsBlocked = true`

Fallback: If no matching data, allow manual entry.

---

## 🧾 Data Model

```csharp
public class ScrumNote
{
    public int Id { get; set; }
    public DateOnly EntryDate { get; set; }  // e.g., 2025-07-03
    public string? YesterdayNotes { get; set; }
    public string? TodayPlan { get; set; }
    public string? Blockers { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

---

## 📊 UI/UX Components (via Blazorise + Fluent)

| Component                  | Role                                                |
| -------------------------- | --------------------------------------------------- |
| `Layout` + `LayoutSider`   | Page layout and sidebar                             |
| `Card`                     | Display of daily summary entries                    |
| `Modal`                    | Edit/add summary entry                              |
| `Dropdown` or `Bar`        | Menu for actions (edit, delete, etc.)               |
| `Badge`                    | Status indicators                                   |
| `Tooltip`                  | Show audit metadata                                 |
| `Snackbar`                 | Feedback for user actions                           |

---

## 🔄 Navigation & State

* Located at `/scrum-summary`
* Loads summary for current day automatically (if exists, allow editing; else, generate)
* Calls `ScrumSummaryService` to:

  * Pull and summarize data from `Tasks`
  * Save/load from `ScrumNotes`

---

## 💡 Behavioral Rules

| Behavior                    | Description                                                   |
| --------------------------- | ------------------------------------------------------------- |
| New Day Entry               | Auto-generates summary fields from latest `Tasks` table state |
| Save Entry                  | Saves or updates row in `ScrumNotes` for current date         |
| Edit Previous Entry         | Allows editing summaries of previous days                     |
| Prevent Duplicate Entry     | Only one entry per `EntryDate` — update if already exists     |
| Override Auto-Pulled Values | Manual edits are allowed; auto-pulled values are suggestions  |

---

## 🧪 Validation Rules

* `EntryDate` must be unique per day
* At least one of the fields (`YesterdayNotes`, `TodayPlan`, `Blockers`) must be filled before save
* `EntryDate` defaults to today on load

---

## 📦 Example Entry

**EntryDate:** 2025-07-03
**YesterdayNotes:**

```
- Completed login UI for Dashboard (Do quadrant)
- Refactored TaskService for dependency injection
```

**TodayPlan:**

```
- Add blocker resolution timestamps to EisenhowerTask
- Begin work on Markdown export utility
```

**Blockers:**

```
- Waiting on design assets for navigation sidebar
```

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

The Scrum Summary Dashboard is tightly integrated with the Eisenhower Matrix feature. When a task in the Eisenhower Matrix is completed or updated with significant progress, it is automatically eligible to be included in the next day's "Yesterday's Work" section of the Scrum Summary. Each summary entry can optionally reference a single Eisenhower task (via a `TaskId` foreign key), but manual entries not tied to a task are fully supported. This ensures that all actionable work tracked in the Eisenhower Matrix is surfaced for daily stand-up reporting without manual duplication, while still allowing flexibility for other types of work.

- When a task is marked as completed, it is auto-pulled into the next day's summary and can be linked via `TaskId`.
- Optionally, tasks with significant updates (not just completion) can also be surfaced for review.
- The auto-pull logic queries the Eisenhower Matrix's `Tasks` table for relevant activity.
- Manual override is always available: users can edit or remove any auto-pulled entry before saving, or add entries not linked to a task.

> **Feature Dependency:** This integration requires both the Eisenhower Matrix and Scrum Summary features to be implemented and working independently before enabling this auto-pull sub-feature.

---

## 🏗️ Build Order & Feature Dependencies

To implement this integration, the following build order is recommended:

1. **Eisenhower Matrix Utility**: Implement basic task capture, quadrant assignment, and completion tracking.
2. **Scrum Summary Dashboard**: Implement daily summary creation, editing, and persistence.
3. **Eisenhower-to-Scrum Integration (Sub-Feature)**: Enable auto-pulling of completed/updated Eisenhower tasks into the Scrum Summary's "Yesterday's Work" section.

This ensures a clean separation of concerns and allows each feature to be tested independently before integration.

---

## 🗃️ Database Schema

See [Scrum Summary Database Definition](feature-scrum-summary-database.md) for the full schema and rationale for this feature's data model.

