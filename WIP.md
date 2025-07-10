Analyze the README.md and follow the links within it to the contents within the ./docs/ subfolder reading those markdown documents to get an understanding of the project that we are working on.  Then analalyze the solution itself to get an idea of what has already been built.  Then start looking at the ./docs/tasks.md as we will need to pick back up on the next unfinished tasks.  After you've done all this analysis report back to me if you agree we are in a state to continue.

Also familiarize yourself with the layout of the solution and it's various projects.

ManagementDashboard = MAUI Blazor Hybrid App
ManagementDashboard.Data = Data Access Layer holding all data models repositories and sql migrations
ManagementDashboard.Core = Where to store common business logic and services created in a testable way
ManagementDashboard.Tests = The unit testing project.

I want you to serve the role of a Software Engineer implementing various features for me.

----------------

Refer to the [Scrum Summary Database Definition](feature-scrum-summary-database.md) for the data model and relationships that will inform the UI design. Also refer to the [Scrum Summary Feature Definition](feature-scrum-summary.md) for the feature requirements and user stories.  There is a "UI/UX Design Considerations (To Be Determined)" section in the feature definition that outlines open questions for UI/UX design.  Use this as a starting point to propose a design that addresses these questions and provides a cohesive user experience.

# 📋 **Personal Scrum Ceremony Summary – UI/UX and Micro-Task Plan (WIP)**

> **How to use:**
> Update this WIP as design/implementation evolves. Each checkpoint can be updated as tasks are in progress/completed or requirements shift. Use as an ongoing “vibe” code-generation checklist for Copilot or dev handoff.

---

## **1. UI/UX Principles & Layout Direction**

* **Audience:** Single user, prepping their own talking points and blockers.
* **Workflow:** Quick entry, easy edit, focused on the user’s own work.
* **Responsive:** Optimized for both desktop and mobile—no horizontal scrolling needed.
* **Clarity:** Each of the 3 scrum questions is visually separated but equally accessible.

---

## **2. Best-Practice Layout Proposal**

### **Desktop**

* Use a **single card with 3 “question panels” in a row** (Yesterday | Today | Blockers).
* Each panel includes its entries (structured/unstructured), a prominent “Add” button, and clear section heading.

### **Mobile/Tablet**

* **Stack panels vertically** so each section is full-width and readable.
* Add persistent sticky/fixed “+ Add” button for quick capture.

### **Universal**

* All key actions are accessible with large touch targets.
* Use Bootstrap 5’s responsive grid (`row` + `col-md-4`) and utility classes (`flex-column`, etc.) to control stacking.

---

## **3. UI Components & Structure**

### **A. ScrumSummary.razor (Main Page)**

* [x] **Header**: Title + date selector (to review/present past days)
* [x] **Panels**: Render 3 summary panels (“Yesterday”, “Today”, “Blockers”)

  * **Desktop:** `d-flex flex-row` with `col-md-4`
  * **Mobile:** `flex-column`, each `w-100 mb-3`
* [ ] **(Optional) Quick Nav:** Tab buttons or scrollspy to jump to each section on long screens.

### **B. ScrumQuestionPanel.razor**

* [x] For each section (Yesterday, Today, Blockers):

  * Heading with icon + tooltip (explains question)
  * List of entries (task/note rows, edit/delete)
  * “+ Add” button at bottom or top-right

### **C. Add/Edit Entry Modal**

* [x] Modal for quick entry (add only for now):

  * Input for structured task selection (dropdown/search) [pending]
  * Textarea for freeform note
  * (Blockers: separate field for details) [pending]
  * Save/Cancel
* [x] Close modal on save/cancel, refresh panel data

### **D. Entry Display**

* [ ] Show icon for task/note, with badges for status
* [ ] Ellipsis menu for edit/delete
* [ ] Touch-friendly spacing (min 44px button areas)

### **5. Responsive**

* [x] Confirm 3 panels **stack** vertically on mobile
* [x] All text/controls readable and usable one-handed
* [ ] “+ Add” button is always visible (or sticky/fixed on mobile for quick add)

### **6. Accessibility**

* [ ] Ensure all headings/buttons are keyboard/tab navigable
* [ ] Tooltips and icons have `aria-labels`

---

## **4. Micro-task Checklist**

### **0. Data Access Layer**

