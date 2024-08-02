using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Engine;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CodeTogether.Data;

public class ApplicationDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseSqlServer("Server=localhost;Initial Catalog=CodeTogether;Integrated Security=SSPI;TrustServerCertificate=True");
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var stringValueComparer = new ValueComparer<IEnumerable<string>>(
			(c1, c2) => c1.SequenceEqual(c2),
			c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
			c => c);

		modelBuilder.Entity<TestCaseModel>()
			.Property(x => x.TST_Arguments)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries),
				stringValueComparer);

		modelBuilder.Entity<ExecutionResultModel>()
			.Property(x => x.EXR_Exception)
			.HasConversion(
				v => ConvertJson(v),        // Converts Exception to JSON string
				v => DeserializeJson<Exception>(v)); // Converts JSON string back to Exception
	}

	static string ConvertJson(object? value) => JsonSerializer.Serialize(value);
	static T? DeserializeJson<T>(string value) => JsonSerializer.Deserialize<T>(value);

	#region Models

	public DbSet<StmDataModel> StmData { get; set; }

	public DbSet<QuestionModel> Questions { get; set; }

	public DbSet<TestCaseModel> TestCases { get; set; }
	public DbSet<TestRunModel> TestRuns { get; set; }
	public DbSet<TestRunExecutionModel> TestExecutions { get; set; }

	public DbSet<ExecutionConfigurationModel> ExecutionConfigurations { get; set; }
    public DbSet<TypeModel> Arguments { get; set; }
	public DbSet<QuestionSignatureModel> ArgumentCollections { get; set; }

	public DbSet<SubmissionModel> Submissions { get; set; }
	public DbSet<ExecutionResultModel> ExecutionResults { get; set; }
    #endregion
}