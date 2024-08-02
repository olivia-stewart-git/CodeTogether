using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Service
{
	internal interface IGameService
	{
		GameModel CreateGame(string lobbyName);

		IEnumerable<GameModel> GetGames();

		void JoinGame(UserModel user);
	}
}
