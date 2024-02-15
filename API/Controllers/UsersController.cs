using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize] // protects all routes unless overriden in controller
public class UsersController : BaseApiController
{
    // make the data context available to rest of class (outside of contstructor)
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context; // note: context instance is scoped to the http request and then destroyed
    }

    // HTTP method handlers should always be async and return a Task
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users; // framework will create OK response automatically for us.
    }

    [HttpGet("{id}")] // /api/users/:id
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}
