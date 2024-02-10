using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // http://.../api/users - name of the class without Controller. // is a GET method by default
public class UsersController : ControllerBase
{
    // make the data context available to rest of class (outside of contstructor)
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context; // note: context instance is scoped to the http request and then destroyed
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        var users = _context.Users.ToList();

        return users; // framework will create OK response automatically for us.
    }

    [HttpGet("{id}")] // /api/users/:id
    public ActionResult<AppUser> GetUser(int id)
    {
        return _context.Users.Find(id);
    }
}
