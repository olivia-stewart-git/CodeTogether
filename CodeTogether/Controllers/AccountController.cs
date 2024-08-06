using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationState = CodeTogether.Client.Integration.Authentication.RegistrationState;

namespace CodeTogether.Controllers;

[Route("api/account")]
public class AccountController : Controller
{
	readonly IRegistrationService registrationService;
	readonly IRegisterVerificationService verificationService;

	public AccountController(IRegistrationService registrationService, IRegisterVerificationService verificationService)
	{
		this.registrationService = registrationService;
		this.verificationService = verificationService;
	}

	[HttpPost]
	[AllowAnonymous]
	[ValidateAntiForgeryToken]
	[Route("/register")]
	public async Task<IActionResult> Register([FromBody]RegisterAccountDTO registrationRequest)
	{
		RegistrationRequestResponse response;
		if ((response = verificationService.ValidateRequest(registrationRequest)).State == RegistrationState.Success)
		{
			var result = await registrationService.RegisterUser(registrationRequest);
			response = new RegistrationRequestResponse(result, "Successfully created account");
		}

		return Json(response);
	}
}