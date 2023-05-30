using Microsoft.AspNetCore.Mvc;

namespace task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiBase : ControllerBase
{
}