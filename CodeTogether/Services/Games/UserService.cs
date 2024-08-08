using CodeTogether.Data;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Service.Games
{
	public class UserService(ApplicationDbContext dbContext) : IUserService
	{
		public UserModel CreateUser(string name)
		{
			var user = new UserModel { USR_LastHeardFromAt = DateTime.Now, USR_UserName = name };
			dbContext.Users.Add(user);
			return user;
		}
	}
}
