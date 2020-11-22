# DemoApp

## This is DemoApp implemented on ASP.Net Core 2.1 framework.

Using EF Code first ORM.

There is Jobs controller with Index and Complete actions.

Index - retreives all jobs from database and displays list of jobs IDs on the page.
Front end not implemented.

Complete action - is an action to update Job to completed state.
Returns JSON with Success = true/false depends on the result of the action.

### To build and run the app you can use Visual Studio.

1. Download sources
2. Open in VS
3. Restore NuGet packages
5. Build and run

### To publish the app to the production

1. Take files from /Publish folder
2. Create the website in IIS manager
3. Copy files to there
4. Configure connection string in appsettings.json
5. Run

Detailed manual on how to publish .Net Core web app to the IIS you can read here:
[.Net Core web app to the IIS](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-5.0&tabs=visual-studio)
