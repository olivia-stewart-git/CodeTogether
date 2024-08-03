using CodeTogether.Service.Games.DTOs;

namespace CodeTogether.Service.Games
{
	public interface ILobbyService
	{
		Guid CreateGame(string lobbyName);

		IEnumerable<GameListGameDTO> GetGames();

		void JoinGame(Guid gameId, Guid userId);
	}
}
