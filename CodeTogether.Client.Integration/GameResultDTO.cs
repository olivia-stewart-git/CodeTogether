namespace CodeTogether.Client.Integration;

public class GameResultDTO
{
	public required string GameName { get; set; }
	public required DateTime GameFinishedUtc { get; set; }
	public required string WinnerUsername { get; set; }
	public required bool WinnerIsYou { get; set; }
	public required string WinnerCode { get; set; }
}