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

## NextTasksList Feature

> **NextTasksList Component — Design & Usage**
>
> The `NextTasksList` component is a self-contained, reusable UI element that displays a prioritized list of actionable tasks for the user, based on the Eisenhower Matrix and task priority. Designed for MAUI/Blazor Hybrid with Bootstrap 5, it injects its own repositories/services and manages state internally. The component fetches tasks from `TaskService.GetNextTasksToWorkOnAsync`, supports both a “Top N” and “All” view (user-selectable via dropdown), and displays each task using the existing `TaskCard` component. Intended for easy inclusion on any dashboard or feature page, it offers a quick, scannable overview of what’s next, with responsive design and accessibility as first-class goals.

---

## **UI/UX Design Considerations (Best Practices)**

* **Self-contained**: Handles its own data fetch/state, so it’s easy to drop into any layout.
* **Dropdown or Toggle for Item Count**: Place a Bootstrap dropdown or segmented toggle above the list for user to select “Top 5”, “Top 10”, or “All” tasks. Default to a reasonable number (e.g., 5 or 10).
* **Task Cards**: Render tasks using the standard `TaskCard` for consistent look and feature set.
* **Responsive Grid/List**: On desktop, display as a grid or stacked cards with margin; on mobile, cards should stack vertically and remain easy to tap.
* **Loading State**: Show a Bootstrap spinner or skeleton loader while fetching.
* **Empty State**: Friendly message (“You’re all caught up!”) if no tasks are returned.
* **Blocked Tasks**: Visually mark blocked tasks with a badge/icon and fade/disable actions as needed.
* **Header/Title**: Clear, concise section title (e.g., “Next Tasks to Work On”).
* **Accessibility**: All dropdowns/buttons are keyboard-accessible and labeled; ensure screen reader compatibility.

---

## **Micro-task Checklist**

1. **Component Setup**

   * [ ] Scaffold new Razor component: `NextTasksList.razor`
   * [ ] Inject required services (`TaskService` etc.) and manage loading/error state locally

2. **Dropdown for Item Count**

   * [ ] Add Bootstrap 5 dropdown/select or segmented toggle for number of tasks to display (“Top 5”, “Top 10”, “All”)
   * [ ] Wire dropdown to trigger reload of task list

3. **Data Fetching**

   * [ ] On parameter or dropdown change, fetch tasks using `GetNextTasksToWorkOnAsync(n)`
   * [ ] Show spinner/loading indicator during fetch
   * [ ] Handle error and empty states gracefully

4. **Task Rendering**

   * [ ] Render tasks using `<TaskCard Task=... />`, passing all needed props
   * [ ] Stack in a Bootstrap grid or vertical list, responsive for all screen sizes
   * [ ] Visually mark blocked tasks (badge, icon, faded look)
   * [ ] Optionally, expand/collapse for more task details

5. **UI Polish**

   * [ ] Add component header/title (“Next Tasks to Work On”)
   * [ ] Apply Bootstrap spacing/margins for clean appearance
   * [ ] Make sure all buttons/controls have tooltips and aria-labels

6. **Testing**

   * [ ] Confirm usability and layout on both desktop and mobile
   * [ ] Test keyboard navigation and screen reader experience

