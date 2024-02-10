# Entity Framework

- ORM that translates code into SQL commands that update the tables in the database
- Gives us Concurrency handling - uses optimistic concurrency by default
- First level caching out of the box (repeated querying returns from cache instead of network)
- Convention first framework

### Sqlite
- Note Sqlite is good for development - it is just a in memory db that is a file on your system (no engine extra software etc.).
- Not suitable for production
- To view db in VS Code install Sqlite extension: alexcvzz.vscode-sqlite
  - Ctrl-Shift-P to open command pallette > SQLite: Open Database
  - Select your db and then you'll then see a new SQLITE EXPLORER Section in the left side tab
## Definitions

- Navigation Prop - Any property in a model that is of type or linked to another model/entity
- DbContext - abstraction used to save and load data to the database
  - acts as bridge between our domain and database
  - We write Linq queries (`DbContext.Users.Add(new User {id = 4, Name = John})` > Database Provider (i.e. Sqlite postgres etc) > `INSERT INTO Users (Id, Name) VALUES (4,John)`)

## Installing Entity Framework to a Project

- In VS Code add the extension: `NuGet Gallery` for managing nuget packages
- Show all commands pallette (Ctrl-Shift-P) > Open Nuget Gallery
- Install package for the db you are using (we use Sqlite in development for ex.): `Microsoft.EntityFrameworkCore.Sqlite` package (make sure the version matches your dotnet version) - Note: do NOT install ...Sqlite.Core package of same name.
- Install `Microsoft.EntityFrameworkCore.Design` package to enable code first migrations
- Make sure you see the packages in `API.csproj`:

```xml
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.15">
      ...
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="7.0.15" />
```

### Add the dotnet-ef cli tool to run migrations

- https://www.nuget.org/packages/dotnet-ef/#supportedframeworks-body-tab
  - `dotnet tool install --global dotnet-ef --version 7.0.15`

## Add a new Entity (table)

1. Create the model class

```c#
using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
  }
}
```

1. Create the DbSet in the Data Context file
1. Optionally Configure relationships with the builder in the Data Context file

```c#
namespace DatingApp.API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      // configure relationships if you need to
      builder.Entity<Like>()
        .HasKey(k => new { k.LikerId, k.LikeeId });
      builder.Entity<Message>()
        .HasOne(u => u.Sender)
        .WithMany(m => m.MessagesSent)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
```

### Migrations

(needs dotnet-ef cli tool installed)

1. Run migration script and apply changes to the database

- Initial Migration: `dotnet ef migrations add InitialCreate -o Data/Migrations`
  - Make sure you are in the folder of your project (i.e. /API)
  - `-o` means where you want the migrations to go, what folder.
- `dotnet ef migrations add MyMigrationName`
- `dotnet ef database update` - run the migration
- NOTE: if you get errors you might want to run `dotnet build` to get more info - as part of the migrations, it builds your app and if there are errors you need to run it to get more info on what went wrong.

1. Start web server back up if in development

### Lazy Loading

- Allows you to load navigation properties only when EF needs them.
  - Can be used if you are including properties on an entity for later use, but you get a warning about how those properties are not being used and the include is not requried. (i.e. in `PagedList.cs` where you get the `CountAsync()` for messages(the source parameter))
  - Implementing lazy loading will enable EF to know what it needs to load to make a count call on an entity or if it converts it to a list what to include (i.e. nav props from related models) automatically
- Install the package `Microsoft.EntityFrameworkCore.Proxies`
  - Select the version that matches your EF runtime
- Add a config option to your DbContext to use lazy loading proxies in both development and prod:

```c#
 public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(x =>
      {
        // LAZY LOADING to address warnings about unnecessary include usage with navprops from the CountAsync() call in PagedList.cs (because at that time it executes the IQueryable before the included Photos are used or returned)
        // Now in the dating repository, you can remove all include statements! EF will know what it needs to use and include automatically
        x.UseLazyLoadingProxies();
        x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")); // comes from appsettings.json files (appsettings.Development.json when in Development mode)
      });

      ConfigureServices(services);
    }

    public void ConfigureProductionServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(x =>
      {
       // Now in the dating repository, you can remove all include statements! EF will know what it needs to use and include automatically
        x.UseLazyLoadingProxies();
        x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

      });

      ConfigureServices(services);
    }
```

- You now remove all `include()` statements in your repository or anywhere else in the codebase - EF Core will automagically know what to include somehow
- Now in the models that use navigation props (any prop that references / is type of another model/entity), make all of those properties `virtual` (for ex in the User model):

```c#
    public virtual ICollection<Photo> Photos { get; set; }
    public virtual ICollection<Like> Likers { get; set; }
    // etc for all nav props
```
