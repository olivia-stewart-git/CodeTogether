using CodeTogether.Data.Models.Game;

namespace CodeTogether.Service.Games
{
	public interface IUserService
	{
		UserModel CreateUser(string name);
	}
}
