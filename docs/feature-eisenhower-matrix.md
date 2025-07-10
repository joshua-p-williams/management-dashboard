# ✅ Feature Definition: Eisenhower Matrix Utility

## 🎯 Purpose

The **Eisenhower Matrix Utility** helps you prioritize and act on tasks by classifying them across two key dimensions: **Urgency** and **Importance**. This feature enables you to capture, visualize, manage, and track actionable work items in four distinct quadrants, with full lifecycle support (creation, update, delegation, blocking, and completion). It also supports a generic task inventory workflow, allowing users to quickly capture tasks without immediately assigning a quadrant, and later categorize them as part of their prioritization process.

---

## 🧩 Functional Scope

### 🎛️ Core Functionalities

| Feature                | Description                                                              |
| ---------------------- | ------------------------------------------------------------------------ |
| Add Task               | Create a task with title, description; quadrant is optional (can be set later) |
| View Uncategorized     | See a list of tasks not yet assigned to a quadrant                        |
| Assign Quadrant        | Assign a quadrant to an uncategorized task                                |
| Quadrant Assignment    | Tasks appear in one of 4 quadrants: Do, Schedule, Delegate, Delete         |
| Edit Task              | Modify task title, description, quadrant, delegation, etc.                 |
| Move Between Quadrants | Drag & drop or reclassify tasks manually                                   |
| Complete Task          | Mark task as completed with timestamp                                      |
| Block Task             | Mark task as blocked with reason and timestamp                             |
| Unblock Task           | Resolve a blocker and record resolution timestamp                          |
| Delete Task            | Hard delete or soft-delete depending on quadrant                           |
| Audit Trail            | Track created, updated, completed, and blocked timestamps                  |
| Delegation             | Capture "who" a task was delegated to (freeform text for now)              |
| Responsive UI          | Grid layout adapts to screen size (especially important for MAUI)          |

---

## 📊 Quadrant Breakdown

| Quadrant Label | Meaning                    | UI Name    |
| -------------- | -------------------------- | ---------- |
| Do             | Urgent & Important         | `Do`       |
| Schedule       | Not Urgent but Important   | `Schedule` |
| Delegate       | Urgent but Not Important   | `Delegate` |
| Delete         | Not Urgent & Not Important | `Delete`   |
| (none)         | Uncategorized             | (Uncategorized) |

---

## 🎨 UI/UX Components (via Bootstrap 5)

| Component                  | Role                                                |
| -------------------------- | --------------------------------------------------- |
| `Grid` + `Sidebar`         | Layout for 2x2 quadrant matrix and sidebar          |
| `Card`                     | Individual task display inside each quadrant        |
| `Modal`                    | Modal for add/edit task                             |
| `Dropdown` or `Navbar`     | Per-task menu: mark done, move, block, delete, etc. |
| `Badge`                    | Optional indicators (blocked, delegated, done)      |
| `Tooltip`                  | Show audit metadata on hover/tap                    |
| `Toast`                    | Feedback for user actions                           |
| `Navbar` / Nav Panel       | Entry point to Eisenhower Matrix utility            |
| `Uncategorized List`       | Dedicated view/component for uncategorized tasks    |

---

## 📦 Data Model (Used by Formula.SimpleRepo)

```csharp
public class EisenhowerTask : IAuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? Quadrant { get; set; } // Do, Schedule, Delegate, Delete, or null for uncategorized
    public DateTime? CompletedAt { get; set; }
    public string? BlockerReason { get; set; }
    public DateTime? BlockedAt { get; set; }
    public DateTime? UnblockedAt { get; set; }
    public string? DelegatedTo { get; set; }
    public PriorityLevel Priority { get; set; } // 0=Low, 1=Medium, 2=High
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Computed properties for business logic/UI only
    [Dapper.NotMapped]
    public bool IsCompleted => CompletedAt != null;
    [Dapper.NotMapped]
    public bool IsBlocked => BlockedAt != null && UnblockedAt == null;
}

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? CompletedAt { get; set; }
    string? BlockerReason { get; set; }
    DateTime? BlockedAt { get; set; }
    DateTime? UnblockedAt { get; set; }
}

public enum PriorityLevel { Low = 0, Medium = 1, High = 2 }
```

