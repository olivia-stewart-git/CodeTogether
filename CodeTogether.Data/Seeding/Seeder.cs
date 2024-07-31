using CodeTogether.Data.Models;

namespace CodeTogether.Data.Seeding;

public class Seeder
{
	readonly ApplicationDbContext dbContext;

	public Seeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Seed()
	{
		if (HasSeeded())
		{
			return; 
		}

		List<ISeedStep> steps =
		[
			new QuestionSeeder(dbContext),
			new SubmissionSeeder(dbContext)
		];


		foreach (var seedStep in steps)
		{
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