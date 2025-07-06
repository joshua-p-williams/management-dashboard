# 🏗️ Architecture Definition: Management Dashboard Platform

## 🔧 Architectural Overview

This application is a **modular, component-based hybrid app** built using the .NET MAUI framework with Blazor for UI composition. It adopts a **clean layered architecture** and uses **Formula.SimpleRepo** with **Dapper** to manage SQLite persistence. The focus is on an offline-first experience with modular utility panels that plug into a central navigation shell.

---

## 📚 Architectural Layers

```
[ AppShell (MAUI) ]
       ↓
[ Blazor UI (Bootstrap 5 Components, local/static) ]
       ↓
[ UI Logic (Pages / Components) ]
       ↓
[ Application Layer (Services / Use Cases) ]
       ↓
[ Data Layer (SimpleRepo Repositories + SQLite Models) ]
       ↓
[ Local SQLite Database ]
```

---

## 🧑‍🔬 Key Technologies & Patterns

| Component            | Tool/Library                                 | Purpose                                                    |
| -------------------- | -------------------------------------------- | ---------------------------------------------------------- |
| UI Framework         | **Bootstrap 5 (local/static)**               | Responsive, semantic UI components (offline, no CDN)        |
| Shell/Platform       | **.NET MAUI Blazor Hybrid**                  | Cross-platform host (Android, Windows, Mac, iOS optional)  |
| Data Access          | **Dapper + Formula.SimpleRepo**              | Repository-based lightweight data access layer             |
| Database             | **SQLite**                                   | File-based local data persistence                          |
| Navigation           | **MAUI Shell + Blazor Routing**              | Host app navigation + component routing                    |
| Dependency Injection | **Microsoft.Extensions.DependencyInjection** | Used for services, repositories, etc.                      |
| State Handling       | **Scoped/Singleton services**                | In-memory transient states (e.g., current session context) |
| Timestamp Auditing   | **Handled in models + repository layer**     | Auto-setting `CreatedAt`, `CompletedAt`, etc.              |

---

## 🧩 Application Modules

| Module                | Description                                                              |
| --------------------- | ------------------------------------------------------------------------ |
| **Core Dashboard**    | Navigation shell and utility entrypoints (sidebar, drawer, or grid)      |
| **Eisenhower Matrix** | Standalone utility feature panel with quadrant logic and task management |
| **Scrum Summary**     | Summary feature pulling from shared data with its own UI                 |
| **Common Services**   | Timestamps, date helpers, blocking logic, notifications                  |
| **Persistence Layer** | SQLite file, migrations (manual or versioned), SimpleRepo model wrappers |
| **Data Models**       | Shareable POCOs between modules with audit fields                        |

---

## 🔁 Data Model Strategy

### Base Entity Interface

```csharp
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? CompletedAt { get; set; }
    bool IsBlocked { get; set; }
    string? BlockerReason { get; set; }
    DateTime? BlockedAt { get; set; }
    DateTime? UnblockedAt { get; set; }
}
```

Each domain entity (e.g. `EisenhowerItem`, `ScrumNote`) implements this.

---

## 📂 Project Structure (Suggested)

```
/ManagementDashboard
│
├── /Platforms/        // MAUI startup config (Windows, Android, Mac)
├── /Pages/            // Blazor pages (Dashboard.razor, Eisenhower.razor, etc.)
├── /Components/       // UI subcomponents (TaskCard, BlockerBadge, etc.)
├── /Services/         // Business logic (TaskService, SummaryService)
├── /Repositories/     // Dapper SimpleRepo implementations
├── /Models/           // POCOs for Tasks, Summaries, etc.
├── /Data/             // SQLite setup, migrations, and seed logic
├── /Shared/           // Reusable utilities and interfaces
└── /wwwroot/          // Static assets (icons, styles)
```

---

## 🧭 Navigation Concept

Use a left-hand sidebar (MudDrawer) or top nav to switch between utilities. Example:

* 🧠 Eisenhower Matrix
* 📋 Scrum Summary
* ➕ Add Utility (Placeholder for new modules)

Each page/component is a plug-and-play unit that binds to common services and models.

---

## 🧪 DevOps / Build Strategy

| Area           | Notes                                                                |
| -------------- | -------------------------------------------------------------------- |
| Build System   | Standard .NET SDK CLI or Visual Studio (.NET 8)                      |
| DB Handling    | Seed SQLite file on first run (or prompt user to create new one)     |
| Cross-Platform | Android and Windows Desktop as primary targets (others optional)     |
| Testing        | Unit tests for services, manual testing for UI (UI automation later) |

---

## 🧭 Extensibility Approach

Future modules can be added using this checklist:

* Define feature in `/Pages/FeatureX.razor`
* Add service in `/Services/FeatureXService.cs`
* Add model in `/Models/FeatureXModel.cs`
* Register routes + navigation in `App.razor` or layout
* Add repository with Formula.SimpleRepo mapping

No core refactors needed if this contract is respected.

---

## ⚙️ Dependency Injection (Startup Example)

```csharp
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();
builder.Services.AddSingleton<IConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddScoped<IRepository<EisenhowerItem>, EisenhowerItemRepo>();
```
