using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Factories;

namespace CodeTogether.Data.Seeding;

internal class UserSeeder : ISeeder
{
	public UserSeeder(ApplicationDbContext dbContext)
	{

	}

	public void Seed()
	{

	}

	void CreateAdminUser()
	{

	}
}
public class Seeder : ISeeder
{
	readonly ApplicationDbContext dbContext;
	readonly Action<string> logging;
	readonly CachedCachedTypeModelFactory typeModelFactory;

	public Seeder(ApplicationDbContext dbContext, Action<string> logging)
	{
		this.dbContext = dbContext;
		this.logging = logging;
		this.typeModelFactory = new CachedCachedTypeModelFactory();
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
			logging($"starting seed step: {seedStep.GetType().FullName}");
			seedStep.Seed();
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