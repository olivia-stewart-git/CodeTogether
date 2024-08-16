using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
		// TODO: check that the user actually exists in the database?
		var name = User.Identity?.Name;
		return string.IsNullOrEmpty(name) ? BadRequest() : Json(name);
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
			new (ClaimTypes.Email, user.USR_Email),
			new (ClaimTypes.Name, user.USR_UserName),
			new (ClaimTypes.NameIdentifier, user.USR_PK.ToString()),
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
			RedirectUri = "/",
		};

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(claimsIdentity),
			authProperties);

		return Json(LoginResponseDTO.Success);
	}

	[HttpPost]
	[Route("logout")]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return Json(LoginResponseDTO.Success);
	}
}