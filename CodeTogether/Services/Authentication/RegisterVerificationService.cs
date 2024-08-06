using System.Text.RegularExpressions;
using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;

namespace CodeTogether.Services.Authentication;

public interface IRegisterVerificationService
{
	RegistrationRequestResponse ValidateRequest(RegisterAccountDTO registrationRequest);
}

public class RegisterVerificationService : IRegisterVerificationService
{
	readonly ApplicationDbContext dbContext;
	const string AllowedPasswordAndUsernameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	const int MinimumPasswordLength = 8;
	const int MinimumUserNameLength = 8;

	public RegisterVerificationService(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public RegistrationRequestResponse ValidateRequest(RegisterAccountDTO registrationRequest)
	{
		var userNameValidation = ValidateUsername(registrationRequest.Username);
		var passwordValidation = ValidatePassword(registrationRequest.Password);
		var emailValidation = ValidateEmail(registrationRequest.Email);

		return RegistrationRequestResponse.Combined(userNameValidation, passwordValidation, emailValidation);
	}

	RegistrationRequestResponse ValidateUsername(string username)
	{
		if (string.IsNullOrEmpty(username))
		{
			return RegistrationRequestResponse.Invalid("Username cannot be empty");
		}

		if (!AreCharactersAllowed(username, out var invalidCharacter))
		{
			return RegistrationRequestResponse.Invalid($"Make sure username contains valid characters,  {invalidCharacter} is invalid");
		}

		if (username.Length < MinimumUserNameLength)
		{
			return RegistrationRequestResponse.Invalid($"Password must be longer than {MinimumUserNameLength} characters");
		}

		if (IsUsernameTaken(username))
		{
			return RegistrationRequestResponse.Invalid($"Username {username} is taken");
		}

		return RegistrationRequestResponse.Valid("Username is valid");
	}

	bool IsUsernameTaken(string userName)
	{
		return dbContext.Users.Any(x => x.USR_UserName == userName);
	}

	RegistrationRequestResponse ValidatePassword(string password)
	{
		if (string.IsNullOrEmpty(password))
		{
			return RegistrationRequestResponse.Invalid("Password cannot be empty");
		}

		if (!AreCharactersAllowed(password, out var invalidCharacter))
		{
			return RegistrationRequestResponse.Invalid($"Make sure password contains valid characters, {invalidCharacter} is invalid");
		}

		if (password.Length < MinimumPasswordLength)
		{
			return RegistrationRequestResponse.Invalid($"Password must be longer than {MinimumPasswordLength} characters");
		}

		if (!Regex.IsMatch(password, @"[\d\W]"))
		{
			return RegistrationRequestResponse.Invalid("Password should contain at least one number or special character");
		}

		return RegistrationRequestResponse.Valid("Password is valid");
	}

	bool AreCharactersAllowed(string inputString, out char disallowed)
	{
		disallowed = 'a';
		var set = AllowedPasswordAndUsernameCharacters.ToHashSet();
		foreach (var c in inputString)
		{
			if (!set.Contains(c))
			{
				disallowed = c;
				return false;
			}
		}

		return true;
	}

	RegistrationRequestResponse ValidateEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return RegistrationRequestResponse.Invalid("Email cannot be empty");
		}

		if (!email.Contains('@'))
		{
			return RegistrationRequestResponse.Invalid("Enter a valid email");
		}

		if (dbContext.Users.Any(x => x.USR_Email == email))
		{
			return RegistrationRequestResponse.Invalid("There is already an account associated with the email request");
		}

		return RegistrationRequestResponse.Valid("Email is valid");
	}
}