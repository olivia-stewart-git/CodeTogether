namespace CodeTogether.Client.Integration.Authentication;

[Serializable]
public class LoginRequestDTO
{
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public class LoginResponseDTO
{
	public static LoginResponseDTO Failed => new LoginResponseDTO { IsAuthenticated = false, Message = "Invalid username or password." };
	public static LoginResponseDTO Success(string token) => new LoginResponseDTO { IsAuthenticated = true, Token = token };

	public bool IsAuthenticated { get; set; } = false;
	public string Token { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
}