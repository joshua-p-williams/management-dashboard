# 🗃️ Database Definition – Management Dashboard Platform

## 📄 Overview

This document defines the schema, relationships, and design rationale for the local SQLite database used by the Management Dashboard Platform. The schema is designed to support offline persistence, support auditable workflows, and allow future extensions.

---

## 📦 Database File

* **File Location**: Local file named `app.db`
* **Storage Engine**: SQLite (single-file)
* **ORM Layer**: Dapper via [Formula.SimpleRepo](https://github.com/NephosIntegration/Formula.SimpleRepo)

---

## 🧱 Core Tables

### 1. `Tasks` (Eisenhower Matrix Items)

| Column Name     | Type     | Constraints                       | Description                                 |
| --------------- | -------- | --------------------------------- | ------------------------------------------- |
| `Id`            | INTEGER  | Primary Key, AutoIncrement        | Unique ID                                   |
| `Title`         | TEXT     | NOT NULL                          | Short title of task                         |
| `Description`   | TEXT     |                                   | Optional details                            |
| `Quadrant`      | TEXT     | NOT NULL, CHECK (IN values below) | 'Do', 'Schedule', 'Delegate', 'Delete'      |
| `IsCompleted`   | BOOLEAN  | Default: 0                        | Whether task is marked done                 |
| `CompletedAt`   | DATETIME |                                   | Nullable timestamp of completion            |
| `IsBlocked`     | BOOLEAN  | Default: 0                        | Whether task is blocked                     |
| `BlockerReason` | TEXT     |                                   | Description of blocker                      |
| `BlockedAt`     | DATETIME |                                   | When task was marked blocked                |
| `UnblockedAt`   | DATETIME |                                   | When blocker was resolved                   |
| `DelegatedTo`   | TEXT     |                                   | Name of person task is delegated to         |
| `CreatedAt`     | DATETIME | Default: CURRENT\_TIMESTAMP       | When task was created                       |
| `UpdatedAt`     | DATETIME | Default: CURRENT\_TIMESTAMP       | Updated on each modification (manually set) |

**Quadrant ENUM Values:**

* `'Do'` – Urgent + Important
* `'Schedule'` – Not Urgent + Important
* `'Delegate'` – Urgent + Not Important
* `'Delete'` – Not Urgent + Not Important

---

### 2. `ScrumNotes` (Daily Scrum Entry Snapshots)

| Column Name      | Type     | Constraints                 | Description                             |
| ---------------- | -------- | --------------------------- | --------------------------------------- |
| `Id`             | INTEGER  | Primary Key, AutoIncrement  | Unique ID                               |
| `EntryDate`      | DATE     | NOT NULL                    | Date of scrum call entry                |
| `YesterdayNotes` | TEXT     |                             | Summary of what was done yesterday      |
| `TodayPlan`      | TEXT     |                             | Tasks or intentions for today           |
| `Blockers`       | TEXT     |                             | Known blockers (can be linked or typed) |
| `CreatedAt`      | DATETIME | Default: CURRENT\_TIMESTAMP | When entry was created                  |
| `UpdatedAt`      | DATETIME | Default: CURRENT\_TIMESTAMP | When entry was last edited              |

---

## 🔗 Suggested Future Tables

| Table Name  | Purpose                                     |
| ----------- | ------------------------------------------- |
| `Projects`  | Group tasks into projects or contexts       |
| `Tags`      | Enable keyword tagging for task filtering   |
| `Reminders` | Optional timed reminders and notifications  |
| `People`    | Named entities for delegation auto-complete |

---

## 🔁 Relationships

Currently all tables are flat and self-contained. In future:

* `Tasks.DelegatedTo` could become a FK to `People.Name` or `People.Id`
* `Tasks.ProjectId` → `Projects.Id`
* `ScrumNotes.EntryDate` can link back to `Tasks` completed that day

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

**Get Scrum Notes for Today:**

```sql
SELECT * FROM ScrumNotes WHERE EntryDate = DATE('now');
```

**Get Tasks Completed in Last 7 Days:**

```sql
SELECT * FROM Tasks WHERE CompletedAt >= DATE('now', '-7 days');
```

---

## ⚙️ Table Creation DDL

```sql
CREATE TABLE IF NOT EXISTS Tasks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Quadrant TEXT NOT NULL CHECK (Quadrant IN ('Do', 'Schedule', 'Delegate', 'Delete')),
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

CREATE TABLE IF NOT EXISTS ScrumNotes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryDate DATE NOT NULL,
    YesterdayNotes TEXT,
    TodayPlan TEXT,
    Blockers TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

---

## 🧰 Formula.SimpleRepo Setup Notes

* Every table needs a primary key named `Id` (✓)
* Add `[Table]`, `[Key]`, `[Column]` attributes as needed for mapping
* Can define `BaseEntity` class with audit fields (`CreatedAt`, `UpdatedAt`, etc.)

