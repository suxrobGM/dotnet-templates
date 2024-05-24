# .NET Templates for Domain-Driven Design project

This repository hosts a collection of .NET templates designed to facilitate the development of applications using Domain-Driven Design (DDD) principles. The templates cover various aspects of DDD, including CQRS command and query classes, domain event classes, and a starter project for ASP.NET Core Web API applications.

## Available Templates

### Domain-Driven Design Web API Project Template

- **Name:** `Domain-Driven Design Web API Project`
- **Short Name:** `ddd-api`
- **Description:** Offers a comprehensive starting point for building scalable and maintainable ASP.NET Core Web API applications using DDD principles.
- **Usage:** 
```bash
dotnet new ddd-api -n <YourProjectName>
```

### CQRS Command Class Template

- **Name:** `CQRS Command Class`
- **Short Name:** `ddd-cqrs-command`
- **Description:** Streamlines the creation of CQRS command classes, complete with command, handler, and validator for efficient command processing and validation.
- **Usage:**
```bash
dotnet new ddd-cqrs-command -n <YourActionName>
```

### CQRS Query Class Template

- **Name:** `CQRS Query Class`
- **Short Name:** `ddd-cqrs-query`
- **Description:** Simplifies the creation of CQRS query classes, featuring query, handler, and validator for effective query processing and validation.
- **Usage:** 
```bash
dotnet new ddd-cqrs-query -n <YourQueryName>
```

### Domain Event Class Template

- **Name:** `DDD Domain Event Class`
- **Short Name:** `ddd-event`
- **Description:** Facilitates the rapid development of domain events within DDD architectures, including event and handler for seamless integration.
- **Usage:** 
```bash
dotnet new ddd-event -n <YourEventName>
```


## Post-Generation Steps

After generating items or projects with these templates, please ensure to:

1. **Open generated files in the editor:** The templates are configured to automatically open the primary files in your editor for immediate review and modification.
2. **Adjust namespaces:** It's crucial to replace the placeholder namespaces (`Domain`, `Application`, `Shared`) with the actual namespaces specific to your project. This step is essential for the correct integration of the generated classes into your project's structure.

## Installation

### Packing the Templates

To package these templates into a NuGet package, navigate to the root directory of the templates (where the `SISS.Templates.DDD.csproj` file is located) and run:

```bash
dotnet pack
```

This command generates a NuGet package in the ./bin/Release/ directory.

### Installing the Templates
After packing the templates into a NuGet package, install them to your local machine using the following command:

```bash
dotnet new install ./bin/Release/SISS.Templates.DDD.1.0.0.nupkg
```

Make sure to adjust the command with the correct version number if it differs.

## Uninstallation
To uninstall the templates, use the dotnet new uninstall command followed by the package ID, like so:

```bash
dotnet new uninstall SISS.Templates.DDD
```

This will remove the templates from your local machine.
