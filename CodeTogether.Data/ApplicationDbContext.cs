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

	public virtual DbSet<StmDataModel> StmData { get; set; }

	public virtual DbSet<QuestionModel> Questions { get; set; }

	public virtual DbSet<TestCaseModel> TestCases { get; set; }
	public virtual DbSet<TestRunModel> TestRuns { get; set; }
	public virtual DbSet<TestRunExecutionModel> TestExecutions { get; set; }

	public virtual DbSet<ExecutionConfigurationModel> ExecutionConfigurations { get; set; }
	public virtual DbSet<TypeModel> Arguments { get; set; }
	public virtual DbSet<QuestionSignatureModel> ArgumentCollections { get; set; }

	public virtual DbSet<SubmissionModel> Submissions { get; set; }
	public virtual DbSet<ExecutionResultModel> ExecutionResults { get; set; }

	public virtual DbSet<GameModel> Games { get; set; }
	public virtual DbSet<UserModel> Users { get; set; }

	#endregion
}