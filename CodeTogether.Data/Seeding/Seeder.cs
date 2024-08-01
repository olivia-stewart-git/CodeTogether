using CodeTogether.Common.Logging;
using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Factories;

namespace CodeTogether.Data.Seeding;

public class Seeder : ISeeder
{
	readonly ApplicationDbContext dbContext;
	readonly ICachedTypeModelFactory typeModelFactory;
	readonly ILoggerManager logManager;

	public Seeder(ApplicationDbContext dbContext, ICachedTypeModelFactory typeModelFactory, ILoggerManager logManager)
	{
		this.dbContext = dbContext;
		this.typeModelFactory = typeModelFactory;
		this.logManager = logManager;
	}

	public void Seed()
	{
		if (HasSeeded())
		{
			return;
		}

		List<ISeeder> steps =
		[
			new QuestionSeeder(dbContext, typeModelFactory),
		];

		foreach (var seedStep in steps)
		{
			logManager.LogInfo($"starting seed step: {seedStep.GetType().FullName}");
			seedStep.Seed();
		}

		FillHasSeeded();
	}

	bool HasSeeded()
	{
		var seeded = dbContext.StmData.FirstOrDefault(x => x.STM_Key == StmDataModel.Constants.HasSeeded);
		if (seeded != null)
		{
			return seeded.STM_Value == "Y";
		}

		return false;
	}

	void FillHasSeeded()
	{
		dbContext.StmData.Add(new StmDataModel()
		{
			STM_Key = StmDataModel.Constants.HasSeeded,
			STM_Value = "Y",
		});
		dbContext.SaveChanges();
	}
}