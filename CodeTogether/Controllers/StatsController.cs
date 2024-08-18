using CodeTogether.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers;

[Route("api/stats")]
public class StatsController(ApplicationDbContext DBContext) : Controller
{
	[HttpGet("stats")]
	public IActionResult GetStats()
	{
		return Ok();
	}
}