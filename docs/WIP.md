# 🚧 Work In Progress: Eisenhower Matrix Feature

> **Reference: Eisenhower Matrix Feature Analysis & Implementation Plan**
>
> ### Step 1: Feature Analysis
> - **README.md:** App is modular, Eisenhower Matrix is a core feature, iterative development.
> - **feature-eisenhower-matrix.md:** User goals: capture, view, manage tasks in quadrants; drag-and-drop, edit, mark complete, filtering, visual clarity. User stories: add, move, complete, edit, delete, view, and get visual feedback on tasks.
> - **feature-eisenhower-matrix-database.md:** Model: ID, Title, Description, Quadrant, Due Date, Priority, Status, Created/Modified, Audit Trail, Blocked, BlockerReason, CompletedBy, CompletionTimestamp.
> - **tasks.md:** Atomic tasks for UI, CRUD, drag-and-drop, filtering, audit trail.
>
> ### Step 2: Best-Practice UI/UX
> - Responsive (desktop/mobile), Bootstrap 5 grid/cards, accessible (ARIA, keyboard), modern look, key actions surfaced.
>
> ### Step 3: Developer Instructions
> - **Component Structure:** EisenhowerMatrix.razor (main), TaskCard.razor, TaskEditor.razor, TaskAuditTrail.razor.
> - **Page Layout:** Bootstrap 2x2 grid, quadrant cards with headers/icons/add button.
> - **Task Card UI:** Title, due date, status, priority, inline actions, visual cues, drag-and-drop.
> - **Task Editor Modal:** All fields, validation, save/cancel, Bootstrap modal.
> - **Task Actions:** Edit, delete, complete, audit trail.
> - **Visual Cues:** Overdue, blocked, completed, priority badges/icons.
> - **Filtering/Sorting:** Optional global filter bar, sort by incomplete/due/priority.
> - **Accessibility:** ARIA roles, keyboard navigation.
> - **Styling:** Bootstrap 5 only, use icons.
> - **Code Organization:** Components in /Components/EisenhowerMatrix/, DI for repo, state updates.
> - **Developer Tasks:** Scaffold, layout, CRUD, drag-and-drop, actions, cues, filtering, tests.

---

## 🗺️ Current Strategy & Checklist

- [x] Scaffold `TaskEditor` component (basic add mode)
- [x] Scaffold `TaskAuditTrail` component (placeholder)
- [x] Implement basic add-task modal (title, description, quadrant)
- [x] Expand `TaskEditor` to support edit mode and all model fields
  - [x] Add fields: Completed At, Priority, Status, Blocker Reason, Delegated To
  - [x] Add dropdown for quadrant selection
  - [x] Add logic for both add and edit modes
  - [x] Map new fields to model
  - [x] Add validation for new fields
  - [ ] Wire up priority (currently placeholder, not in model)
  - [ ] Add accessibility/ARIA improvements (tabbing works, but ARIA roles could be improved)
- [x] Implement `TaskAuditTrail` logic and parameters
- [x] Integrate `TaskEditor` with main EisenhowerMatrix page (add/edit)
  - [x] Wire up edit (pencil icon) to open TaskEditor in edit mode
- [ ] Implement drag-and-drop or move logic for tasks between quadrants
- [x] Add visual cues (badges/icons for overdue, blocked, completed, priority)
  - [ ] Add visual cue for priority (badge/icon)
  - [ ] Make overdue threshold a configurable setting (default 2 days)
- [ ] Add filtering/sorting UI (optional)
- [ ] Write or update unit/component tests

---

## 📝 Notes & Progress Log

- **2025-07-08:**
  - Add-task modal works, validation and cancel work, blocked logic works, accessibility (tabbing) is good.
  - Editing is now wired up from the pencil icon (edit modal opens and is pre-filled).
  - TaskAuditTrail modal now shows audit info for the selected task (created, updated, completed, blocked/unblocked).
  - Visual cues (badges/icons) for Done, Blocked, Active, and Overdue are now shown on TaskCard. Overdue is currently hardcoded to 2 days for Do tasks.
  - Next: Add visual cue for priority, make overdue threshold a configurable setting, then drag-and-drop.

---

## ⚙️ Settings Notes

- **Dark mode / Light mode:** Currently the only user setting.
- **Overdue threshold:** To be added. Will control how many days before a Do task is considered overdue (default: 2 days). TaskCard should honor this setting for the Overdue badge.

---

## ⏭️ Next Steps

- [ ] Build and run the app to verify visual cues
- [ ] Add visual cue for priority (badge/icon)
- [ ] Make overdue threshold a configurable setting (default 2 days)
- [ ] Begin implementing drag-and-drop or move logic for tasks between quadrants
- [ ] Continue incremental improvements as per checklist above
