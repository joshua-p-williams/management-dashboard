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

## 📊 UI/UX Components (via MudBlazor)

| Component               | Role                                                   |
| ----------------------- | ------------------------------------------------------ |
| `MudTextField`          | Inline text input for each summary section             |
| `MudExpansionPanel`     | Expand/collapse previous summaries                     |
| `MudButton`             | Save, Refresh, Export actions                          |
| `MudTable` or `MudList` | Render pulled task summaries (editable inline)         |
| `MudTabs` (optional)    | View: Today / Past Days                                |
| `MudTooltip`            | Hover for data source explanations (e.g., auto-pulled) |
| `MudSnackbar`           | Success/error feedback                                 |

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

