using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

/**
* Class used to bridge between domain and Database.
  Gives us a session with out database.
  You need to register this in Program.cs to tell the app about this.
*/
// inherit from DbContext from EF core
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // DbSet establishes an entity object to run db operations
    // pass in the entity class you created as the generic
    public DbSet<AppUser> Users { get; set; }
}
