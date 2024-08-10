using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;
using CodeTogether.Data.Models.Game;
using CodeTogether.Services.Authentication;
using CodeTogether.TestFramework;
using Moq;

namespace CodeTogether.App.Authentication.Test;

internal class RegisterVerificationServiceTest
{
	[TestCaseSource(nameof(TestAllowsValidPasswordSource))]
	public void TestAllowsValidPassword(RegisterAccountDTO request, RegistrationState expectedState)
	{
		var mockContext = new Mock<ApplicationDbContext>();
		mockContext.SetupMock(x => x.Users, new List<UserModel>());

		var verificationService = new RegisterVerificationService(mockContext.Object);

		var result = verificationService.ValidateRequest(request);

		Assert.That(result.State, Is.EqualTo(expectedState));
	}

	[Test]
	public void TestNotAllowedDuplicateEmail()
	{
		var mockContext = new Mock<ApplicationDbContext>();
		mockContext.SetupMock(x => x.Users, new List<UserModel> { new UserModel { USR_Email = "myMail@gmail.com" } });

		var request = new RegisterAccountDTO { Username = "successName", Password = "Password123", Email = "myMail@gmail.com" };
		var verificationService = new RegisterVerificationService(mockContext.Object);

		var result = verificationService.ValidateRequest(request);
		Assert.That(result.State, Is.EqualTo(RegistrationState.Failure));
	}

	//TestAllowsValidPasswordSource
	static IEnumerable<TestCaseData> TestAllowsValidPasswordSource()
	{
		yield return new TestCaseData(new RegisterAccountDTO { Username = "successName", Password = "Password123", Email = "myMail@gmail.com"}, RegistrationState.Success) { TestName = "Valid: username and password" };
		yield return new TestCaseData(new RegisterAccountDTO { Username = "a", Password = "Password123", Email = "myMail@Gmail.com" }, RegistrationState.Failure) { TestName = "Invalid: Username too short" };
		yield return new TestCaseData(new RegisterAccountDTO { Username = "successName", Password = "ad", Email = "myMail@Gmail.com" }, RegistrationState.Failure) { TestName = "Invalid: Password too short" };
		yield return new TestCaseData(new RegisterAccountDTO { Username = "successName", Password = "Password123", Email = "myMaiGmail.com" }, RegistrationState.Failure) { TestName = "Invalid: Email invalid" };
	}
}