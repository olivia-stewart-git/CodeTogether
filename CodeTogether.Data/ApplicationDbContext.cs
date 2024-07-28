using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data;

public class ApplicationDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
	}
}