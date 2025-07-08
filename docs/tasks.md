# ✅ Iterative Task List: Management Dashboard Platform

This is the **vibe-friendly micro-task development guide** that functions like a **build roadmap + personal PM**. This provides tightly-scoped, commit-ready checkpoints that evolve the platform in clean, testable steps.

Each task will be:

* Scoped to fit within a session or Pomodoro (20–60 min)
* End in a build/test/commit checkpoint
* Named clearly for versioning or commit messages
* Ordered to bootstrap infrastructure → utilities → features

---

## 🛠️ Phase 0: Project Bootstrap

### [x] Create Solution and Initial Project

* [x] 🧱 Create a new `.NET MAUI Blazor App` (name it `ManagementDashboard`)
* [x] 🔧 Set target platforms: Windows (✅), Android (optional for now)
* [x] 🚮 Delete all default pages (`Counter`, `FetchData`, etc.)
* [x] 🧪 Build & run to confirm project compiles and runs on desktop

> 💬 **Checkpoint:** Project loads, blank page, no errors.

---

### [x] Add Unit Testing Project

* [x] 📁 Add a new test project `ManagementDashboard.Tests` (.NET 8, MSTest or xUnit)
* [x] 🧬 Add basic test for true == true (smoke test)
* [x] 🧪 Confirm test discovery and run works

> 💬 **Checkpoint:** Solution contains app + working test project.

---

## 🎨 Phase 1: UI Framework & Shell Setup

### [1.1] Add Bootstrap 5 (Local)

* [x] 🧩 Add Bootstrap 5 CSS/JS to `wwwroot/bootstrap/` (no CDN, all static)
* [x] 💄 Reference Bootstrap in `index.html` and ensure it loads offline
* [x] 🧪 Add a `Button` (Bootstrap) on main screen to confirm it's rendering
> 💬 **Checkpoint:** Basic Bootstrap 5 layout with test UI element loads.

* [x] 🌐 Apply Bootstrap classes for layout and shell in `App.razor` and layouts

---

### [1.2] Build Dashboard Shell

* [x] 🧱 Create `MainLayout.razor` using Bootstrap 5 nav/sidebar/grid for layout (see code sample in project notes)
* [x] 🧭 Add navigation links for:

  * "Eisenhower Matrix"
  * "Scrum Summary"
* [x] 🧪 Style nav shell (light/dark mode switch optional, use Bootstrap utilities)
* [x] 🔄 Wire navigation via `@page` directive and `NavLink`s (Blazor/Bootstrap)
* [x] 🧪 Test responsive collapse/toggle on desktop and mobile

> 💬 **Checkpoint:** Navigation layout works, pages render in routed content, sidebar is responsive.

---

## 💽 Phase 2: SQLite + Repository Setup

### [2.1] Add SQLite & Formula.SimpleRepo

* [x] 📦 Install NuGet:

  * `Microsoft.Data.Sqlite`
  * `Dapper`
  * `Formula.SimpleRepo`
* [x] 🛠 (N/A) Create `SqliteConnectionFactory` using `IConnectionFactory` (not needed with Formula.SimpleRepo)
* [x] 🔌 Register configuration and repository in DI container
* [x] 🧪 Create and open connection to local `app.db` on first run (via migration runner)

> 💬 **Checkpoint:** SQLite database file is created and reachable.

---

### [2.2] Create Task Table & Seed

* [x] 🧾 Define `EisenhowerTask` model (as per definition)
* [x] 🗃️ Create raw SQL to create `Tasks` table if not exists
* [x] 🌱 Execute on startup if `Tasks` doesn’t exist (via migration runner)
* [x] 🧪 Confirm with direct SQL query (implicit via migration runner)

> 💬 **Checkpoint:** SQLite initialized with `Tasks` table, tested.

---

### [2.3] Build Repository for Tasks

* [x] 🔧 Implement `IEisenhowerTaskRepository` with `Formula.SimpleRepo`
* [x] 🧪 Add methods for:

  * AddTaskAsync
  * GetTasksByQuadrant
  * UpdateTaskAsync
* [ ] 🧪 Add unit tests (mock connection, assert query behavior)

> 💬 **Checkpoint:** Repos are wired and working with local test data (unit tests pending).

---

## 📐 Phase 3: Eisenhower Matrix Feature

### [3.1] Create Matrix Page + Grid

* [ ] 🧱 Create `/Pages/EisenhowerMatrix.razor`
* [ ] 🧮 Render 2x2 Bootstrap grid layout for four quadrants
* [ ] 🧪 Display hardcoded cards in each section

