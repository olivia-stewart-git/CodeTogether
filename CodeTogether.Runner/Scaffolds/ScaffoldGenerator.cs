using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Scaffolds;

internal class ScaffoldGenerator : IScaffoldLoader
{
	const string template = @"using System;
public class {0}
{
public {1} Solve({2})
{
}
}
";

	public string LoadScaffold(QuestionModel question)
	{
		var className = question.QST_Name.Replace(" ", "");
		var returnType = question.QST_Scaffold.EXE_ReturnArgument.OT_TypeName;
		var parametersString = string.Join(", ", question.QST_Scaffold.EXE_Parameters.Select(GetStringForParameter));
		return string.Format(template, 
			className,
			returnType,
			parametersString
		);
	}

	string GetStringForParameter(ParameterModel parameter)
	{
		return $"{parameter.TC_Type.OT_TypeName} {parameter.TC_Name}";
	}
}
