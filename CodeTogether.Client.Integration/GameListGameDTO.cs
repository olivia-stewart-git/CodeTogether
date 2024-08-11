namespace CodeTogether.Client.Integration;

// A game to be displayed in the game list
public class GameListGameDTO
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
	public required int NumPlayers { get; set; }
	public required bool Playing { get; set; }
	public required DateTime CreatedAt { get; set; }
}