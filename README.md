# 🧭 Management Dashboard Platform

The **Management Dashboard Platform** is a modular, cross-platform app built with **.NET MAUI + Blazor Hybrid**, designed for software architects, engineering managers, and team leads to stay organized and focused.

It features a set of lightweight utilities designed to streamline task management, daily planning, and technical leadership workflows — all running locally with no external dependencies.

---

## 📦 Features

### ✅ Eisenhower Matrix Utility
Prioritize and manage your tasks across four quadrants: **Do**, **Schedule**, **Delegate**, and **Delete**.  
Track completion, delegation, blockers, and audit history.

📄 See [docs/feature-eisenhower-matrix.md](docs/feature-eisenhower-matrix.md)

---

### 📋 Scrum Call Summary Dashboard
Auto-generate a summary of your daily stand-up using your activity history.  
Answer the three core Scrum questions with one click and save your daily entries.

📄 See [docs/feature-scrum-summary.md](docs/feature-scrum-summary.md)

---

## 🧱 Architecture

The app is built using a clean layered architecture:
- **.NET MAUI + Blazor Hybrid** frontend
- **Bootstrap 5** for modern, responsive UI components
- **SQLite** for lightweight local storage
- **Dapper + Formula.SimpleRepo** for fast and structured data access

📄 See [docs/architecture-definition.md](docs/architecture-definition.md)

---

## 🗃️ Database Design

A simple, audit-friendly SQLite schema with extensibility for future modules.  
Includes tables for tasks (`Tasks`) and scrum summaries (`ScrumNotes`).

📄 See [docs/database-definitions.md](docs/database-definitions.md)

---

## 📌 Project Objectives

This platform is designed to be:
- **Modular**: Add new utilities over time without major refactor
- **Offline-first**: Works entirely without internet
- **Fast & Focused**: Only essential tools, no fluff

📄 See [docs/product-definition.md](docs/product-definition.md)

---

## 🧩 Development Tasks & Roadmap

Development is structured into iterative, vibe-friendly micro-tasks.  
Each task is designed to be commit-ready, testable, and small in scope.

📄 See [docs/tasks.md](docs/tasks.md)

---

## 🚀 Getting Started

> ⚙️ Prerequisites:
- .NET 8 SDK
- Visual Studio 2022 or newer (with MAUI + Android workloads installed)
- SQLite tools (optional for inspection)

```bash
git clone https://github.com/joshua-p-williams/management-dashboard
cd management-dashboard
dotnet build
```

To run:
```bash
dotnet run --project ManagementDashboard
```

---

## 💬 Contributing

This project is maintained in a "vibe coding" workflow. If you're adding new utilities or features, please follow the architectural structure and update:
- `docs/tasks.md` with your task plan
- `docs/database-definitions.md` if you change the schema

---

## 🧩 Bootstrap 5 Maintenance & Upgrade Strategy

This project uses **Bootstrap 5 (currently v5.3.7)** for all UI styling, included as static assets for full offline support. To maintain and upgrade Bootstrap in a safe, predictable way:

1. **Location of Assets:**
   - CSS: `ManagementDashboard/wwwroot/bootstrap/css/bootstrap.min.css`
   - JS:  `ManagementDashboard/wwwroot/bootstrap/js/bootstrap.bundle.min.js`

2. **Upgrade Process:**
   - Download the latest compiled Bootstrap assets from [getbootstrap.com](https://getbootstrap.com/docs/5.3/getting-started/download/).
   - Place the CSS and JS files in the `wwwroot/bootstrap/css/` and `wwwroot/bootstrap/js/` folders, respectively.
   - Test the app offline to confirm no regressions in layout or interactivity.
   - Review the [Bootstrap migration guide](https://getbootstrap.com/docs/5.3/migration/) for any breaking changes.
   - Update this README and any relevant documentation with the new version number and date.

3. **Best Practices:**
   - Never reference Bootstrap from a CDN or external source—always use local files.
   - Keep custom CSS in separate files (e.g. `app.css`) to simplify Bootstrap upgrades.
   - After upgrading, check all major UI flows and run a full build/test cycle.

4. **Version Tracking:**
   - Document Bootstrap version and upgrade date in this section.
   - Example: `v5.3.7 added July 2025`

---

## 📄 License

MIT License © Joshua P Williams
