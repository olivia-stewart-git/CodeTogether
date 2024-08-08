using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Services.Authentication;

public class RegistrationService : IRegistrationService
{
	readonly ICryptographyService cryptographyService;
	readonly ApplicationDbContext dbContext;

	public RegistrationService(ICryptographyService cryptographyService, ApplicationDbContext dbContext)
	{
		this.cryptographyService = cryptographyService;
		this.dbContext = dbContext;
	}

	public Task<RegistrationState> RegisterUser(RegisterAccountDTO registrationRequest)
	{
		var hash = cryptographyService.HashString(registrationRequest.Password, out var salt);
		var user = new UserModel
		{
			USR_Email = registrationRequest.Email,
			USR_UserName = registrationRequest.Username,
			USR_PasswordSalt = salt,
			USR_PasswordHash = hash
		};

		dbContext.Users.Add(user);
		dbContext.SaveChanges();
		return Task.FromResult(RegistrationState.Success);
	}
}