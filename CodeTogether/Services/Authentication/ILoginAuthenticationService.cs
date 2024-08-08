using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Services.Authentication;

public interface ILoginAuthenticationService
{
	UserModel? GetAuthenticatedUser(LoginRequestDTO loginRequest);
}