
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
- **Blazorise + Fluent** for modern, responsive UI components
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

## 📄 License

MIT License © Joshua P Williams
