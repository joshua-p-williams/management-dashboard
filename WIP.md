Analyze the README.md and follow the links within it to the contents within the ./docs/ subfolder reading those markdown documents to get an understanding of the project that we are working on.  Then analalyze the solution itself to get an idea of what has already been built.  Then start looking at the ./docs/tasks.md as we will need to pick back up on the next unfinished tasks.  After you've done all this analysis report back to me if you agree we are in a state to continue.

Also familiarize yourself with the layout of the solution and it's various projects.

ManagementDashboard = MAUI Blazor Hybrid App
ManagementDashboard.Data = Data Access Layer holding all data models repositories and sql migrations
ManagementDashboard.Core = Where to store common business logic and services created in a testable way
ManagementDashboard.Tests = The unit testing project.

I want you to serve the role of a Software Engineer implementing various features for me.

----------------

Now that we have the basic functionality in place, I'd like to add a feature on the home page that gives us quick actions to "Add Task", and "Add Work Capture". This will allow users to quickly create new tasks or work captures without navigating away from the home page.

Steps;

Layout the look and feel of the "Quick Actions" section on the home page.

* [ ] Add a "Quick Actions" section to the home page.
* [ ] Implement a button for "Add Task" that just get's stubbed out initially and has no functionality but does have it's event handler wired up with a simple console log.
* [ ] Implement a button for "Add Work Capture" that just gets stubbed out initially and has no functionality but does have its event handler wired up with a simple console log.

Implement the Add Task.

We currently have this functionality already available on the EisenhowerMatrix.  See it's TaskEditor component usage and be sure to examine the code-behind on it.  We will need to replicate this functionality on the home page.

* [ ] Wire up the functionality to the "Add Task" button so that it opens the TaskEditor component when clicked, with all event handlers now fully wired up to handle the task creation process.  When a new task is added, we would want to refresh our task list on the home page to reflect the new task.

Implement the Add Work Capture.

We currently have this functionality already available in the ScrumSummary.  See it's use of the WorkCaptureNoteModal component and be sure to examine the code-behind on it.  We will need to replicate this functionality on the home page.

* [ ] Wire up the functionality to the "Add Work Capture" button so that it opens the WorkCaptureNoteModal component when clicked, with all event handlers now fully wired up to handle the work capture creation process.