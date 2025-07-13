Analyze the README.md and follow the links within it to the contents within the ./docs/ subfolder reading those markdown documents to get an understanding of the project that we are working on.  Then analalyze the solution itself to get an idea of what has already been built.  Then start looking at the ./docs/tasks.md as we will need to pick back up on the next unfinished tasks.  After you've done all this analysis report back to me if you agree we are in a state to continue.

Also familiarize yourself with the layout of the solution and it's various projects.

ManagementDashboard = MAUI Blazor Hybrid App
ManagementDashboard.Data = Data Access Layer holding all data models repositories and sql migrations
ManagementDashboard.Core = Where to store common business logic and services created in a testable way
ManagementDashboard.Tests = The unit testing project.

I want you to serve the role of a Software Engineer implementing various features for me.

----------------

We will be working on the Scrum Summary feature, which provides a tabbed interface for summarizing Scrum ceremonies. This feature will allow users to capture and view work notes related to their Scrum activities, as well as pulling in details from the Eisenhower Matrix tasks. The UI consists of a date picker, a work capture input field, and three tabs for "Yesterday", "Today", and "Blockers". Each tab displays relevant entries and allow users to add or edit notes.

Refer to the [Scrum Summary Database Definition](feature-scrum-summary-database.md) for the data model and relationships that will inform the UI design. Also refer to the [Scrum Summary Feature Definition](feature-scrum-summary.md) for the feature requirements and user stories.  There is a "UI/UX Design Considerations (To Be Determined)" section in the feature definition that outlines open questions for UI/UX design.  Use this as a starting point to propose a design that addresses these questions and provides a cohesive user experience.

---

# **Build Instructions: Scrum Ceremony Summary (Tabbed UI) Feature**

> **How to use:**
> Work through each task one at a time.
> Each task is broken down into micro-tasks for clarity—check off or comment as you complete, adjust, or revisit.
> If a design decision or blocker arises, note it below the relevant task.

---


I'd like to work on the cosmetics of the scrum summary feature.  Currently it looks clunky.

### **Condensed Critique**

* **Too many borders and nested cards/lists** create visual clutter, making the UI hard to scan and overwhelming.
* **Lack of clear hierarchy:** Notes, tasks, and meta info all look equally important and are not visually grouped together.
* **Notes and tasks feel disconnected:** When a note is linked to a task, it appears as a separate block instead of as part of a single work capture.
* **Overuse of Bootstrap defaults** (list-groups, cards) results in a “samey” look and excess space.
* **Status and blockers don’t stand out:** Important info is buried in noise, not highlighted visually.

**Summary:**
The interface feels cluttered, with information fragmented and hard to follow. Use a single card or block per work capture, combine notes/tasks/meta into one group, and rely more on simple backgrounds or badge bars—less on borders—to clarify hierarchy and flow.

---

## Refactor to use cards for a cleaner, more cohesive UI

**Why Cards?**

* Cards are the most effective modern design pattern for visually grouping related information and actions, especially in productivity and dashboard-style apps.
* Each “thing” (work capture or task) becomes a clear, scannable visual block, making it easier for users to track what’s important, see relationships, and interact with content.
* Cards adapt beautifully to both desktop (side-by-side, grid) and mobile (stacked) layouts using Bootstrap’s responsive utilities.
* **Expandable details** inside cards (e.g., show more task info, work capture meta) keep the UI uncluttered while always giving users access to depth.
* Using cards for both work captures and tasks creates a **consistent, unified UX**—everything important is presented as a card, so users always know what’s actionable and what belongs together.

---

## **Micro-task Checklist for Cards-First Scrum Summary**

We'll implement this in a series of micro-tasks to ensure clarity and focus. Each task will build on the previous one, gradually transforming the UI to a card-based layout.

### **1. Refactor TaskSummary as a Card Component**

* [X] Convert TaskSummary so that each task displays as a standalone Bootstrap card (not a list item).
* [X] Card should show: task title (bold), key meta (status, priority, quadrant, badges) as a horizontal bar or chips.  Be sure not to lose functionality, jsut refactor it's layout.
* [X] Add expandable/collapsible area (accordion/collapse) for task events/state details, only visible when expanded.
* [X] Apply subtle card shadow/background, minimize borders for clean look.
* [X] Ensure all actions (edit, delete, complete) are shown as icon buttons within the card, clearly separated from content.
* [X] Test layout at different screen sizes (cards should stack on mobile).

### **2. Build WorkCaptureCard Component**

* [X] Create a new component (e.g., `WorkCaptureCard.razor`) for individual work captures (notes/entries).  Make sure to use a clean markup only razor component with the code implmentation in the code-behind file.
* [X] Card displays:
  * Note/entry text as main content.
  * If there’s an associated task, show an “expand for task details” button or icon; when expanded, render the TaskSummary card inside (as a nested card, but with reduced visual weight—lighter shadow, less padding).
  * Actions (edit, delete) as icon buttons at top right.  Make sure to use the same event handlers that are already in place.
* [X] Apply matching card style to TaskSummary for consistency.

### **3. Implement Card Components in ScrumSummary.razor**

* [X] In each tab (Yesterday, Today, Blockers), map over work captures and render a `WorkCaptureCard` for each.
* [X] Below work captures, display any standalone task summaries (not tied to a work capture) as TaskSummary cards in a grid or stacked list.
* [X] Remove all legacy list-group/list-item markup and any nested Bootstrap cards/lists inside cards.
* [X] Ensure spacing and layout are clean and visually consistent.

### **4. Polish & Responsive Design**

* [ ] Use Bootstrap grid/flex to layout cards responsively—stacked on mobile, grid on desktop.
* [ ] Test expand/collapse, actions, and keyboard accessibility.
* [ ] Refine spacing, background colors, and shadows for clear card separation without excess borders.
* [ ] Make sure blockers and statuses use visually strong color/indicator on card edge or header.

