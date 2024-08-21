using System.Reflection;
using CodeTogether.Data.Models.Questions;
using Newtonsoft.Json;

namespace CodeTogether.Data.Seeding;

public class QuestionLoader
{
	public static string QuestionDirectory => Path.Combine(Assembly.GetExecutingAssembly().Location, "QuestionFiles");

	public void SaveQuestion(QuestionModel question)
	{
		var asJson = JsonConvert.SerializeObject(question);
		var filePath = Path.Combine(QuestionDirectory, $"{question.QST_Name}_question.json");
		File.WriteAllText(filePath, asJson);
	}

	public IEnumerable<QuestionModel> LoadQuestions()
	{
		string[] questionFiles = new string[] { }; ;
		try
		{
			questionFiles = Directory.GetFiles(QuestionDirectory, "*.json");
		}
		catch (DirectoryNotFoundException)
		{
			Directory.CreateDirectory(QuestionDirectory);
		}

		return questionFiles
			.Where(x => x.EndsWith("_question.json", StringComparison.Ordinal))
			.Select(LoadQuestionFromFile)
			.OfType<QuestionModel>();
	}

	public QuestionModel? LoadQuestionFromFile(string path)
	{
		if (!File.Exists(path))
		{
			return null;
		}

		var asJson = File.ReadAllText(path);
		return JsonConvert.DeserializeObject<QuestionModel>(asJson);
	}
}