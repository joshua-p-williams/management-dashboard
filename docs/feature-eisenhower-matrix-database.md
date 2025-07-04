# 🗃️ Eisenhower Matrix Database Definition

This document defines the database schema and rationale for the **Eisenhower Matrix** feature in the Management Dashboard Platform.

> 🔗 **Back to Feature:** [Eisenhower Matrix Utility](feature-eisenhower-matrix.md)

---

## 📄 Purpose

Stores and manages all actionable tasks, their status, quadrant (optional for uncategorized tasks), delegation, blockers, and audit trail for the Eisenhower Matrix utility. Also supports capturing tasks before they are assigned a quadrant, enabling a generic task inventory workflow.

---

## 🧱 Table: `Tasks`

| Column Name     | Type     | Constraints                       | Description                                 |
| --------------- | -------- | --------------------------------- | ------------------------------------------- |
| `Id`            | INTEGER  | Primary Key, AutoIncrement        | Unique ID                                   |
| `Title`         | TEXT     | NOT NULL                          | Short title of task                         |
| `Description`   | TEXT     |                                   | Optional details                            |
| `Quadrant`      | TEXT     | NULLABLE, CHECK (IN values below) | 'Do', 'Schedule', 'Delegate', 'Delete', or NULL for uncategorized |
| `IsCompleted`   | BOOLEAN  | Default: 0                        | Whether task is marked done                 |
| `CompletedAt`   | DATETIME |                                   | Nullable timestamp of completion            |
| `IsBlocked`     | BOOLEAN  | Default: 0                        | Whether task is blocked                     |
| `BlockerReason` | TEXT     |                                   | Description of blocker                      |
| `BlockedAt`     | DATETIME |                                   | When task was marked blocked                |
| `UnblockedAt`   | DATETIME |                                   | When blocker was resolved                   |
| `DelegatedTo`   | TEXT     |                                   | Name of person task is delegated to         |
| `CreatedAt`     | DATETIME | Default: CURRENT_TIMESTAMP        | When task was created                       |
| `UpdatedAt`     | DATETIME | Default: CURRENT_TIMESTAMP        | Updated on each modification (manually set) |

**Quadrant ENUM Values:**

* `'Do'` – Urgent + Important
* `'Schedule'` – Not Urgent + Important
* `'Delegate'` – Urgent + Not Important
* `'Delete'` – Not Urgent + Not Important
* `NULL` – Uncategorized (default for new tasks)

---

## ⚙️ Table Creation DDL

```sql
CREATE TABLE IF NOT EXISTS Tasks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Quadrant TEXT CHECK (Quadrant IN ('Do', 'Schedule', 'Delegate', 'Delete') OR Quadrant IS NULL),
    IsCompleted BOOLEAN DEFAULT 0,
    CompletedAt DATETIME,
    IsBlocked BOOLEAN DEFAULT 0,
    BlockerReason TEXT,
    BlockedAt DATETIME,
    UnblockedAt DATETIME,
    DelegatedTo TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

---

## 🔁 Relationships

Currently, the `Tasks` table is self-contained. In the future, it may be related to:
- `Projects` (via a `ProjectId` foreign key)
- `People` (for delegation, via `DelegatedTo` as a FK)
- Referenced by `ScrumNotes` for completed tasks in scrum summaries ([see Scrum Summary Database](feature-scrum-summary-database.md))

---

## 🔍 Sample Queries

**Get All Active Tasks:**

```sql
SELECT * FROM Tasks WHERE IsCompleted = 0 ORDER BY CreatedAt DESC;
```

**Get Blocked Items:**

```sql
SELECT * FROM Tasks WHERE IsBlocked = 1;
```

**Get Tasks Completed in Last 7 Days:**

```sql
SELECT * FROM Tasks WHERE CompletedAt >= DATE('now', '-7 days');
```

**Get All Uncategorized Tasks:**

```sql
SELECT * FROM Tasks WHERE Quadrant IS NULL AND IsCompleted = 0 ORDER BY CreatedAt DESC;
```
