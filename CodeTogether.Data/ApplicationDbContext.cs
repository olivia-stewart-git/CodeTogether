using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Engine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CodeTogether.Data;

public class ApplicationDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var builder = new ConfigurationBuilder()
			.SetBasePath(binPath)
			.AddJsonFile("appsettings.json");
		var configuration = builder.Build();
		var connectionString = configuration.GetConnectionString("MainDb");
		optionsBuilder.UseSqlServer(connectionString);
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

		modelBuilder.Entity<UserModel>()
			.Property(x => x.USR_CheckPoints)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries),
				stringValueComparer);

		modelBuilder.HasJsonConversion<SubmissionResultModel, Exception>(x => x.EXR_CompileError)
					.HasJsonConversion<TestRunModel, Exception>(x => x.TCR_Exception);
	}

	#region Models

	public virtual DbSet<StmDataModel> StmData { get; set; }

	public virtual DbSet<QuestionModel> Questions { get; set; }

	public virtual DbSet<TestCaseModel> TestCases { get; set; }
	public virtual DbSet<TestRunModel> TestRuns { get; set; }

	public virtual DbSet<ScaffoldModel> Scaffolds { get; set; }
	public virtual DbSet<TypeModel> Types { get; set; }
	public virtual DbSet<ParameterModel> Parameters { get; set; }

	public virtual DbSet<SubmissionModel> Submissions { get; set; }
	public virtual DbSet<SubmissionResultModel> SubmissionResults { get; set; }

	public virtual DbSet<GameModel> Games { get; set; }
	public virtual DbSet<UserModel> Users { get; set; }
	public virtual DbSet<GamePlayerModel> GamePlayers { get; set; }

	#endregion
}