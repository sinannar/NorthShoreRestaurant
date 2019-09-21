# NorthShoreRestaurant
This repository contains the source code for presentation made by Sinan NAR at North Shore user group <br>
You can swithc between branches to see the progression

---

## Content
1. Angular single page application created by `ng new`
2. Api project folder created and required project created by `dotnet`, `.gitignore` setup for projects
3. Swagger configuration for API
4. nswag configuration for SPA project
5. first api call (ssl cert and cors)
6. Entity Framework integration, creation of entities and first migration
7. Repository implementations
8. Service implementations
9. API implementations
10. bootstrap integration
11. Restaurant and subcomponents implemented
12. Authentication & Authorization with aspnet identity

---

## 1 `Angular Single Page Application`
we will run `ng new NorthShoreSpa` in the root folder<br>
* `ng new NorthShoreSpa`
```
    ? Would you like to add Angular routing? Yes
    ? Which stylesheet format would you like to use? CSS
```

---

## 2 `Aspnet Web Api Project`
create a folder `NorthShoreApi` with `mkdir` and get into it with `cd`<br>
* `mkdir NorthShoreApi`

create a solution file named as `NorthShoreApi` with dotnet cli<br> 
* `dotnet new sln -n NorthShoreApi`

create required projects
* `dotnet new webapi -n NorthShore.Application`
* `dotnet new classlib -n NorthShore.Domain`
* `dotnet new classlib -n NorthShore.EfContext`
* `dotnet new classlib -n NorthShore.Infrastructure`
* `dotnet new classlib -n NorthShore.Payload`

add projects to sln
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Application\NorthShore.Application.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Domain\NorthShore.Domain.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.EfContext\NorthShore.EfContext.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Infrastructure\NorthShore.Infrastructure.csproj` 
* `dotnet sln .\NorthShoreApi.sln add .\NorthShore.Payload\NorthShore.Payload.csproj` 

---

