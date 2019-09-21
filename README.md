# NorthShoreRestaurant
This repository contains the source code for presentation made by Sinan NAR at North Shore user group <br>
You can swithc between branches to see the progression

## Content
1. Angular single page application created by `ng new`
2. Api project folder created and required project created by `dotnet`
3. `.gitignore` setup for projects
4. Swagger configuration for API
5. nswag configuration for SPA project
6. first api call (ssl cert and cors)
7. Entity Framework integration, creation of entities and first migration
8. Repository implementations
9. Service implementations
10. API implementations
11. bootstrap integration
12. Restaurant and subcomponents implemented
13. Authentication & Authorization with aspnet identity

## 1 `Angular Single Page Application`
we will run `ng new NorthShoreSpa` in the root folder
```
    ? Would you like to add Angular routing? Yes
    ? Which stylesheet format would you like to use? CSS
```

## Cheatsheet
* `ng new NorthShoreSpa`
* `mkdir NorthShoreApi`
* `dotnet new sln -n NorthShoreApi`
* `dotnet new webapi -n NorthShore.Application`
* `dotnet new classlib -n NorthShore.Domain`
* `dotnet new classlib -n NorthShore.EfContext`
* `dotnet new classlib -n NorthShore.Infrastructure`
* `dotnet new classlib -n NorthShore.Payload`
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Application\NorthShore.Application.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Domain\NorthShore.Domain.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.EfContext\NorthShore.EfContext.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Infrastructure\NorthShore.Infrastructure.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Payload\NorthShore.Payload.csproj` 

