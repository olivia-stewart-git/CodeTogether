using Moq;
using CodeTogether.TestFramework;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Data.Seeding.Test
{
	internal class SeederTest
	{
		[Test]
		public void TestLocatesAllSeedSteps()
		{
			var allSeedSteps = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.GetInterfaces().Contains(typeof(ISeedStep)))
				.ToList();

			var mockDb = new Mock<ApplicationDbContext>();
			mockDb.SetupMockDbSet(db => db.Scaffolds, new List<ScaffoldModel>());
			var seeder = new Seeder(mockDb.Object, s => {});
			var seedSteps = seeder.GetSeedSteps().ToList();
			Assert.That(seedSteps.Count, Is.EqualTo(allSeedSteps.Count));
		}
	}
}
