using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;

namespace CodeTogether.Services.Games
{
	public interface IGameService
	{
		// If cache is true the gameModel recieved may be stale
		GameModel GetActiveGameForUser(ApplicationDbContext dbContext, string? userIdString, bool cache = true);

		void MarkAsFinished(ApplicationDbContext dbContext, Guid gameId, SubmissionModel fromSubmission);
	}
}
