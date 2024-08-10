namespace CodeTogether.Client.Services
{
	public abstract class AccountException : Exception
	{
		public AccountException(string message) : base(message) { }
	}

	public class LoginFailedException : AccountException
	{
		public LoginFailedException(string message) : base(message) { }
	}

	public class RegistrationFailedException : AccountException
	{
		public RegistrationFailedException(string message) : base(message) { }
	}
}
