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

I really like what you did on the WorkCaptureCard component when a task is associated with it.

            <div class="d-flex align-items-center mb-2">
                <span class="fw-semibold text-truncate" style="max-width: 60%">
                    <i class="bi bi-list-task me-1"></i>@Truncate(Note.Task.Title, 40)
                </span>
                <button class="btn btn-sm btn-link ms-2" @onclick="ToggleTaskDetails" aria-label="Show Task Details">
                    <i class="bi @(ShowTaskDetails ? "bi-chevron-up" : "bi-chevron-down")"></i>
                </button>
            </div>

I like it so much I'd like to do that on the TaskSummary.razor component as well in place of the current Events & State Accordion.  Make the following changes to the TaskSummary.razor component:

* [ ] Instead of doing an accordion for the Events & State, present the icon followed by the text "Events & State" followed by a chevron icon that toggles the visibility of the events and state, replacing the text "Events & State" and chevron with the actual events and state when expanded.