using CodeTogether.Client.Integration;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Service.Games
{
	public interface ILobbyService
	{
		Guid CreateLobby(string lobbyName);

		IEnumerable<GameListGameDTO> GetLobbies();

		void JoinLobby(Guid gameId, Guid userId);

		GameModel UpdateConfiguration(SetLobbyConfigurationDTO newState, GameModel game);
	}
}
