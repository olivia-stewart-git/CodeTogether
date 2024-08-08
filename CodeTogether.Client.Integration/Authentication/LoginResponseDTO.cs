namespace CodeTogether.Client.Integration.Authentication;

[Serializable]
public class LoginResponseDTO
{
	public static LoginResponseDTO Failed => new LoginResponseDTO { IsAuthenticated = false, Message = "Invalid username or password." };
	public static LoginResponseDTO Success => new LoginResponseDTO { IsAuthenticated = true, };

	public bool IsAuthenticated { get; set; } = false;
	public string Message { get; set; } = string.Empty;
}