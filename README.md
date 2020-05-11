# Dependencies Graph Services

[![License](https://img.shields.io/npm/l/@angular/cli.svg)](/LICENSE)
[![.NET Core][github-actions-badge]][github-actions]

Dependencies Graph Services provides Apis to store and retrieve assemblies and these dependencies. behind service, a [Neo4j][neo4j-url] database ensures graph storage.

Analyse can be realized with Dependencies Viewer and sending to Dependencies Graph with a specific export plugin.

##Links repositories
|        Project                         |                Build State                              | 
| -------------------------------------- | :-----------------------------------------------------: | 
| [**Dependencies Viewer**][viewer-url]  |      [![Build][viewer-badge]][viewer-url]               | 
| Export plugin                          |                                                         | 


## Features

### Functional features:

* Import Assembly definitions
* Retrieve assemblies
* Managed partial assembly (assembly not found during analysis)
* Managed Software (assembly with a main method)

### Technical features:

* Transactions
* Swagger to view REST APIs
* Docker

## Packages

This project generates two packages. They are available from the [packages][github-package] page.
* docker container for APIs
* NuGet package for DTO assemblies

### Docker container 

This image is base on Linux. 

You can configure container by setting environment variables.

|   Configuration file  | Environment variable  |          Comment           |   default value     |
| --------------------------------------------- | :----------------------------------------------- |
| ForceHttps            | ForceHttps            | Activate https redirection | false               |
| GraphConfig.Uri       | GraphConfig__Uri      | Uri for neo4j connection   | bolt://localhost    |
| GraphConfig.User      | GraphConfig__User     | neo4j user                 |                     |
| GraphConfig.Password  | GraphConfig__Password | neo4j user password        |                     |

Like all asp.net code applications, you can [customize host configuration][host-configuration-ms].

You can start a Dependencies Graph Service container like this:

```
docker run \
    --publish=5001:80 \
    dependencies-graph-api:tag
```
When service is running, you can go to swagger page pour explore services

<img src="doc/images/swagger.png"/>

If you need force https on service, you can use the following command sample:

```
docker run \
     -p 5000:80 -p 5001:443 \
     -e ForceHttps=true
     -e ASPNETCORE_Kestrel__Certificates__Default__Password="mycertificatepassword" \
     -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/mycertificatename.pfx \ 
     -v %USERPROFILE%\.aspnet\https:/https/ \
     dependencies-graph-api:tag
```

### Nuget package

This NuGet contains an assembly with all Data transfer objects (DTO). These classes are Plain Old C# Object (POCO) and no other dependencies.
This package support Net Standard 2.1.

You can install nuget package like this:

```
dotnet add package Dependencies.Graph.Dtos
```

## Development

This project has tooling for Visual Studio and Visual Studio Code.

### Visual Studio Code

The project is configured to work with the [Remote Development][remote-development-plugin-url] plugin. With VS Code, you can open the workspace directory in a container (from mcr.microsoft.com/dotnet/core/sdk:3.1) and work inside.

VS Code launch Two containers when you open workplace
* Development container
* neo4j Container

Some plugins are automatically installed on VS Code for the development session. Now plugins added are:
* C# (ms-dotnettools.csharp)
* Debugger for Chrome (msjsdiag.debugger-for-chrome)

You can run application with following code (in VS Code Terminal):

```
/workspace# dotnet restore
/workspace# dotnet run --project Dependencies.Graph.Api/Dependencies.Graph.Api.csproj
```

After build, you can open a navigator in your local computer and navigate to
```
http://localhost:5000
```

### Visual Studio 

The solution contains a docker-compose project to run from visual studio:
* Container with the software compiled for the container (Linux)
* neo4j container

To start a debugging session, define docker-compose project as the startup project for the solution and start Visual Studio debugger. A navigator should be launch with the application start page.

## Database schema

In a graph database, the schema is defined by node (and labels associated) and relations between nodes

### Node

For this project we have one node for a specif version of assembly. The assembly full name (ex: *Dependencies.Viewer.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null*) is used as node unique key.

### Relation

* Reference: represents a reference between two assemblies

### Labels

* Assembly: represents an assembly version (assembly full name is used as a key)
* Software: Additional label for assembly with a main method
* Partial: an additional label for assembly not found during dependencies analyses (missing assembly or another version is used when program use assembly)

<img src="doc/images/database-schema.png"/>

[github-actions]:                   https://github.com/xclemence/Dependencies-graph-services/actions
[github-actions-badge]:             https://github.com/xclemence/Dependencies-graph-services/workflows/Build/badge.svg?branch=master
[github-package]:                   https://github.com/xclemence/dependencies-graph-services/packages

[viewer-badge]:                     https://github.com/xclemence/Dependencies.Viewer/workflows/Ms%20Build/badge.svg
[viewer-url]:                       https://github.com/xclemence/Dependencies.Viewer

[neo4j-url]:                        https://neo4j.com/
[remote-development-plugin-url]:    https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack 
[host-configuration-ms]:            https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1#host-configuration