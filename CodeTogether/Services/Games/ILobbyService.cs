using CodeTogether.Client.Integration;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CodeTogether.Service.Games
{
	public interface ILobbyService
	{
		Guid CreateLobby(string lobbyName, string creatorName);

		IEnumerable<GameListGameDTO> GetLobbies();

		void JoinLobby(Guid gameId, Guid userId);

		GameModel UpdateConfiguration(SetLobbyConfigurationDTO newState, GameModel game);
	}
}
