# Setup a .NET Project (VS Code)

- Download SDK from https://dotnet.microsoft.com/en-us/download
  - note: you can have multiple versions installed simultaneously
- Download both the RUNTIME and the SDK (left side of screen at https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Full source code (for checking for errors) is [HERE](https://github.com/TryCatchLearn/DatingApp)
- run `dotnet --info` to see installed information
- NOTE: if you need to use an earlier installed version of the SDK you can create a global.json file to specify this.
- Good extension for VS Code: C# devkit
  ms-dotnettools.csdevki

### Useful commands

- `dotnet -h` - help and different commands documentation
- `dotnet new list` - list of templates of projects you can create

## Terms

- SOLUTION: a container for C# .NET projects

## Setup a web api

- `dotnet new sln` to create a solution file for the project
- `dotnet new webapi -n API`
  - in .NET 8 you need to add `--use controllers` after API
- Add the API project into the solution: `dotnet sln add API`
  - run `dotnet sln list` to see the projects in the solution

## Running the API

- cd into the project (`cd API`)
- `dotnet run`
  - listening port is randomly selected - need to update launch settings (see below)
- use `dotnet watch` to get hot reloading

## Launch Settings

- When executing dotnet run, the settings that are used are in `/Properties/launchSettings.json`
- Typically you want to set `launchBrowser` to false
- Swagger is enabled by default: `localhost:{port}/swagger/index.html`
- optionally you can remove `launchUrl` field since we don't want to launch the browser when starting the API
- You can set a standard port for your API: `"applicationUrl": "http://localhost:5000;https://localhost:5001",`
  - run on http and https (5000 and 5001)
- You can optionally remove the other https and IIS Express blocks so you only have http block
  - we just need a single profile to run over the ports specified

### appsettings.json

- Can use configuration info in this file in code at runtime
  - `<project>\appsettings.Development.json`
  - Update logging to information: `"Microsoft.AspNetCore": "Information"` to get more info while developing

### global.json
- If you need to make a project with an older dotnet sdk version or to specify the version explicitly, create a global.json
- `dotnet new globaljson`

### Program.cs

- Entry point to application
- Can add services in the top part
- The bottom part is the HTTP request pipeline
  - Can use middleware to manipulate the http requests and responses here. i.e. `app.UseAuthorization();`

## Troubleshooting

- HTTPS not working (if you go to the port and get a certificate expired error)
  - run `dotnet dev-certs https --trust`
