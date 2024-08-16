using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models;
using System.Security.Cryptography;
using System.Text;

namespace CodeTogether.Data.Seeding
{
	public class SchemaVersionSeeder(ApplicationDbContext dbContext) : ISeedStep
	{
		public int Order => 1;

		public void Seed(bool initalSeed)
		{
			var schemaHash = GetSchemaVersionHash();
			dbContext.StmData.Add(new StmDataModel { STM_Key = StmDataModel.Constants.SchemaVersion, STM_Value = schemaHash});
			dbContext.SaveChanges();
		}

		static string GetSchemaVersionHash()
		{
			var tables = typeof(IDbModel).Assembly.GetTypes()
				.Where(t => typeof(IDbModel).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
				.ToList();
			var columns = tables.SelectMany(t => t.GetProperties().Select(c => t.Name + c.Name + c.Attributes.ToString()));
			var columnsString = string.Join("", columns);
			using (var algo = SHA256.Create())
			{
				var digest = algo.ComputeHash(Encoding.UTF8.GetBytes(columnsString));
				return Convert.ToBase64String(digest);
			}
		}

		public static void CheckSchemaVersion()
		{
			using var dbContext = new ApplicationDbContext();
			var expectedSchemaVersionHash = GetSchemaVersionHash();
			var actualSchemaVersionHash = dbContext.StmData.FirstOrDefault(x => x.STM_Key == StmDataModel.Constants.SchemaVersion)?.STM_Value;
			if (actualSchemaVersionHash == null)
			{
				return;
			}
			if (expectedSchemaVersionHash != actualSchemaVersionHash)
			{
				throw new InvalidOperationException("Schema in database is outdated from schema in code, please run CodeTogether.Deployment to update");
			}
		}
	}
}
