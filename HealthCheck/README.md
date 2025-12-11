# HealthCheck
Example of using Angular and ASP.NET together. 
- Angular 17
- .NET 8

## Solution Overview
- [healthcheck.client (Angular)](https://github.com/WyrdCoders/angular-aspnet-examples/tree/main/HealthCheck/healthcheck.client)
- [HealthCheck.Server (ASP.NET Web API)](https://github.com/WyrdCoders/angular-aspnet-examples/tree/main/HealthCheck/HealthCheck.Server)

## HealthCheck.Server

#### Dependencies virtual folder
- Contains all the internal, external, and third-party references required to build the project, including all the NuGet packages.

#### Controllers folder
- Define a set of action methods which are called by the routing middleware to handle the requests mapped to them through routing rules.

#### Program.cs
- Application configuration, including the modules and middleware, compilation settings, and publishing rules.
- The main purpose is to create a builder: a factory object that is used to set up and build the interface that will host the web application.
- The host must implement the IHost interface, which exposes a collection of web-related features and services that will be used to handle the HTTP requests.
- Relies on the WebApplicationBuilder class with a built-in implementation of IHostBuilder and IHost.
- Note that starting from .NET 6, Startup.cs is no longer required as the framework introduced a new hosting model that unifies Startup.cs and Program.cs in a single file.
 
## healthcheck.client

### Workspace root files

#### angular.json
The workspace configuration file contains workspace-wide and project-specific configuration defaults for all build and development tools provided by the Angular CLI.
- version: The configuration file version.
- newProjectRoot: The path where new projects are created, relative to the workspace root folder.
- projects: A container item that hosts a sub-section for eaach project in the workspace, containing project-specific configuration options.

#### package.json
The Node Package Manager (npm) configuration file. It contains a list of npm packages to be restored before the project starts.

#### tsconfig.json
The generic TypeScript configuration file.

##### tsconfig.*.json
Project-specific configuration options for various aspects of the app. These options will override those set in the generic tsconfig.json file.
- tsconfig.app.json for application level
- tsconfig.server.json for server level
- tsconfig.spec.json for tests

For additional information, visit [TypeScript configuration](https://angular.io/config/tsconfig).

#### .editorconfig
A workspace-specific configuration for code editors.

#### aspnetcore-https.js
A script that sets up HTTPS for the application using the ASP.NET Core HTTPS certificate.

#### karma.conf.js
An application-specific Karma configuration used to run Jasmine-based tests.

### /src/ folder

#### favicon.ico
A file containing one or more small icons that will be shown in the web browser's address bar, as well as near the page's title in various browser components (tabs, bookmarks, history).

#### index.html
The main HTML page that is served. The CLI automatically adds all JavaScript and CSS files when building the app.

#### main.ts
The main entry point for the application. Compiles the application with the JIT compiler and bootstraps the application's root module (AppModule) to run in the browser.
The AOT compiler can be used by appending the --aot flag to CLI build and serve commands.

#### proxy.conf.ts
The Angular live development server's proxy configuration settings.

#### styles.css
Global CSS or list of CSS files for a project.

### /src/app/ folder

#### AppModule
Angular apps require a root module, conventionally called AppModule, that tells Angular how to assemble the application. 
The root module also contains a reference list of all available components.
Other modules are known as feature modules and serve a different purpose.

#### AppComponent
The only component placed in the /app/ root folder. All other components should be in sub-folders, which will act as a dedicated namespace.

##### app.component.ts
Defines the component logic.

##### app.component.html
Defines the HTML template associated with the AppComponent. Optional but good practice unless the component comes with very minimal UI.

##### app.component.css
Defines the base CSS style sheet for the component. Optional if the component doesn't require UI styling.

##### app.component.spec.ts
Contains the unit tests for the app.component.ts source file.
