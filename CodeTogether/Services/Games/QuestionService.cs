using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Services.Games
{
	public class QuestionService(ApplicationDbContext dbContext)
	{
		public QuestionDTO? GetQuestionById(Guid? questionId)
		{
			var question = dbContext.Questions
				.Include(x => x.QST_Scaffold)
				.Include(x => x.QST_TestCases)
				.Where(x => x.QST_PK == questionId)
				.FirstOrDefault();

			if (question == null)
			{
				return null;
			}

			return new QuestionDTO
			{
				Id = question.QST_PK,
				Name = question.QST_Name,
				Description = question.QST_Description,
				ScaffoldCode = question.QST_Scaffold.EXE_ScaffoldText,
				TestCases = question.QST_TestCases.Select(x => new TestCaseDto
				{
					Id = x.TST_PK,
					Name = x.TST_Title,
					Arguments = x.TST_Arguments,
					ExpectedResponse = x.TST_ExpectedResponse,
				}).ToList()
			};
		}
	}
}
