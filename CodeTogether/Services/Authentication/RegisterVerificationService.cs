using System.Text.RegularExpressions;
using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using CodeTogether.Data;

namespace CodeTogether.Services.Authentication;

public interface IRegisterVerificationService
{
	RegistrationRequestResponseDTO ValidateRequest(RegisterAccountDTO registrationRequest);
}

public class RegisterVerificationService : IRegisterVerificationService
{
	readonly ApplicationDbContext dbContext;
	const string AllowedPasswordAndUsernameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	const int MinimumPasswordLength = 8;
	const int MinimumUserNameLength = 5;

	public RegisterVerificationService(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public RegistrationRequestResponseDTO ValidateRequest(RegisterAccountDTO registrationRequest)
	{
		var userNameValidation = ValidateUsername(registrationRequest.Username);
		var passwordValidation = ValidatePassword(registrationRequest.Password);
		var emailValidation = ValidateEmail(registrationRequest.Email);

		return RegistrationRequestResponseDTO.Combined(userNameValidation, passwordValidation, emailValidation);
	}

	RegistrationRequestResponseDTO ValidateUsername(string username)
	{
		if (string.IsNullOrEmpty(username))
		{
			return RegistrationRequestResponseDTO.Invalid("Username cannot be empty");
		}

		if (!AreCharactersAllowed(username, out var invalidCharacter))
		{
			return RegistrationRequestResponseDTO.Invalid($"Make sure username contains valid characters,  {invalidCharacter} is invalid");
		}

		if (username.Length < MinimumUserNameLength)
		{
			return RegistrationRequestResponseDTO.Invalid($"Password must be longer than {MinimumUserNameLength} characters");
		}

		if (IsUsernameTaken(username))
		{
			return RegistrationRequestResponseDTO.Invalid($"Username {username} is taken");
		}

		return RegistrationRequestResponseDTO.Valid("Username is valid");
	}

	bool IsUsernameTaken(string userName)
	{
		return dbContext.Users.Any(x => x.USR_UserName == userName);
	}

	RegistrationRequestResponseDTO ValidatePassword(string password)
	{
		if (string.IsNullOrEmpty(password))
		{
			return RegistrationRequestResponseDTO.Invalid("Password cannot be empty");
		}

		if (!AreCharactersAllowed(password, out var invalidCharacter))
		{
			return RegistrationRequestResponseDTO.Invalid($"Make sure password contains valid characters, {invalidCharacter} is invalid");
		}

		if (password.Length < MinimumPasswordLength)
		{
			return RegistrationRequestResponseDTO.Invalid($"Password must be longer than {MinimumPasswordLength} characters");
		}

		if (!Regex.IsMatch(password, @"[\d\W]"))
		{
			return RegistrationRequestResponseDTO.Invalid("Password should contain at least one number or special character");
		}

		return RegistrationRequestResponseDTO.Valid("Password is valid");
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

	RegistrationRequestResponseDTO ValidateEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return RegistrationRequestResponseDTO.Invalid("Email cannot be empty");
		}

		if (!email.Contains('@'))
		{
			return RegistrationRequestResponseDTO.Invalid("Enter a valid email");
		}

		if (dbContext.Users.Any(x => x.USR_Email == email))
		{
			return RegistrationRequestResponseDTO.Invalid("There is already an account associated with the email request");
		}

		return RegistrationRequestResponseDTO.Valid("Email is valid");
	}
}