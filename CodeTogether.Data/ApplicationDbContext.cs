using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Engine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

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

		modelBuilder.HasJsonConversion<ExecutionResultModel, Exception>(x => x.EXR_Exception)
					.HasJsonConversion<TestRunModel, Exception>(x => x.TCR_Exception);
	}

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

	public DbSet<GameModel> Games { get; set; }
	public DbSet<UserModel> Users { get; set; }

	#endregion
}