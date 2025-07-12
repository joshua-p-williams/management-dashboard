Analyze the README.md and follow the links within it to the contents within the ./docs/ subfolder reading those markdown documents to get an understanding of the project that we are working on.  Then analalyze the solution itself to get an idea of what has already been built.  Then start looking at the ./docs/tasks.md as we will need to pick back up on the next unfinished tasks.  After you've done all this analysis report back to me if you agree we are in a state to continue.

Also familiarize yourself with the layout of the solution and it's various projects.

ManagementDashboard = MAUI Blazor Hybrid App
ManagementDashboard.Data = Data Access Layer holding all data models repositories and sql migrations
ManagementDashboard.Core = Where to store common business logic and services created in a testable way
ManagementDashboard.Tests = The unit testing project.

I want you to serve the role of a Software Engineer implementing various features for me.

----------------

Refer to the [Scrum Summary Database Definition](feature-scrum-summary-database.md) for the data model and relationships that will inform the UI design. Also refer to the [Scrum Summary Feature Definition](feature-scrum-summary.md) for the feature requirements and user stories.  There is a "UI/UX Design Considerations (To Be Determined)" section in the feature definition that outlines open questions for UI/UX design.  Use this as a starting point to propose a design that addresses these questions and provides a cohesive user experience.

---

# **Build Instructions: Scrum Ceremony Summary (Tabbed UI) Feature**

> **How to use:**
> Work through each task one at a time.
> Each task is broken down into micro-tasks for clarity—check off or comment as you complete, adjust, or revisit.
> If a design decision or blocker arises, note it below the relevant task.

---

## 1. Scaffold the Page and Layout

### 1.1. Create Main Page

Refer to the Eisenhower Matrix for the initial page structure and components as a pattern for how we are building pages using bootstrap 5.  Implement this with code in code-behind files to keep the razor file clean.

* [x] Add new page/component: `ScrumSummary.razor` with route `/scrum-summary`
* [x] Add a page header (e.g. “Scrum Ceremony Summary”)
* [x] Insert a date picker (Bootstrap input, default to today)
* [x] Add a "Work Capture" input field at the top - this will be used to capture work notes for the date specified in the date picker.  This will pop-up a modal to add/edit entries.
* [x] Add a "How does this work?" button just like we see on the EisenhowerMatrix.razor page that opens a modal with an explanation of the Scrum Summary feature.
* [x] Add Bootstrap 5 `nav-tabs` for “Yesterday”, “Today”, and “Blockers”
* [x] For each tab, add a section (panel) for displaying entries related to that question (with a placeholder for now):
  * “What did you work on yesterday?”
  * “What will you work on today?”
  * “What is blocking you?”
* [x] Place an info icon (`bi-info-circle`) next to each tab label or panel header
* [x] Tooltip explains the purpose of each section
* [x] Ensure tab selection updates the visible panel below with custom text and content for each tab 
* [x] Wire appropriate events for handlers for the controls in the code-behind file to handle the date change and work capture input.  For now these can be stubbed out as "not implemented yet" or similar.

Pause for a build (dotnet build), fix any issues and ask for a review before proceeding to the next section.

---

## 2. Display and Manage Entries

### 2.1. Entry List

* [x] Under each panel, display a list of entries (e.g., structured tasks and freeform notes, or blockers under the "Blockers" tab)
  * structured tasks which will be fetched by the date selected as having updates or completion etc.. on the date selection at the top 
  * and/or freeform notes
* [x] For each task entry, show:

  * Title
  * Description (if available)
  * badge/icon (e.g., “Done”, “In Progress”, “Blocked”) - Refer to the Eisenhower Matrix for how to display these

* [x] For each work capture note, show:

  * Note text
  * If associated with a task, fetch it and show the same task entry details as above

**Note**
* Since work capture notes are not required to be linked to tasks, we will allow freeform notes that are not associated with any task.
* For blockers, we can only show the tasks that are blocked (will require a new repository method for fetching these).
* Be sure to show the blocker details in the "Blockers" tab

Pause for a build (dotnet build), fix any issues and ask for a review before proceeding to the next section.


## 3. Wire up all repositories and fetch and populate the data


* [x] Implement repository methods to fetch entries by date (if not already done)
* [x] Populate entry lists in each panel based on the selected date
* [x] Ensure data is reactive and updates on state changes


Pause for a build (dotnet build), fix any issues and ask for a review before proceeding to the next section.

---

## 4. Add/Edit Entry Modal

### 4.1. Modal Implementation

* [ ] Reusable modal component for add/edit

  * Fields: 
    * note textarea
    * structured task selector (optional) - we will need to implement an autocomplete or dropdown to select existing tasks later, stub it out for now
    * DateCreated (auto-filled with current date)
  * Save/cancel buttons
  * Autofocus first field

* [ ] Modal opens when clicking the add from the main Scrum Summary page
* [ ] Ensure all code is in the code-behind file to keep the Razor file clean
* [ ] Wire up to the model and ensure we are persisting to the database
* [ ] On save, add/update entry and refresh the panel list
* [ ] On cancel, close modal with no changes

Pause for a build (dotnet build), fix any issues and ask for a review before proceeding to the next section.

---

## Improvements and Enhancements

I'd like to be able to get a list of events that happened on a Task. For example, if I had a method called "SummarizeEvents", and it takes a date as a parameter, it could look at the date properties on the task (UpdatedAt, CompletedAt, BlockedAt, UnblockedAt, CreatedAt, DeletedAt, etc..), it can give an IEnumerable of descriptions for things that happened on the task for the date given. Example.. If it was Blocked on the date passed in, one of the SummarizedEvents would be "Became blocked", and if it has a blocker reason, it could be included to say "Became blocked - {Blocker Reason)". Make this a new testable service with extension methods for the EisenhowerTasks, but place it in the ManagementDashboard.Core project under it's list of services as appropriate. Maybe a new Folder called Extensions or something? And call it EisenhowerTasksExtensions. Consider whatever best practices are, implement this, and make it tested within the ManagementDashboard.Tests project.

Now that we have the extension methods, we can use them to summarize the events for a task in the Scrum Summary page. This will allow users to see a quick summary of what happened with each task on the selected date.  Can you implement this under each applicable section (Yesterday, Today, Blockers) in the Scrum Summary page?