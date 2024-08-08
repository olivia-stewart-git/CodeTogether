namespace CodeTogether.Client.Integration.Authentication;

[Serializable]
public class LoginRequestDTO
{
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}