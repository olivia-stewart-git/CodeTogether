using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;
using CodeTogether.Data.Models.Game;
using CodeTogether.Services.Authentication;
using CodeTogether.TestFramework;
using Moq;

namespace CodeTogether.App.Authentication.Test;

internal class LoginAuthenticationServiceTest
{
	[TestCase("test", "password", true)]
	[TestCase("test", "wrongPassword", false)]
	[TestCase("wrongUsername", "password", false)]
	[TestCase("wrongUsername", "wrongPassword", false)]
	[TestCase("test", "", false)]
	[TestCase("", "password", false)]
	[TestCase("test@email.com", "password", true)]
	public void TestValidAuthenticatedUser(string username, string password, bool expectValid)
	{
		const string validPassword = "password";
		var cryptographyService = new Mock<ICryptographyService>();
		cryptographyService.Setup(x => x.VerifyHash(It.IsAny<string>(), It.IsAny<string>(), validPassword)).Returns(true);

		var mockContext = new Mock<ApplicationDbContext>();
		mockContext.SetupMockDbSet(x => x.Users, new List<UserModel>
		{
			new UserModel()
			{
				USR_Email = "test@email.com",
				USR_UserName = "test",
				USR_PasswordHash = "hash",
				USR_PasswordSalt = "salt"
			}
		});

		var loginRequest = new LoginRequestDTO { Username = username, Password = password };
		var loginService = new LoginAuthenticationService(mockContext.Object, cryptographyService.Object);
		var result = loginService.GetAuthenticatedUser(loginRequest);

		Assert.That(result is not null, Is.EqualTo(expectValid));
	}
}