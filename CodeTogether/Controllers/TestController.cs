using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : Controller
{
	[HttpGet("/test")]
	public IActionResult SomeResult()
	{
		return Ok();
	}
}