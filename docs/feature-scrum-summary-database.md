# 🗃️ Scrum Summary Database Definition (Revised)

This document defines the improved database schema for the **Scrum Call Summary Dashboard** in the Management Dashboard Platform.

---

## 📄 Purpose

Enables flexible, auditable work capture and reporting for scrum ceremonies. Supports both structured (task-based) and unstructured (freeform) work capture, allowing the Scrum Summary to dynamically report on all relevant activity.

---

## 🧱 Table: `WorkCaptureNotes`

| Column Name   | Type     | Constraints                 | Description                                   |
| ------------- | -------- | --------------------------- | --------------------------------------------- |
| `Id`          | INTEGER  | Primary Key, AutoIncrement  | Unique ID                                     |
| `Notes`       | TEXT     |                             | Freeform work capture note                    |
| `TaskId`      | INTEGER  | Nullable, FK to Tasks.Id    | (Optional) Related Eisenhower task            |
| `CreatedAt`   | DATETIME | Default: CURRENT_TIMESTAMP  | When the note was created                     |
| `UpdatedAt`   | DATETIME | Default: CURRENT_TIMESTAMP  | When the note was last edited                 |

---

## ⚙️ Table Creation DDL

```sql
CREATE TABLE IF NOT EXISTS WorkCaptureNotes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Notes TEXT,
    TaskId INTEGER,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id)
);
```

---

## 🔁 Relationships

- `WorkCaptureNotes` can be linked to a task (via `TaskId`) or stand alone.
- Scrum Summary queries both `WorkCaptureNotes` and `Tasks` for reporting.

---

## 🔍 Sample Queries

**Get Work Capture Notes for a Date:**

```sql
SELECT * FROM WorkCaptureNotes WHERE DATE(CreatedAt) = DATE('now', '-1 day');
```

**Get Tasks Updated or Completed on a Date:**

```sql
SELECT * FROM Tasks WHERE DATE(UpdatedAt) = DATE('now', '-1 day') OR DATE(CompletedAt) = DATE('now', '-1 day');
```

**Get All Blocked Tasks:**

```sql
SELECT * FROM Tasks WHERE BlockedAt IS NOT NULL AND (UnblockedAt IS NULL OR UnblockedAt < BlockedAt);
```
