# Dependencies Graph Services
[![License](https://img.shields.io/npm/l/@angular/cli.svg)](/LICENSE)
[![.NET Core][github-actions-badge]][github-actions]

Dependencies Graph Services provides Apis to store and retrieve assemblies and these dependencies. behind service, a [Neo4j](https://neo4j.com/) database ensures graph storage.

Analyse can be realized with Dependencies Viewer and sending to Dependencies Graph with spécif export plugin.

##Links repositories
|                             |                Build State                              | 
| --------------------------- | :-----------------------------------------------------: | 
| **Depencencies Viewer**     |      [![Build][viewer-badge]][viewer-url]               | 
| **Export plugin**           |                                                         | 


## Features
### Functional features:
- Assemblies import
- Retrieve assemblies
- Managed partial assembly (assembly not found during analysis)
- Managed Sofware (assembly with a main method)

### Technical features:
- Transactions
- Swagger to view REST APIs
- Docker

## Package
This project generates two packages. They are available from the package page.
- docker container for APIs
- NuGet package for DTOs assembly


### Docker container 
this image is base on Linux. 

You can start a Dependencies Graph Service container like this:

```
docker run \
    --publish=5001:80 \
    dependencies-graph-api:tag
```
When service is running, you can go to swagger page pour explore services

// TODO Screnshot

### Nuget package
This NuGet contains an assembly with all Data transfer objects (DTO). These classes are Plain Old C# Object (POCO) and no other dependencies.
This package support Net Standard 2.1.

You can install nuget package like this:

```
dotnet add package Dependencies.Graph.Dtos
```

## Database schema
The database schema is very simple.

### Nodes
- Assembly: represents an assembly version (assembly full name is used as a key)
- Software: Additional label for assembly with a main method
- Partial: an additional label for assembly not found during dependencies analyses (missing assembly or another version is used when program use assembly)
### Relation
- Reference: represents a reference between two assemblies

// TODO Diagram

## Development
This project has tooling for Visual Studio and Visual Studio Code.

### Visual Studio Code
The project is configured to work with the Remote Development plugin (//todo add link). With VS Code, you can open the workspace directory in a container (from mcr.microsoft.com/dotnet/core/sdk:3.1) and work inside.

VS Code launch Two containers when you open workplace
- Development container
- neo4j Container

Some plugins are automatically installed on VS Code for the development session. Now plugins added are:
- C# (ms-dotnettools.csharp)
- Debugger for Chrome (msjsdiag.debugger-for-chrome)

You can run application with following code (in VS Code Terminal):

```
:/workspace# dotnet restore
:/workspace# dotnet run --project Dependencies.Graph.Api/Dependencies.Graph.Api.csproj
```

After build, you can open a navigator in your local computer and navigate to
```
http://localhost:5000
```

### Visual Studio 


[github-actions]:               https://github.com/xclemence/Dependencies.Graph.Services/actions
[github-actions-badge]:         https://github.com/xclemence/Dependencies.Graph.Services/workflows/Build/badge.svg?branch=master

[viewer-badge]:                 https://github.com/xclemence/Dependencies.Viewer/workflows/Ms%20Build/badge.svg
[viewer-url]:                   https://github.com/xclemence/Dependencies.Viewer
