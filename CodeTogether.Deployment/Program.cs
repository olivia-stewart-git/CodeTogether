using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using CodeTogether.Services.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Deployment;

class Program
{
	static void Main(string[] args)
	{
		if (!args.Contains("SeedOnly"))
		{
			var ranSuccessfully = ScriptRunner.RunPowershellScript();
			if (!ranSuccessfully)
			{
				return;
			}
		}

		using var dbContext = new ApplicationDbContext();
		dbContext.Database.BeginTransaction();

		dbContext.Database.EnsureCreated();
		dbContext.Database.Migrate();

		dbContext.Database.CommitTransaction();

		var seeder = new Seeder(dbContext, Console.WriteLine);
		seeder.ExplicitSeed(
			typeof(UserSeeder),
			typeof(QuestionSeeder),
			typeof(SchemaVersionSeeder)
		);
	}
}