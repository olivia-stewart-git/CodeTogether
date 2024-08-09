using CodeTogether.Client.Integration.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeTogether.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CodeTogether.Controllers;

[Route("api/account")]
public class LoginController : Controller
{
	readonly ILoginAuthenticationService loginAuthenticationService;

	public LoginController(ILoginAuthenticationService loginAuthenticationService)
	{
		this.loginAuthenticationService = loginAuthenticationService;
	}

	[HttpGet]
	[Route("user")]
	public IActionResult GetUser()
	{
		var name = User.Identity?.Name;
		return string.IsNullOrEmpty(name) ? NotFound() : Json(name);
	}

	[HttpPost]
	[Route("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
	{
		var user = loginAuthenticationService.GetAuthenticatedUser(loginRequest);
		if (user == null)
		{
			return Json(LoginResponseDTO.Failed);
		}

		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, user.USR_Email),
			new ("UserName", user.USR_UserName),
		};

		foreach (var userUserCheckPoint in user.USR_CheckPoints)
		{
			claims.Add(new Claim(ClaimTypes.Role, userUserCheckPoint));
		}

		var claimsIdentity = new ClaimsIdentity(
			claims, CookieAuthenticationDefaults.AuthenticationScheme);

		var authProperties = new AuthenticationProperties
		{
			IsPersistent = true,
			ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
			RedirectUri = "/login",
		};

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(claimsIdentity),
			authProperties);

		return Json(LoginResponseDTO.Success);
	}
}