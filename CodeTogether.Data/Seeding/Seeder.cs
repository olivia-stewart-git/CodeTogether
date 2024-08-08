using CodeTogether.Data.Models;

namespace CodeTogether.Data.Seeding;

public class Seeder : ISeeder
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

	public void Seed()
	{
		SeedCore(GetSeedSteps());
	}

	void SeedCore(IEnumerable<ISeedStep> steps)
	{
		if (HasSeeded())
		{
			return;
		}

		foreach (var seedStep in steps)
		{
			logging($"starting seed step: {seedStep.GetType().FullName}");
			seedStep.Seed();
		}

		FillHasSeeded();
		logging("Seeding complete");
	}

	public IEnumerable<ISeedStep> GetSeedSteps()
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		return assemblies
			.SelectMany(x => x.GetTypes())
			.Where(x => typeof(ISeedStep).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
			.Select(x => Activator.CreateInstance(x, dbContext))
			.OfType<ISeedStep>()
			.OrderBy(x => x.Order);
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