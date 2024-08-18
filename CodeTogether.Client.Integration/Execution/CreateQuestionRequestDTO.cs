namespace CodeTogether.Client.Integration.Execution;

[Serializable]
public class CreateQuestionRequestDTO
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public List<(string, string)> Arguments { get; set; } = new();
	public string ReturnType { get; set; } = string.Empty;
	public List<TestCaseDto> TestCases { get; set; } = new();
}