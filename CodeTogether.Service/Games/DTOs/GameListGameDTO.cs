namespace CodeTogether.Service.Games.DTOs
{
	// A game to be displayed in the game list
	internal class GameListGameDTO
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public int NumPlayers { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
