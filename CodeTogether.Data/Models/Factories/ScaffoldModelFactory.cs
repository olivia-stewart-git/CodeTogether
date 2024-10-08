﻿using CodeTogether.Common;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using System.Collections.Concurrent;
using System.Reflection.Metadata;

namespace CodeTogether.Data.Models.Factories;

// ParameterModel should garentee that it has a scaffold, so should have a seperate type to represent just the info before we have created the scaffold
public record ParameterInfo(string Name, Type Type);

public interface IScaffoldModelFactory
{
	ScaffoldModel GetScaffold(IEnumerable<ParameterInfo> parameters, Type returnType, ExecutionRunnerType executionRunner = ExecutionRunnerType.ClassInstance);
}

public class ScaffoldModelFactory(ApplicationDbContext context, ICachedTypeModelFactory typeFactory) : IScaffoldModelFactory
{
	Dictionary<string, ScaffoldModel>? scaffoldNameCache;

	public ScaffoldModel GetScaffold(IEnumerable<ParameterInfo> parameters, Type returnType, ExecutionRunnerType executionRunner)
	{
		if (scaffoldNameCache == null)
		{
			scaffoldNameCache = new();
			foreach (var existingScaffold in context.Scaffolds)
			{
				scaffoldNameCache.Add(existingScaffold.EXE_ScaffoldName, existingScaffold);
			}
		}

		var key = GenerateNameKey(parameters, returnType, executionRunner);
		if (scaffoldNameCache.ContainsKey(key))
		{
			return scaffoldNameCache[key];
		}

		var resultScaffold = executionRunner switch
		{
			ExecutionRunnerType.ClassInstance => GetExecutionRunnerScaffold(parameters, returnType),
			_ => throw new NotImplementedException(),
		};

		scaffoldNameCache[key] = resultScaffold;

		return resultScaffold;
	}

	string GenerateNameKey(IEnumerable<ParameterInfo> parameters, Type returnType, ExecutionRunnerType executionRunner)
	{
		return $"{string.Join(",", parameters.Select(p => p.Type.Name + p.Name))}-TO-{returnType.Name}-{executionRunner.ToString()}";
	}

	// Can split each execution runner type creator into its own class when there are more

	ScaffoldModel GetExecutionRunnerScaffold(IEnumerable<ParameterInfo> parameters, Type returnType)
	{
		var parametersString = string.Join(", ", parameters.Select(p => $"{p.Type.GetAliasedName()} {p.Name}"));
		var scaffoldText = @$"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Problem
{{
	public {returnType.GetAliasedName()} Solve({parametersString})
	{{
		
	}}
}}";

		var scaffold = new ScaffoldModel
		{
			EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
			EXE_ExecutionRunnerArgument = "Problem::Solve",
			EXE_ReturnType = typeFactory.Get(returnType),
			EXE_ScaffoldName = GenerateNameKey(parameters, returnType, ExecutionRunnerType.ClassInstance),
			EXE_ScaffoldText = scaffoldText
		};


		var parameterModels = new List<ParameterModel>();
		var i = 0;
		foreach (var paramInfo in parameters)
		{
			parameterModels.Add(new ParameterModel { TC_Name = paramInfo.Name, TC_Position = i, TC_Type = typeFactory.Get(paramInfo.Type), TC_Scaffold = scaffold } );
			i++;
		}

		scaffold.EXE_Parameters = parameterModels;

		return scaffold;
	}
}
