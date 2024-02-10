namespace API;

public class AppUser
{
    // Entity Framewok needs these properties to be public to set in db etc.
    public int Id { get; set; }

    // since C# 6 strings are assumed to be non-null, now you need to add ? to make it optional
    // you can turn off this flag in API.csproj (project file) and changing <Nullable> flag to disable
    public string UserName { get; set; }
}