> 💬 **Note:** The Eisenhower Matrix feature will initially be built without the Scrum Summary integration. See Phase 5.4 for the integration sub-feature.

> 💬 **Checkpoint:** Static UI for matrix is in place and styled.

---

### [3.2] Load and Render Tasks

* [ ] 🔄 Fetch tasks from repo and bind by quadrant
* [ ] 🃏 Create `TaskCard.razor` component
* [ ] 🎯 Display task title, status (done/blocked), and actions

> 💬 **Checkpoint:** Real data rendered per quadrant.

---

### [3.3] Add Create Task Modal

* [ ] ➕ Add FAB or button to create task
* [ ] 🪟 Show Bootstrap modal with input fields (title, description, quadrant, delegate)
* [ ] ✅ Save to DB via repo

> 💬 **Checkpoint:** Create task flow works.

---

### [3.4] Task Actions (Edit / Move / Block / Done)

* [ ] 🛠 Add dropdown menu or icon buttons on `TaskCard`

  * Mark as Done
  * Block / Unblock
  * Move to another quadrant
  * Delete
* [ ] 🧪 All actions update DB and re-render state

> 💬 **Checkpoint:** Matrix is interactive and fully functional.

---

### [3.5] Add Audit Tooltips

* [ ] 🧾 Add Bootstrap tooltip to show created/updated/completed timestamps
* [ ] 🧪 Hover/click task to reveal audit history

> 💬 **Checkpoint:** Tasks show full lifecycle metadata.

---

## 📋 Phase 4: Scrum Summary Feature

### [4.1] Create Scrum Summary Page

* [ ] 📄 Create `/Pages/ScrumSummary.razor`
* [ ] 🗓 Show `EntryDate` defaulting to today
* [ ] 📑 Add editable text areas: Yesterday, Today, Blockers

> 💬 **Note:** The Scrum Summary feature will initially be built without the Eisenhower Matrix integration. See Phase 5.4 for the integration sub-feature.

> 💬 **Checkpoint:** Editable summary form is available.

---

### [4.2] Auto-fill From Tasks

* [ ] 🔄 Fetch completed (yesterday), active (today), and blocked tasks
* [ ] 🧠 Format as bulleted summary in text areas
* [ ] ✏️ Allow user to override/edit values

> 💬 **Checkpoint:** Auto-fill logic works.

---

### [4.3] Save and Load Daily Entries

* [ ] 💾 Save `ScrumNote` to DB on submit
* [ ] 🔄 Check for existing note — update instead of insert
* [ ] 🧪 Add list of previous entries to page (optional for now)

> 💬 **Checkpoint:** Scrum entries persist with editable history.

---

## 🌱 Phase 5: Cleanup and Reusability

### [5.1] Centralize Services

* [ ] 🧼 Move repo access to `TaskService`, `ScrumService`
* [ ] 🧪 Add unit tests to services
* [ ] 🔁 Inject into pages via DI

> 💬 **Checkpoint:** Pages decoupled from raw repositories.

---

### [5.2] Add Basic Theme / Branding

* [ ] 🎨 Configure default Bootstrap theme (colors, typography)
* [ ] 🧪 Add favicon / titlebar / app name

> 💬 **Checkpoint:** Platform has clean, branded polish.

---

### [5.3] Tag First Release

* [ ] 📦 Tag commit as `v0.1.0` — MVP with both features working
* [ ] 📸 Create a screenshot or README preview
* [ ] ✅ Push to GitHub (private or public)

---

### [5.4] Integrate Eisenhower Matrix with Scrum Summary (Sub-Feature)

* [ ] 🔄 Implement auto-pull of completed/updated Eisenhower tasks into Scrum Summary's "Yesterday's Work" section
* [ ] 🧪 Ensure manual override/edit is possible in Scrum Summary
* [ ] 🔗 Link each Scrum Summary entry to an optional Eisenhower task via `TaskId` foreign key
* [ ] 🧪 Add tests for integration logic and data consistency

> 💬 **Checkpoint:** Scrum Summary and Eisenhower Matrix are fully integrated; completed/updated tasks are surfaced in daily summaries, and manual entries remain supported.

---

## 📅 Bonus: Next-Level Ideas for v0.2+

| Idea                      | Scope            |
| ------------------------- | ---------------- |
| Search/filter tasks       | Eisenhower       |
| Markdown export for Scrum | Scrum            |
| Reminder or notification  | Shared           |
| Timeline visualization    | Matrix + Summary |

