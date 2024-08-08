using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Services.Authentication;

public class LoginAuthenticationService : ILoginAuthenticationService
{
	readonly ApplicationDbContext dbContext;
	readonly ICryptographyService cryptographyService;

	public LoginAuthenticationService(ApplicationDbContext dbContext, ICryptographyService cryptographyService)
	{
		this.dbContext = dbContext;
		this.cryptographyService = cryptographyService;
	}

	public UserModel? GetAuthenticatedUser(LoginRequestDTO loginRequest)
	{
		if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
		{
			return null;
		}
		var matchingPotentialUsers = dbContext.Users.Where(u => u.USR_Email == loginRequest.Username || u.USR_UserName == loginRequest.Username).ToList();
		return matchingPotentialUsers.FirstOrDefault(u => cryptographyService.VerifyHash(u.USR_PasswordSalt, u.USR_PasswordHash, loginRequest.Password));
	}
}