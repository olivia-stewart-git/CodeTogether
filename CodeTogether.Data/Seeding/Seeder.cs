using CodeTogether.Data.Models;

namespace CodeTogether.Data.Seeding;

public class Seeder
{
	readonly ApplicationDbContext dbContext;
	readonly Action<string> logging;

	public Seeder(ApplicationDbContext dbContext, Action<string> logging)
	{
		this.dbContext = dbContext;
		this.logging = logging;
	}

	public void ExplicitSeed(params Type[] seederTypes)
	{
		var steps = seederTypes
			.Select(x => Activator.CreateInstance(x, dbContext))
			.OfType<ISeedStep>()
			.OrderBy(x => x.Order);
		SeedCore(steps);
	}

	void SeedCore(IEnumerable<ISeedStep> steps)
	{
		var initialSeed = !HasSeeded();

		foreach (var seedStep in steps)
		{
			logging($"starting seed step: {seedStep.GetType().FullName}");
			seedStep.Seed(initialSeed);
		}

		FillHasSeeded();
		logging("Seeding complete");
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