using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

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

			var seeder = new Seeder(new Mock<ApplicationDbContext>().Object, s => {});
			var seedSteps = seeder.GetSeedSteps().ToList();
			Assert.That(seedSteps.Count, Is.EqualTo(allSeedSteps.Count));
		}
	}
}
