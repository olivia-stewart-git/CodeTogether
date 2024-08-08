using System.Text;

namespace CodeTogether.Client.Integration.Authentication;

[Serializable]
public struct RegistrationRequestResponseDTO
{
	public static RegistrationRequestResponseDTO Invalid(string message) => new (RegistrationState.Failure, message);
	public static RegistrationRequestResponseDTO Valid(string message) => new(RegistrationState.Success, message);

	public RegistrationRequestResponseDTO(RegistrationState state, string message)
	{
		State = state;
		Message = message;
	}

	public static RegistrationRequestResponseDTO Combined(params RegistrationRequestResponseDTO[] responses)
	{
		var state = responses.Any(x => x.State == RegistrationState.Failure)
			? RegistrationState.Failure 
			: RegistrationState.Success;

		var sb = new StringBuilder();
		foreach (var response in responses)
		{
			sb.AppendLine(response.Message);
		}
		return new (state, sb.ToString());
	}

	public RegistrationState State { get; set; }
	public string Message { get; set; }
}