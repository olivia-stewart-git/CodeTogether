using CodeTogether.Data;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Seeding;
using CodeTogether.Services.Authentication;

namespace CodeTogether.Services.Seeding;

public class UserSeeder : ISeedStep
{
	readonly ApplicationDbContext dbContext;

	public UserSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public int Order => 0;
	public void Seed()
	{
		CreateAdminUser();
	}

	void CreateAdminUser()
	{
		var cryptographyService = new CryptographyService();
		var password = "admin";
		var email = "ct.client.admin@gmail.com";
		var hash = cryptographyService.HashString(password, out var salt);
		var user = new UserModel
		{
			USR_UserName = "admin",
			USR_Email = email,
			USR_PasswordHash = hash,
			USR_PasswordSalt = salt,
			USR_CheckPoints = ["Admin"]
		};

		dbContext.Users.Add(user);
		dbContext.SaveChanges();
	}
}