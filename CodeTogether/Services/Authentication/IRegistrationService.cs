using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;

namespace CodeTogether.Services.Authentication;

public interface IRegistrationService
{
	Task<RegistrationState> RegisterUser(RegisterAccountDTO registrationRequest);
}