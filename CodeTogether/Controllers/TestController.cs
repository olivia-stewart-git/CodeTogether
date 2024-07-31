using CodeTogether.Common.Logging;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers;

[ApiController]
[Route("api")]
public class ProductsController : Controller
{
	readonly ILoggerManager logManager;

	public ProductsController(ILoggerManager logManager)
	{
		this.logManager = logManager;
	}

	[HttpGet("/test")]
	public IActionResult SomeResult()
	{
		logManager.LogInfo("Something happened");
		return Ok();
	}
}