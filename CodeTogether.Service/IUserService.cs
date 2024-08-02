using CodeTogether.Data.Models.Game;

namespace CodeTogether.Service
{
	internal interface IUserService
	{
		UserModel CreateUser(string name);
	}
}
