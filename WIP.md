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

#  Create a self-contained component for displaying next tasks to work on

Now that I have my eisenhower matrix and a basic scrum ceremony, on the "today" tab, I'd like to be able to show "What I plan on working today".  I'd like this to be a component (that is sel-contained), as I'd like to list it on both my "today" tab, as well as my dashboard.  I'm thinking of a simple list of tasks (that can be expanded for details) that I can show.

At a minimum, I need to be able to show items that aren't deleted (DeletedAt == null) and that are not completed (CompletedAt == null).  If it's blocked, I need to be able to show that as we can't work on it until it's cleared.

I need to be able to show the task in order honoring the eisenhower matrix category, the priority under that, and then probably anything that's "overdue".

I want to add a constraint to the ManagementDashboard.Data\Repositories\EisenhowerTaskConstraints.cs file to limit these tasks to only those that are not deleted and not complete.  This can be a "ReadyToWork" constraint that can be used in the repository to filter tasks.

I then need a new service in ManagementDashboard.Core that can be used to retrieve these tasks, and return them in the order that we want them displayed based on priority.  The service could be called TaskService and this method could be called GetNextTasksToWorkOnAsync.  It should return a list of EisenhowerTask objects that are ready to work on. It can take a parameter for the number of tasks to return, defaulting to 5 if not specified.

The priority order for the tasks should be:
1. Honor the Eisenhower Matrix Category (called Quadrant in the code and on the task model) "Do", "Schedule", "Delegate", and "Delete"
    * We will need to establish a numeric order for these categories, e.g., "Do" = 1, "Schedule" = 2, "Delegate" = 3, "Delete" = * You can establish an enum for this in the ManagementDashboard.Core project.
2. Within each category, order by priority (We have a PriorityLevel enum in code we can use)
3. After priority, it would be by blocked status (The task has a property called IsBlocked that can be used for this)
4. After that the CreatedDate (oldest date has higest priority)

I would then like the to have some unit tests for this service in the ManagementDashboard.Tests project.  The tests should cover the following scenarios:
1. Retrieve the correct number of tasks.
2. Retrieve tasks in the correct order.
3. Handle cases where there are no tasks to retrieve.
