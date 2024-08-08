using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;
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
	readonly ApplicationDbContext dbContext;

	public AccountController(IRegistrationService registrationService, IRegisterVerificationService verificationService, ApplicationDbContext dbContext)
	{
		this.registrationService = registrationService;
		this.verificationService = verificationService;
		this.dbContext = dbContext;
	}

	[HttpPost]
	[Route("register")]
	public async Task<IActionResult> Register([FromBody]RegisterAccountDTO registrationRequest)
	{
		RegistrationRequestResponseDTO responseDto;
		if ((responseDto = verificationService.ValidateRequest(registrationRequest)).State == RegistrationState.Success)
		{
			var result = await registrationService.RegisterUser(registrationRequest);
			responseDto = new RegistrationRequestResponseDTO(result, "Successfully created account");
		}

		return Json(responseDto);
	}
}