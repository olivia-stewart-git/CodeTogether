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
	const bool StrictValidation = false;
	readonly ApplicationDbContext dbContext;
	const int MinimumPasswordLength = 4;
	const int MinimumUserNameLength = 3;

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

		if (!AreCharactersAllowed(username, out var invalidCharacters))
		{
			return RegistrationRequestResponseDTO.Invalid($"Make sure username contains valid characters,  '{string.Join("', '", invalidCharacters)}' are invalid");
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

		if (password.Length < MinimumPasswordLength)
		{
			return RegistrationRequestResponseDTO.Invalid($"Password must be longer than {MinimumPasswordLength} characters");
		}

		if (StrictValidation && !Regex.IsMatch(password, @"[\d\W]"))
		{
			return RegistrationRequestResponseDTO.Invalid("Password should contain at least one number or special character");
		}

		return RegistrationRequestResponseDTO.Valid("Password is valid");
	}

	bool AreCharactersAllowed(string inputString, out IEnumerable<char> disallowed)
	{
		disallowed = inputString.Where(ch => !(Char.IsLetterOrDigit(ch) || Char.IsPunctuation(ch) || Char.IsSymbol(ch)));
		return !disallowed.Any();
	}

	RegistrationRequestResponseDTO ValidateEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return RegistrationRequestResponseDTO.Invalid("Email cannot be empty");
		}

		if (!email.Contains('@'))
		{
			return RegistrationRequestResponseDTO.Invalid("Email must contain an @");
		}

		if (dbContext.Users.Any(x => x.USR_Email == email))
		{
			return RegistrationRequestResponseDTO.Invalid("There is already an account associated with the email request");
		}

		return RegistrationRequestResponseDTO.Valid("Email is valid");
	}
}