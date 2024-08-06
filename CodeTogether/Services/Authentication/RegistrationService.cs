using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;

namespace CodeTogether.Services.Authentication;

public interface IRegistrationService
{
	Task<RegistrationState> RegisterUser(RegisterAccountDTO registrationRequest);
}

public class RegistrationService : IRegistrationService
{
	readonly ICryptographyService cryptographyService;

	public RegistrationService(ICryptographyService cryptographyService)
	{
		this.cryptographyService = cryptographyService;
	}

	public Task<RegistrationState> RegisterUser(RegisterAccountDTO registrationRequest)
	{
		return Task.FromResult(RegistrationState.Success);
	}
}