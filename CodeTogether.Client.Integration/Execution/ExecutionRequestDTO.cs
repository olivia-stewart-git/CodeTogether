namespace CodeTogether.Client.Integration.Execution;

[Serializable]
public class ExecutionRequestDTO
{
	public required string RawCode { get; set; } = string.Empty;
	public required Guid QuestionId { get; set; } = Guid.Empty;
	public required Guid GameId { get; set; } = Guid.Empty;
}