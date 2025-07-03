# ✅ Feature Definition: Eisenhower Matrix Utility

## 🎯 Purpose

The **Eisenhower Matrix Utility** helps you prioritize and act on tasks by classifying them across two key dimensions: **Urgency** and **Importance**. This feature enables you to visualize, manage, and track actionable work items in four distinct quadrants, with full lifecycle support (creation, update, delegation, blocking, and completion).

---

## 🧩 Functional Scope

### 🎛️ Core Functionalities

| Feature                | Description                                                              |
| ---------------------- | ------------------------------------------------------------------------ |
| Add Task               | Create a task with title, description, quadrant, and optional delegation |
| Quadrant Assignment    | Tasks appear in one of 4 quadrants: Do, Schedule, Delegate, Delete       |
| Edit Task              | Modify task title, description, quadrant, delegation, etc.               |
| Move Between Quadrants | Drag & drop or reclassify tasks manually                                 |
| Complete Task          | Mark task as completed with timestamp                                    |
| Block Task             | Mark task as blocked with reason and timestamp                           |
| Unblock Task           | Resolve a blocker and record resolution timestamp                        |
| Delete Task            | Hard delete or soft-delete depending on quadrant                         |
| Audit Trail            | Track created, updated, completed, and blocked timestamps                |
| Delegation             | Capture "who" a task was delegated to (freeform text for now)            |
| Responsive UI          | Grid layout adapts to screen size (especially important for MAUI)        |

---

## 📊 Quadrant Breakdown

| Quadrant Label | Meaning                    | UI Name    |
| -------------- | -------------------------- | ---------- |
| Do             | Urgent & Important         | `Do`       |
| Schedule       | Not Urgent but Important   | `Schedule` |
| Delegate       | Urgent but Not Important   | `Delegate` |
| Delete         | Not Urgent & Not Important | `Delete`   |

---

## 🖼️ UI/UX Components (via MudBlazor)

| Component                  | Role                                                |
| -------------------------- | --------------------------------------------------- |
| `MudGrid` / `MudItem`      | Layout for 2x2 quadrant matrix                      |
| `MudCard`                  | Individual task display inside each quadrant        |
| `MudDialog`                | Modal for add/edit task                             |
| `MudMenu` or `MudIconMenu` | Per-task menu: mark done, move, block, delete, etc. |
| `MudBadge`                 | Optional indicators (blocked, delegated, done)      |
| `MudTooltip`               | Show audit metadata on hover/tap                    |
| `MudSnackbar`              | Feedback for user actions                           |
| `MudDrawer` / Nav Panel    | Entry point to Eisenhower Matrix utility            |

---

## 📦 Data Model (Used by Formula.SimpleRepo)

```csharp
public class EisenhowerTask : IAuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string Quadrant { get; set; } = "Do"; // Do, Schedule, Delegate, Delete
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsBlocked { get; set; }
    public string? BlockerReason { get; set; }
    public DateTime? BlockedAt { get; set; }
    public DateTime? UnblockedAt { get; set; }
    public string? DelegatedTo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

---

## 🧠 Behavioral Rules

| Scenario                          | Behavior                                                 |
| --------------------------------- | -------------------------------------------------------- |
| Task marked “Done”                | `IsCompleted = true`, `CompletedAt = now()`              |
| Task flagged “Blocked”            | `IsBlocked = true`, `BlockerReason`, `BlockedAt = now()` |
| Task unblocked                    | `IsBlocked = false`, `UnblockedAt = now()`               |
| Task edited                       | `UpdatedAt = now()`                                      |
| Task deleted from “Delete” Q      | Task is fully removed from DB                            |
| Task deleted from other quadrants | Ask confirmation — soft delete or move to “Delete”       |

---

## 🧪 Validation Rules

* `Title` is required, 3–100 chars
* `Quadrant` must be one of `'Do'`, `'Schedule'`, `'Delegate'`, `'Delete'`
* If `IsBlocked = true`, `BlockerReason` is required
* If `IsCompleted = true`, set `CompletedAt` if null

---

## 🔄 Navigation & State

* Eisenhower Matrix is a standalone page (`/eisenhower`)
* State managed via `TaskService` (Scoped DI)
* UI binds to quadrant-specific `List<EisenhowerTask>` collections
* Service methods trigger DB sync via SimpleRepo

---

## 🔗 User Actions

| Action               | UI Element        | DB Update |
| -------------------- | ----------------- | --------- |
| Create Task          | "Add Task" button | INSERT    |
| Move Quadrant        | Dropdown / drag   | UPDATE    |
| Edit Task            | Edit icon         | UPDATE    |
| Complete Task        | Checkmark / icon  | UPDATE    |
| Block / Unblock Task | Context menu      | UPDATE    |
| Delete Task          | Trash icon        | DELETE    |

---

## 🧩 Future Enhancements

* Tags / Project assignment
* Search / Filter by title or keyword
* Export tasks to Markdown or CSV
* Integration with calendar or reminders
* Task recurrence (e.g. weekly review)
