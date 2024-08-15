using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using System.Collections.Concurrent;
using System.Reflection.Metadata;

namespace CodeTogether.Runner.Scaffolds;

internal class ScaffoldModelFactory
{
	public ScaffoldModelFactory(ApplicationDbContext context)
	{
		this.context = context;
		foreach (var scaffold in context.Scaffolds)
		{
			scaffoldNameCache.Add(scaffold.EXE_ScaffoldName, scaffold);
		}
	}

	Dictionary<string, ScaffoldModel> scaffoldNameCache = new ();
	ApplicationDbContext context;

	public ScaffoldModel GetScaffold(List<ParameterModel> parameters, TypeModel returnType, ExecutionRunnerType executionRunner)
	{
		var key = GenerateNameKey(parameters, returnType, executionRunner);
		if (scaffoldNameCache.ContainsKey(key))
		{
			return scaffoldNameCache[key];
		}

		return executionRunner switch
		{
			ExecutionRunnerType.ClassInstance => GetExecutionRunnerScaffold(parameters, returnType),
			_ => throw new NotImplementedException(),
		};
	}

	string GenerateNameKey(List<ParameterModel> parameters, TypeModel returnType, ExecutionRunnerType executionRunner)
	{
		return string.Join(",", parameters.Select(p => p.TC_Name + p.TC_Type.OT_TypeName)) + "-TO-" + returnType.OT_TypeName + "classInstance";
	}

	// Can split each execution runner type creator into its own class when there are more

	ScaffoldModel GetExecutionRunnerScaffold(List<ParameterModel> parameters, TypeModel returnType)
	{
		var parametersString = string.Join(", ", parameters.Select(p => $"{p.TC_Type.OT_TypeName} {p.TC_Name}"));
		var scaffoldText = string.Format(classInstanceTemplate, returnType, parametersString);

		return new ScaffoldModel
		{
			EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
			EXE_ExeuctionRunnerArgument = "Problem::Solve",
			EXE_ReturnType = returnType,
			EXE_Parameters = parameters,
			EXE_ScaffoldName = GenerateNameKey(parameters, returnType, ExecutionRunnerType.ClassInstance),
			EXE_ScaffoldText = scaffoldText
		};
	}

	const string classInstanceTemplate = @"using System;

public class Problem
{
	public {0} Solve({1})
	{
	}
}
";

}
