# 🗃️ Database Definition – Management Dashboard Platform

## 📄 Overview

This document provides a high-level summary of the database features and links to detailed per-feature database definitions. Each feature's database schema is documented separately for clarity and maintainability.

---

## 🧱 Core Database Features

### 1. Eisenhower Matrix Tasks

The Eisenhower Matrix database feature stores and manages all actionable tasks, their status, quadrant, delegation, blockers, and audit trail for the Eisenhower Matrix utility.

- 📄 **Detailed Schema:** [feature-eisenhower-matrix-database.md](feature-eisenhower-matrix-database.md)
- 🔗 **Related Feature:** [Eisenhower Matrix Utility](feature-eisenhower-matrix.md)

---

### 2. Scrum Call Summary

The Scrum Summary database feature stores daily scrum entries, including yesterday's work, today's plan, and blockers, for the Scrum Summary Dashboard utility.

- 📄 **Detailed Schema:** [feature-scrum-summary-database.md](feature-scrum-summary-database.md)
- 🔗 **Related Feature:** [Scrum Call Summary Dashboard](feature-scrum-summary.md)

---

## 🔗 Suggested Future Tables

| Table Name  | Purpose                                     |
| ----------- | ------------------------------------------- |
| `Projects`  | Group tasks into projects or contexts       |
| `Tags`      | Enable keyword tagging for task filtering   |
| `Reminders` | Optional timed reminders and notifications  |
| `People`    | Named entities for delegation auto-complete |

---

## 🧰 Formula.SimpleRepo Setup Notes

* Every table needs a primary key named `Id` (✓)
* Add `[Table]`, `[Key]`, `[Column]` attributes as needed for mapping
* Can define `BaseEntity` class with audit fields (`CreatedAt`, `UpdatedAt`, etc.)

