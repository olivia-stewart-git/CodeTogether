namespace CodeTogether.Client.Integration.Execution;

[Serializable]
public class ExecutionRequestDTO
{
	public string RawCode { get; set; } = string.Empty;
	public Guid QuestionId { get; set; } = Guid.Empty;
	public Guid GameId { get; set; } = Guid.Empty;
	public Guid UserId { get; set; } = Guid.Empty;
}