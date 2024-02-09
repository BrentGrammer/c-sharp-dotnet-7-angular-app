# Publishing to Azure

## Azure Devops

- (https://dev.azure.com/)

## Setup

- Go to (https://visualstudio.microsoft.com/) and click the Get Started For Free under Microsoft Azure
- (email is alternate gmail)

- Repo is at `https://brentonmarquez@dev.azure.com/brentonmarquez/dotnet-practice/_git/dotnet-practice`
  - Repo is private: sign in is alt gmail addr with regular pass, old addr!
- To transfer an existing repo, go to Repos and use the example shown to use the command line. can use:
  - `git remote add azure https://brentonmarquez@dev.azure.com/brentonmarquez/dotnet-practice/_git/dotnet-practice`
  - `git push origin azure`
    - Or you can set the repo to origin and set it as upstream alternatively (this is done here to maintain the code on Github)

## Deploying to Azure Portal

- (https://portal.azure.com/)
- Go to `Create Resource` -> `Web app`
  - **NOTE** Another option is to search marketplace for `Web app + Sql` which comes bundled with a database setup
  - Select a subscription (should have one by default)
  - Name a resource group (just a collection of resources i.e. `DatingAppResourceGroup`)
    - The resource group helps you associate resources with a project
  - Enter a unique name for the url
  - Select publish as Code or Docker Container(if using containerization)
  - Choose the .NET Core runtime stack
  - Select `Linux` for Operating System(can be faster and cheaper on Linux)
  - Select or create an existing Plan
  - **Change Sku and size** - select the free or lowest cost option if desired - a more expensive option is selected by default
    - Select the Dev/Testing Tier box and the free plan
  - Select `Review + Create` button, review your choices and then click `Create`
- When done, you can click the notifications icon in the top right of the screen and select `pin to dashboard` for easy access
- If you visit the url for your project you should get a notice that your service is running and you can now deploy your code.

### Publishing a release

#### In VS Code:

- `dotnet publish -c Release`
  - latest code changes will be put in a `publish` directory
  - Before doing this you need to have built a prod build of your angular app and configured the output of the files to be sent to a folder (i.e. `wwwroot`) in the root of your dotnet project (see commit in repo for changes to make)
  - In SPA project folder, run `ng build --prod` (and make sure you've adjusted the SPA to output build files to a root folder in your dotnet project which is looked at to serve static files)
- Can add the `Azure App Service` management extension by Microsoft to aid in publishing to Azure from VS Code (Also installs Azure Account extension for login)
  - can click on new Azure tab in left panel to sign in to Azure
  - You want to populate the `Files` directory in your project under the subscription/project in the side panel after signing in
    - Click the `Deploy to Web app` icon in the panel (upload symbol), click `Browse...` and navigate to and select the `publish` folder in your project where your files were output with the publish command (will be in a release folder)
    - select the web app to deploy to in the dropdown list and confirm prompt to deploy
    - You can select `Skip for now` in the VS Code prompt unless you want to opt in to it

## Database Setup

- You can remove the Connection string in your production app settings file since it will be in Configuration of Application Settings as an environment variable on Azure
  - (You need to set that connection string up on Azure of course)
- Make sure you change database provider in startup.cs to `x.UseSqlServer(...)`

### Creating a Database Resource on Azure

Note: The server for the database is basically free in Azure, you pay per database on that server

- Go to Azure portal and select `Create Resource`
- Select `SQL Database`
- Then select the same resource group that holds your project
- Name a server domain (must be a unique name) for `Server` field:
  - select the Create New option and fill out the form in the right panel
    - Fill out the user and password for the database
- Configure the Database
  - the default is probably too much for a practice app
  - The elastic pool option is good if you have numerous databases which consume more or less resources at different times. Elastic pool will manage that and allocate more resources to databases that need them automatically.
  - Select the Basic tier which is \$5/mo for testing - note this would also work for a small production app technically
  - Click `Create` (you can pin the completed created resource to your dashboard from the notification in the top right of the screen when it's done)
  - Add your client IP address to allow connection to SQL server
    - Select the database resource created in Azure portal (in your dashboard or find it in list of resources)
    - `Set server firewall..` -> `Add Client IP` (this automatically adds your IP to the whitelist)
    - Toggle to allow Azure Services and Resources to access this server to `Yes` or `On`
    - click `Save`
    - _Note_ These rules are per **server** and not per database, so these rules will apply to all databases on that server

### Adding a connection string to Azure

- Go to your web app resource (not the database) in the portal and to `Configuration` in the left panel
- In the `Application Settings` you can store your secrets/connection strings
  - To find the connection string go to your database resource in Azure Portal and find the `Show connection strings` in the summary page of your database
  - In your web app Application Settings (under Configure still), create a new connection string
    - set the Name field to `DefaultConnection`
    - Paste the copied conn string from your database into the Value field
    - Select AzureSQL as the database type
    - Click `OK`
    - Click `Save` to save changes in the summary view

### Adding Configuration variables/secrets

- Same as Connection string, but add them in the section above it on the same Configuration page
- **If you have nested variables in app.settings.json like AppSettings:Token, when you host on a linux setup you cannot use colons so use a double underscore; `AppSettings__Token` for the key name**

## Troubleshooting

- It may be easiest to just insert the `app.UseDeveloperExceptionPage()` in your prod config in `Startup.cs` to detect if something goes wrong on deployment or staging when testing and comment out any custom exception handling there as well (`app.useEcveptionHandler`).
  - Otherwise, you need to setup Application Insights and get detailed logging setup in Azure
