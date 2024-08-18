using CodeTogether.Client.Integration;
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
		var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
		if (nameClaim == null)
		{
			return BadRequest();
		}
		if (!Guid.TryParse(nameClaim.Value, out var userId))
		{
			return BadRequest();
		}
		return string.IsNullOrEmpty(name) ? BadRequest() : Json(new UserInfoDTO { Name = name, Id = userId });
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
			//RedirectUri = "/", //do not redirect if messes things up!1
			// TODO: work out how to do token refreshes
		};

		try
		{
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);
		}
		catch (Exception e)
		{
			return Json(new LoginResponseDTO() { IsAuthenticated = false, Message = $"Something went wrong attempting to sign in {e.Message}"});
		}

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