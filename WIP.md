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


I want to make improvements to the WorkCaptureCard —using a hamburger (ellipsis) menu for actions keeps your card header clean, while truncating both note and task titles maintains a compact, scannable summary. Showing the full note in the body and revealing task details only when the user requests them balances clarity and depth, improving both desktop and mobile experience.

**In summary:**
You’re presenting only the most important info at a glance—short note (truncated in the header), a tidy action menu, and (if present) a task title preview. Users can choose to expand for the full note or to reveal all task details via the “show more” icon. This keeps the UI uncluttered and highly readable, without hiding important actions.

---

## **Micro-task Checklist for This Approach**

* [ ] Truncate the card header note to the first 40 characters (ending with “…” if longer).
* [ ] Move the full note text to the card body—displayed in full beneath the header.
* [ ] Place a right-aligned ellipsis (hamburger) menu in the card header for all actions (Edit, Delete, etc.). Just like we do on the TaskCard for the Eisenhower Matrix.

  * [ ] Ensure this is accessible and touch-friendly.
* [ ] If the note has an associated task:

  * [ ] Display the **task title** (truncated to 40 characters with ellipsis if needed, just like we did on the header for the work capture in the first task) in the card body, just below the note, with a small icon/button (chevron or info) to “Show Task Details.”
  * [ ] When the user clicks this icon, expand to show the full **TaskSummary** control in place of the preview title.
  * [ ] Collapse again to return to just the task title preview.
* [ ] Style the preview/expand icon to be visually distinct but not dominant.
* [ ] Test how this behaves with and without associated tasks, on desktop and mobile.
* [ ] Check keyboard and screen reader accessibility for the hamburger menu and expand/collapse controls.

---

**This pattern keeps your Scrum Summary UI concise, powerful, and user-friendly—surfacing key info and actions, while hiding detail until it’s needed.**
Let me know if you’d like a diagram or want to move to implementation tips for any step!

