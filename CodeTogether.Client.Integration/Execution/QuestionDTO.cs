namespace CodeTogether.Client.Integration.Execution;

[Serializable]
public class QuestionDTO
{
	public Guid Id { get; set; } = Guid.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

}