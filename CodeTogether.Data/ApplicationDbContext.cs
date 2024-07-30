using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Questions;
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

	internal DbSet<StmDataModel> StmData { get; set; }

	internal DbSet<QuestionModel> Questions { get; set; }
	internal DbSet<TestCaseModel> TestCases { get; set; }
	internal DbSet<ExecutionConfigurationModel> ExecutionConfigurations { get; set; }
	internal DbSet<TypeModel> Types { get; set; }
	internal DbSet<ExecutionModel> Executions { get; set; }
		 
	#endregion
}