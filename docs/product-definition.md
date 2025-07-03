# 🧭 Product Definition: Management Dashboard Platform

## 💡 Product Name (Working Title)

**Project Compass**
*A personal command center for tech leads and architects.*

---

## 🎯 Purpose

The **Management Dashboard Platform** is a modular, extensible toolset built to assist technical leaders in managing priorities, tracking tasks, and reflecting on daily progress — all from a single unified interface. It provides lightweight utilities aimed at organizing thoughts, visualizing priorities, and answering key questions without relying on heavyweight enterprise tools.

---

## 🧑‍💼 Target Audience

* Software Architects
* Engineering Managers / Directors
* Technical Leads
* Individual Contributors looking for lightweight personal organization tools

---

## 🛠️ Key Characteristics

| Trait              | Description                                                              |
| ------------------ | ------------------------------------------------------------------------ |
| **Platform**       | .NET MAUI / Blazor Hybrid for cross-platform support (desktop & mobile)  |
| **Persistence**    | SQLite (file-based) local database with schema evolvability              |
| **UI Framework**   | MudBlazor — sleek, responsive UI components                              |
| **Modular Design** | Architecture allows the easy addition of new utilities or micro-tools    |
| **Offline-first**  | Works completely offline with local persistence and syncing potential    |
| **Personal Scope** | Tool is designed for individual use, with potential for cloud sync later |

---

## 🧩 Core Modules (Initial Scope)

1. **Eisenhower Matrix Utility**

   * Visual quadrant for prioritizing tasks by urgency/importance
   * Task management with editing, re-categorizing, blockers, and auditing

2. **Scrum Call Summary Dashboard**

   * Summarizes prior work, current plans, and blockers
   * Automatically pulls relevant items from the Eisenhower Matrix
   * Enables exporting/sharing summaries (future enhancement)

---

## 🔐 Core Functional Requirements

| Capability                | Description                                                                |
| ------------------------- | -------------------------------------------------------------------------- |
| Local SQLite Database     | All data persisted locally with timestamp and status tracking              |
| Add/Edit/Delete Tasks     | Full CRUD operations for utility records                                   |
| Eisenhower Categorization | Support for 4 quadrants: Do, Decide, Delegate, Delete                      |
| Task Auditing             | Tracks created/updated/completed timestamps                                |
| Blocker Handling          | Supports flagging blockers with descriptions and resolution timestamps     |
| Delegation Support        | Assign task to someone (name/email freeform text initially)                |
| Scrum Summary             | Displays “Yesterday”, “Today”, and “Blockers” summary based on task states |
| UX-first Navigation       | Clean, touch-friendly interface powered by MudBlazor components            |

---

## 🔮 Future Capabilities (Beyond MVP)

| Feature                     | Idea                                                                |
| --------------------------- | ------------------------------------------------------------------- |
| Notification System         | Reminders for tasks with due dates or check-ins                     |
| Cloud Sync or GitHub Backup | Optionally sync to GitHub repo or cloud storage (OneDrive, Dropbox) |
| AI Copilot Prompts          | Summarize Eisenhower quadrant or suggest priorities using AI        |
| Calendar View Integration   | Show daily focus pulled from matrix + summary                       |
| Team Sharing Mode           | Optional collaboration mode with shared tasks and visibility        |

---

## 🧪 Constraints

* Must be usable **entirely offline**
* SQLite only (no external DBs or APIs initially)
* Cross-platform UI consistency via MudBlazor
* Easy deployment (single click install, side-loadable builds)

---

## 🧭 Guiding Principles

* **"Add as you go"**: Design so that new modules/utilities can be dropped in with minimal rework
* **"Think like a solo app suite"**: Every utility should work well standalone but integrate into the dashboard
* **"Developer-first UX"**: Prioritize low friction, keyboard-friendly, minimal-click workflows

---

## 🔗 Initial Dependencies

| Dependency  | Purpose                       |
| ----------- | ----------------------------- |
| .NET MAUI   | Hybrid App Platform           |
| Blazor      | UI rendering and logic        |
| MudBlazor   | UI component library          |
| SQLite      | Embedded database             |
| Dapper / SimpleRepo | Lightweight ORM |
