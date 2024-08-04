namespace CodeTogether.Client.Integration
{
	// A game to be displayed in the game list
	public class GameListGameDTO
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public int NumPlayers { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
