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
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

---

## 🔁 Relationships

Currently, the `ScrumNotes` table is self-contained. In the future, it may reference:
- `Tasks` completed on a given day, for richer scrum summaries ([see Eisenhower Matrix Database](feature-eisenhower-matrix-database.md))

---

## 🔍 Sample Queries

**Get Scrum Notes for Today:**

```sql
SELECT * FROM ScrumNotes WHERE EntryDate = DATE('now');
```
