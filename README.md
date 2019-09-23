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
## 1. Angular Single Page Application
we will run `ng new NorthShoreSpa` in the root folder<br>
* `ng new NorthShoreSpa`
```
    ? Would you like to add Angular routing? Yes
    ? Which stylesheet format would you like to use? CSS
```

---
## 2. Aspnet Web Api Project
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
## 3. Swagger Configuration for Web Api
Add swashbuckle aspnet core package to Api project
* `dotnet add package Swashbuckle.AspNetCore`

Configure Startup.cs with followings
```
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        // Swagger setup
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new Info
            {
                Version = "v1",
                Title = "North Shore Demo API",
                Description = "North Shore Demo API",
                TermsOfService = "None",
                Contact = new Contact() { Name = "Sinan NAR", Email = "sinan.nar@gmail.com" }
            });
        });
        ...
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client Data V1");
        });
        ...
    }
}
```

visiting `https://localhost:5001/swagger/index.html` should show following
![picture-001](Pictures/picture-001.jpg)

---
## 4. NSwag Setup for Angular Spa Project
Install nswag via npm<br>
* `npm install nswag`

create nswag folder
* `mkdir nswag`

create `service.config.nswag` and `refresh.bat` files.<br>
When you run `refresh.bat` in nswag folder, it will create `\NorthShoreSpa\src\shared\service-proxies.ts` file<br>
`service-proxies.ts` contains view models and services for apis<br>

---
## 5. First api call (ssl cert and cors)
1. change ValuesController route attribute as shown `[Route("api/[controller]/[action]")]`
2. regenerate service proxies
3. import `HttpClientModule` and app.module.ts
4. set base url for service proxies as shown below
```
...
import { environment } from '../environments/environment';
import { API_BASE_URL as api_url } from '../shared/service-proxies';
export function getRemoteServiceBaseUrl(): string {
  return environment.backEndUrl;
}
...
@NgModule({
  ...
  providers: [
    { provide: api_url, useFactory: getRemoteServiceBaseUrl },
  ],
  ...
})
export class AppModule { }
```
5. inject ValuesServiceProxy into `app.component.ts` and do api call at ngOnInit, and show values on `app.component.html`
6. update environment files which are `environment.ts` and `environment.prod.ts`

After all of these, see the CORS problem on network or console tab of chrome dev tools as shown
![picture-002](Pictures/picture-002.jpg)

7. Solve CORS as shown below on Startup.cs
``` 
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddCors(options =>
        {
            options.AddPolicy(_defaultCorsPolicyName, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        ...
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...
        app.UseCors(_defaultCorsPolicyName);
        ...
    }
}
```

After these, if ssl cert problem appears, use dotnet cli to 
![picture-003](Pictures/picture-003.jpg)
```
$ dotnet dev-certs https --clean
$ dotnet dev-certs https --trust
```
---
## 6. Entity Framework integration, creation of entities and first migration

1. Install following packages to EfContext project
    * Microsoft.EntityFrameworkCore
    * Microsoft.EntityFrameworkCore.SqlServer
    * Microsoft.EntityFrameworkCore.Tools
    * Microsoft.Extensions.Configuration.Json
2. Add project dependcy to EfContext, to access Domain dll
3. Create entities under Domain project with Entities namespace
4. Create `NorthShoreDbContext` with DbSets in it with proper relationship
5. Add connection strings to appsettings.{ENVIRONMENT}.json files as follows
```
  "ConnectionStrings": {
    "MsSql": "Server=localhost;Database=NorthShoreRestaurantDb;Trusted_Connection=True;"
  }
```
6. Update Startup class as follows
```
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration["ConnectionStrings:MsSql"];

        services.AddDbContext<NorthShoreDbContext>(options => {
            options.UseSqlServer(connectionString);
        });
        ...
    }
}
```
7. Implement `IDesignTimeDbContextFactory` for `NorthShoreDbContext` as follows (this implementation depends project folder structure)
```
public class NorthShoreDbContextFactory : IDesignTimeDbContextFactory<NorthShoreDbContext>
{
    public NorthShoreDbContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var directoryInfo = new DirectoryInfo(assemblyPath);
        var root = directoryInfo.Parent.Parent.Parent.Parent;
        var webapi = Path.Combine(root.FullName, "NorthShore.Application");

        var conf = new ConfigurationBuilder()
            .SetBasePath(webapi)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true).Build();

        var optionsBuilder = new DbContextOptionsBuilder<NorthShoreDbContext>();
        var connectionString = conf["ConnectionStrings:MsSql"];
        optionsBuilder.UseSqlServer(connectionString);
        return new NorthShoreDbContext(optionsBuilder.Options);
    }
}
```
8. Create initial migration via *dotnet clie* or *Package Manager Console*

*CLI*
```
    dotnet ef migrations add InitialMigration
    dotnet ef database update
```

*Package Manager Console*
```
    Add-Migration InitialMigration
    Update-Database
```

9. After migration created and database updated, you should see the table in database as follows
![picture-004](Pictures/picture-004.jpg)

---
## 7. Repository implementations

---
## 8. Service implementations

---
## 9. API implementations

---
## 10. Bootstrap integration

---
## 11. Restaurant and subcomponen implementations

---
## 12. Authentication & Authorization with aspnet identity

---
