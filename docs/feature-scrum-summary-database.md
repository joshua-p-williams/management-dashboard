# 🗃️ Scrum Summary Database Definition

This document defines the database schema and rationale for the **Scrum Call Summary Dashboard** feature in the Management Dashboard Platform.

> 🔗 **Back to Feature:** [Scrum Call Summary Dashboard](feature-scrum-summary.md)

---

## 📄 Purpose

Stores daily scrum entries, including yesterday's work, today's plan, and blockers, for the Scrum Summary Dashboard utility.

---

## 🧱 Table: `ScrumNotes`

| Column Name      | Type     | Constraints                 | Description                             |
| ---------------- | -------- | --------------------------- | --------------------------------------- |
| `Id`             | INTEGER  | Primary Key, AutoIncrement  | Unique ID                               |
| `EntryDate`      | DATE     | NOT NULL                    | Date of scrum call entry                |
| `YesterdayNotes` | TEXT     |                             | Summary of what was done yesterday      |
| `TodayPlan`      | TEXT     |                             | Tasks or intentions for today           |
| `Blockers`       | TEXT     |                             | Known blockers (can be linked or typed) |
| `TaskId`         | INTEGER  | Optional, FK to Tasks.Id    | (Optional) Related Eisenhower task, if any |
| `CreatedAt`      | DATETIME | Default: CURRENT_TIMESTAMP  | When entry was created                  |
| `UpdatedAt`      | DATETIME | Default: CURRENT_TIMESTAMP  | When entry was last edited              |

---

## ⚙️ Table Creation DDL

```sql
CREATE TABLE IF NOT EXISTS ScrumNotes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EntryDate DATE NOT NULL,
    YesterdayNotes TEXT,
    TodayPlan TEXT,
    Blockers TEXT,
    TaskId INTEGER, -- Optional, FK to Tasks.Id
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id)
);
```

---

## 🔁 Relationships

The `ScrumNotes` table can optionally reference a single task from the Eisenhower Matrix via the `TaskId` column (foreign key to `Tasks.Id`). This allows a scrum summary entry to be linked to a specific task when relevant, but does not require every summary entry to be tied to a task. Manual notes and summaries unrelated to Eisenhower tasks are fully supported.

- For more on the Eisenhower Matrix schema, see [Eisenhower Matrix Database](feature-eisenhower-matrix-database.md)

---

## 🔍 Sample Queries

**Get Scrum Notes for Today:**

```sql
SELECT * FROM ScrumNotes WHERE EntryDate = DATE('now');
```

**Get Scrum Notes Linked to a Specific Task:**

```sql
SELECT * FROM ScrumNotes WHERE TaskId = 42;
-- Replace 42 with the desired Task ID
```
