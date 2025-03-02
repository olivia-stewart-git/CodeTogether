using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace CodeTogether.Data.Seeding
{
	public class SchemaVersionSeeder(ApplicationDbContext dbContext) : ISeedStep
	{
		public int Order => 1;

		public void Seed(bool initalSeed)
		{
			var schemaHash = GetSchemaVersionHashFromModel();
			dbContext.StmData.Add(new StmDataModel { STM_Key = StmDataModel.Constants.SchemaVersion, STM_Value = schemaHash});
			dbContext.SaveChanges();
		}

		static string GetSchemaVersionHashFromModel()
		{
			var tables = typeof(IDbModel).Assembly.GetTypes()
				.Where(t => typeof(IDbModel).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
				.ToList();
			var columns = tables.SelectMany(t => t.GetProperties().Select(c => t.Name + c.Name + c.Attributes.ToString()));
			var columnsString = string.Join("", columns);
			var digest = SHA256.HashData(Encoding.UTF8.GetBytes(columnsString));
			return Convert.ToBase64String(digest);
		}
		
		string? GetSchemaHashFromDatabase(){
			try{
				return dbContext.StmData.FirstOrDefault(x => x.STM_Key == StmDataModel.Constants.SchemaVersion)?.STM_Value;
			} catch (PostgresException ex) when (ex.Message.Contains("does not exist")){
				return null;
			}
		}

		public void CheckSchemaVersion(bool fixIfOutdated)
		{
			var expectedSchemaVersionHash = GetSchemaVersionHashFromModel();
			var actualSchemaVersionHash = GetSchemaHashFromDatabase();
			
			var isOutdated = expectedSchemaVersionHash != actualSchemaVersionHash;
			if (isOutdated)
			{
				if (!fixIfOutdated){
					throw new InvalidOperationException("Schema in database is outdated from schema in code, create a migration and then run CodeTogether.Deployment to update");
				}
				Console.WriteLine("Schema was outdated, recreating database");
				dbContext.Database.EnsureDeleted();
				Console.WriteLine("Database deleted");
				dbContext.Database.EnsureCreated();
				Console.WriteLine("Database recreated");
			}
		}
	}
}
