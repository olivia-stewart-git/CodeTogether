using CodeTogether.Client.Integration;

namespace CodeTogether.Service.Games
{
	public interface ILobbyService
	{
		Guid CreateLobby(string lobbyName);

		IEnumerable<GameListGameDTO> GetLobbies();

		void JoinLobby(Guid gameId, Guid userId);
	}
}
