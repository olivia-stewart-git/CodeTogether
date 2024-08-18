using CodeTogether.Data;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Seeding;
using CodeTogether.Services.Authentication;

namespace CodeTogether.Services.Seeding;

public class UserSeeder(ApplicationDbContext dbContext) : ISeedStep
{
	public int Order => 0;
	public void Seed(bool initialSeed)
	{
		if (initialSeed)
		{
			CreateAdminUser();
		}
	}

	void CreateAdminUser()
	{
		var cryptographyService = new CryptographyService();
		var password = "sysadminpass";
		var email = "ct.client.admin@gmail.com";
		var hash = cryptographyService.HashString(password, out var salt);
		var user = new UserModel
		{
			USR_UserName = "sysadmin",
			USR_Email = email,
			USR_PasswordHash = hash,
			USR_PasswordSalt = salt,
			USR_CheckPoints = ["Admin"]
		};

		dbContext.Users.Add(user);
		dbContext.SaveChanges();
	}
}