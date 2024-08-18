using CodeTogether.Client.Integration.Execution;

namespace CodeTogether.Client.Integration
{
	public class GameWinInfoDTO
	{
		public required string GameName { get; set; }
		public required string WinnerName { get; set; }
		public required string WinnerCode { get; set; }
		public required QuestionDTO Question { get; set; }
	}
}
