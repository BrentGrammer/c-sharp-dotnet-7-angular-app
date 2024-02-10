// if namespace is API then entire app space has access (everything in API folder)
// typically we have the namespace match the folder name the class is in.
namespace API.Entities;


public class AppUser
{
    // Entity Framewok needs these properties to be public to set in db etc.
    // Note: this is convention - if we have Id in our entity, EF will use that as the primary key
    public int Id { get; set; }

    // since C# 6 strings are assumed to be non-null, now you need to add ? to make it optional
    // you can turn off this flag in API.csproj (project file) and changing <Nullable> flag to disable
    public string UserName { get; set; }
}