---

## 🧠 Behavioral Rules

| Scenario                          | Behavior                                                 |
| --------------------------------- | -------------------------------------------------------- |
| Task created                      | `Quadrant = null` by default (uncategorized)             |
| Task assigned to quadrant         | `Quadrant` set to one of the four values                 |
| Task marked “Done”                | `CompletedAt = now()`                                    |
| Task flagged “Blocked”            | `BlockerReason`, `BlockedAt = now()`                     |
| Task unblocked                    | `UnblockedAt = now()`                                    |
| Task edited                       | `UpdatedAt = now()`                                      |
| Task deleted from “Delete” Q      | Task is fully removed from DB                            |
| Task deleted from other quadrants | Ask confirmation — soft delete or move to “Delete”       |

---

## 🧪 Validation Rules

* `Title` is required, 3–100 chars
* `Quadrant` is optional; if set, must be one of `'Do'`, `'Schedule'`, `'Delegate'`, `'Delete'`
* If `IsBlocked` (computed) is true, `BlockerReason` is required
* If `IsCompleted` (computed) is true, set `CompletedAt` if null

---

## 🔄 Navigation & State

* Eisenhower Matrix is a standalone page (`/eisenhower`)
* State managed via `TaskService` (Scoped DI)
* UI binds to quadrant-specific `List<EisenhowerTask>` collections, plus a collection for uncategorized tasks
* Service methods trigger DB sync via SimpleRepo

---

## 🔗 User Actions

| Action               | UI Element        | DB Update |
| -------------------- | ----------------- | --------- |
| Create Task          | "Add Task" button | INSERT    |
| Assign Quadrant      | Dropdown / action | UPDATE    |
| Move Quadrant        | Dropdown / drag   | UPDATE    |
| Edit Task            | Edit icon         | UPDATE    |
| Complete Task        | Checkmark / icon  | UPDATE    |
| Block / Unblock Task | Context menu      | UPDATE    |
| Delete Task          | Trash icon        | DELETE    |

---

## 🔗 Integration with Scrum Summary

When a task is completed or updated with significant progress in the Eisenhower Matrix, it is automatically surfaced for inclusion in the next day's Scrum Summary ("Yesterday's Work"). This ensures that all meaningful activity tracked in the Eisenhower Matrix is available for daily stand-up reporting.

- Completed tasks are auto-pulled into the Scrum Summary for the following day.
- Optionally, tasks with major updates (not just completion) can also be included.
- This integration is only enabled once both the Eisenhower Matrix and Scrum Summary features are implemented and working independently.
- Manual review and editing of the auto-pulled summary is always possible in the Scrum Summary Dashboard.

> **Feature Dependency:** This sub-feature depends on the Scrum Summary Dashboard being present and functional.

---

## 🏗️ Build Order & Feature Dependencies

To support this integration, the following build order is recommended:

1. **Eisenhower Matrix Utility**: Implement basic task capture, quadrant assignment, and completion tracking.
2. **Scrum Summary Dashboard**: Implement daily summary creation, editing, and persistence.
3. **Eisenhower-to-Scrum Integration (Sub-Feature)**: Enable auto-pulling of completed/updated Eisenhower tasks into the Scrum Summary's "Yesterday's Work" section.

This staged approach ensures each feature is robust and testable before integration.

---

## 🧩 Future Enhancements

* Tags / Project assignment
* Search / Filter by title or keyword
* Export tasks to Markdown or CSV
* Integration with calendar or reminders
* Task recurrence (e.g. weekly review)

---

## 🗃️ Database Schema

See [Eisenhower Matrix Database Definition](feature-eisenhower-matrix-database.md) for the full schema and rationale for this feature's data model.