* [x] Implement WorkCaptureNotes data access layer (model, migration, constraints, repository) according to feature-scrum-summary-database.md and following the same patterns as Eisenhower Matrix in ManagementDashboard.Data.

### **1. Page & Layout**

* [x] Scaffold **ScrumSummary.razor** (initial markup; needs code-behind refactor)
* [x] Add page header with icon and “Pick date” control (`<input type="date">` or Bootstrap datepicker)
* [x] Arrange 3 question panels in a **row** on desktop (`d-none d-md-flex flex-row`) and **stacked** on mobile (`d-flex flex-column d-md-none`)
* [x] Ensure each panel has equal height/spacing

### **2. Panels**

* [x] Build **ScrumQuestionPanel.razor** component with:

  * Heading + tooltip (e.g., Yesterday: “What did you work on yesterday?”)
  * List of entries (task/note rows, edit/delete)
  * Prominent **“+ Add” button** (top right or bottom center)
  * For blockers, use warning colors/icons (Bootstrap `text-warning`, etc.)

### **3. Add/Edit Modal**

* [x] Reusable modal for adding/editing entries

  * Input: link a structured task (dropdown/search)
  * Textarea: note
  * (Blockers: separate field for details)
  * Save/Cancel; auto-focus on open
* [x] Close modal on save/cancel, refresh panel data

### **4. Entry Display**

* [ ] Show icon for task/note, with badges for status
* [ ] Ellipsis menu for edit/delete
* [ ] Touch-friendly spacing (min 44px button areas)

### **5. Responsive**

* [x] Confirm 3 panels **stack** vertically on mobile
* [x] All text/controls readable and usable one-handed
* [ ] “+ Add” button is always visible (or sticky/fixed on mobile for quick add)

### **6. Accessibility**

* [ ] Ensure all headings/buttons are keyboard/tab navigable
* [ ] Tooltips and icons have `aria-labels`

---

## **5. Component Sketch (Pseudocode/Bootstrap)**

```razor
<!-- Page Header -->
<div class="d-flex justify-content-between align-items-center mb-3">
  <h2><i class="bi bi-clipboard-check me-2"></i> Scrum Ceremony Summary</h2>
  <input type="date" class="form-control w-auto" @bind="SelectedDate" />
</div>

<!-- Panels (responsive row on desktop, stacked on mobile) -->
<div class="d-flex flex-column flex-md-row gap-3">
  <ScrumQuestionPanel
    Title="Yesterday"
    Icon="bi-arrow-left-circle"
    Tooltip="What did you work on yesterday?"
    Entries="@YesterdayEntries"
    OnAdd="()=>OpenAddModal('Yesterday')" />

  <ScrumQuestionPanel
    Title="Today"
    Icon="bi-arrow-right-circle"
    Tooltip="What will you work on today?"
    Entries="@TodayEntries"
    OnAdd="()=>OpenAddModal('Today')" />

  <ScrumQuestionPanel
    Title="Blockers"
    Icon="bi-exclamation-triangle-fill"
    Tooltip="What is blocking your progress?"
    Entries="@BlockerEntries"
    OnAdd="()=>OpenAddModal('Blockers')"
    Highlight="warning" />
</div>
```

---

## **6. WIP Guidance & Usage**

* Treat this as a living doc—update as UI/UX evolves.
* Check off completed tasks, add notes for design tweaks.
* Feed each component/task section into Copilot/AI as a coding prompt.
* Document design decisions (e.g., button placement, stacking breakpoints) as they are finalized.

---

## **7. Open Questions (to finalize as you build)**

* Should “Add” always open a modal, or support inline quick add for “Today”/“Blockers”?
* Should notes/tasks be editable/deletable after saving (for corrections)?
* Is history navigation (see yesterday’s standup, etc) in-scope for v1?
* What color/icon/label scheme works best for blockers (test on dark/light backgrounds)?

---

## **8. Summary**

* Use **3 side-by-side panels on desktop, stacked on mobile**, each focused on one of the three core Scrum questions.
* Key actions are “add” and “edit,” always prominent and touch-friendly.
* Responsive, Bootstrap 5, MAUI Blazor Hybrid-first UI.

---

**Next starting point:**
- Implement edit/delete actions for entries in the panels and modal.
- Improve entry display: show task title, badges, note icon, and actions.
- Wire up task selection to EisenhowerTask data.
- Continue accessibility and responsive polish.

