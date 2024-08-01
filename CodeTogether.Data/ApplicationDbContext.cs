using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Engine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace CodeTogether.Data;

public class ApplicationDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=CodeTogether;Integrated Security=SSPI;TrustServerCertificate=True", x => x.MigrationsAssembly("CodeTogether.Migrations"));
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TestCaseModel>()
			.Property(x => x.TST_Arguments)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
				);
	}

    #region Models

    public DbSet<StmDataModel> StmData { get; set; }

	public DbSet<QuestionModel> Questions { get; set; }

	public DbSet<TestCaseModel> TestCases { get; set; }
	public DbSet<TestRunModel> TestRuns { get; set; }
	public DbSet<TestRunExecutionModel> TestExecutions { get; set; }

	public DbSet<ExecutionConfigurationModel> ExecutionConfigurations { get; set; }
    public DbSet<TypeModel> Arguments { get; set; }
	public DbSet<ArgumentCollectionModel> ArgumentCollections { get; set; }

	public DbSet<SubmissionModel> Submissions { get; set; }
	public DbSet<ExecutionResultModel> ExecutionResults { get; set; }
    #endregion
}