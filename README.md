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
1. BaseEntity implementation as follows 
```
    public class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
```
2. Update entities to extend BaseEntity as follows
```
    public class Food : BaseEntity<long>
    {
        ...
    }

    public class FoodMenuMapping : BaseEntity<long>
    {
        ...
    }

    public class Menu : BaseEntity<long>
    {
        ...
    }
```
3. Add migrations and update database as follows
```
    dotnet ef migrations add BaseEntityMigration
    dotnet ef database update
```
4. Create base Repository and UnitOfWork interfaces at Domain, and concrete implementations at Infrastructure
5. Handle dependency injection at Startup
---
## 8. Service implementations

Use current repositories in service to implement business rules.
```
    public interface IRestaurantService
    {
        Task CreateFood(Food food);
        Task EditFood(Food food);
        Task DeleteFood(Food food);
        IQueryable<Food> ListFood();

        Task CreateMenu(Menu menu);
        Task DeleteMenu(Menu menu);
        IQueryable<Menu> ListMenu();
        Menu GetMenuWithMappings(long menuId);

        IQueryable<Food> ListMenuFoods(IEnumerable<FoodMenuMapping> mapping);
        IQueryable<Food> ListNonMenuFoods(IEnumerable<FoodMenuMapping> mapping);
        IQueryable<Menu> ListFoodMenus(IEnumerable<FoodMenuMapping> mapping);

        Task DeleteFoodMenuMapping(long foodId, long menuId);
        Task DeleteFoodMenuMapping(FoodMenuMapping mapping);
        Task CreateFoodMenuMapping(long foodId, long menuId);
        Task CreateFoodMenuMapping(long foodId, List<long> menuIds);
        Task CreateFoodMenuMapping(List<long> foodIds, long menuId);
        Task UpdateMenuValues(long menuId);
    }
```
---
## 9. API implementations
1. create Dtos under payload project
2. create Adapters under payload project
3. create APIs 
4. refresh swagger generated clients and viewmodels
---
## 10. ngx-bootstrap integration
1. run `npm install bootstrap`
2. run `npm install ngx-bootstrap`
3. add `<link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet">` to index.html
4. import the component is being used so you can register in app.module.ts
```
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import { RatingModule } from 'ngx-bootstrap/rating';
import { SortableModule } from 'ngx-bootstrap/sortable';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';

@NgModule({
    ...
    imports: [
        ...
        AccordionModule.forRoot(),
        AlertModule.forRoot(),
        ButtonsModule.forRoot(),
        CarouselModule.forRoot(),
        CollapseModule.forRoot(),
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        ModalModule.forRoot(),
        PaginationModule.forRoot(),
        PopoverModule.forRoot(),
        ProgressbarModule.forRoot(),
        RatingModule.forRoot(),
        SortableModule.forRoot(),
        TabsModule.forRoot(),
        TimepickerModule.forRoot(),
        TooltipModule.forRoot(),
        TypeaheadModule.forRoot(),
    ],
    ...
})
export class AppModule { }
```

For more information pls visit the [documentation of ngx-bootsrap](https://valor-software.com/ngx-bootstrap/#/documentation)

---
## 11. Restaurant and subcomponen implementations
1. use following commands to create necessary components
    * `ng g component restaurant`
    * `ng g component restaurant/create-food`
    * `ng g component restaurant/create-menu`
    * `ng g component restaurant/edit-food`
    * `ng g component restaurant/edit-menu`
    * `ng g component restaurant/list-food`
    * `ng g component restaurant/list-menu`
2. import `FormsModule` so you can use forms
3. implemented the inside of the components and do necessary api calls

---
## 12. Authentication & Authorization with aspnet identity
1. install `Microsoft.AspNetCore.Identity.EntityFrameworkCore` to EfContext project
2. update db context setup as follows `public class NorthShoreDbContext : IdentityDbContext<IdentityUser>`
3. update startup as follows
```
public class Startup
{
    ...
    public void ConfigureServices(IServiceCollection services)
    {
        ...
            services.AddDbContext<NorthShoreDbContext>(options => {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<NorthShoreDbContext>()
            .AddDefaultTokenProviders();
        ...
    }
    ...
}
```
4. add migration for new db changes and update database, database should look like follows

![picture-005](Pictures/picture-005.jpg)

5. add jwt settings to appsettings.{ENVIRONMENT}.json files as follows
```
  "Jwt": {
    "Key": "SOME_RANDOM_KEY_DO_NOT_SHARE",
    "Issuer": "http://yourdomain.com",
    "ExpireDays": 30,
    "ExpireMinutes": 1
  }
```

6. setup jwt authentication on startup
```
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            ...
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ...
            app.UseCookiePolicy();
            app.UseAuthentication();
            ...
        }
    }
```

7. implement AccountController
8. add attributes to controllers `[AllowAnonymous]` or `[ApiController]`
9. update swagger settings so `Authorization` header can be set as follows
```
public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "North Shore Demo API",
                    Description = "North Shore Demo API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Sinan NAR", Email = "sinan.nar@gmail.com" }
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
            ...
        }
    }
``` 
10. implement `AuthService` in SPA project as singleton DIable
```
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public isAuthenticated = false;
  public token = '';
}
```

11. implement `AuthInterceptor` so api calls can have authorization header
```
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.authService.isAuthenticated) {
      request = request.clone({ setHeaders: { Authorization: `Bearer ${this.authService.token}` } });
    }
    return next.handle(request);
  }
}
```
12. update app.module.ts as follows
```
@NgModule({
    ...
    providers: [
    { provide: api_url, useFactory: getRemoteServiceBaseUrl },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    AuthService
  ],
  bootstrap: [AppComponent]
})
```
13. implement register and login pages so that application can be used
---
