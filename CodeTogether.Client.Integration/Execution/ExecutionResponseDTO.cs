namespace CodeTogether.Client.Integration.Execution;

public class ExecutionResponseDTO
{
	public Guid Id;
	public string Output { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public TestCaseDto.RunDTO[] TestResults { get; set; } = [];

	Dictionary<Guid, TestCaseDto.RunDTO>? testResultsDictionary;
	public Dictionary<Guid, TestCaseDto.RunDTO> TestResultsDictionary => testResultsDictionary??= TestResults.ToDictionary(x => x.Id);
}