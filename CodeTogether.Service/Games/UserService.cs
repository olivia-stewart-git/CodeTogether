using CodeTogether.Data;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Service.Games
{
	public class UserService(ApplicationDbContext dbContext) : IUserService
	{
		public UserModel CreateUser(string name)
		{
			var user = new UserModel { UR_LastHeardFromAt = DateTime.Now, UR_Name = name };
			dbContext.Users.Add(user);
			return user;
		}
	}
}
