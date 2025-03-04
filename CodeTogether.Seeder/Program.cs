using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Seeder
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var connectionString = ApplicationDbContext.GetConnectionStringFromConfig();
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseNpgsql(connectionString);
			var dbContext = new ApplicationDbContext(optionsBuilder.Options);

			new Data.Seeding.Seeder(dbContext, Console.WriteLine).ExplicitSeed(
				typeof(SchemaVersionSeeder),
				typeof(QuestionSeeder)
			);
		}
	}
}
