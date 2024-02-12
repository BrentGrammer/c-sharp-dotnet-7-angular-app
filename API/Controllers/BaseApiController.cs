using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/**
* To help DRY code, we create a Base controller with boilerplate (the decorators, etc.)
*/

[ApiController]
[Route("api/[controller]")]  // http://.../api/className - name of the class without "Controller". // is a GET method by default
public class BaseApiController : ControllerBase
{

}
