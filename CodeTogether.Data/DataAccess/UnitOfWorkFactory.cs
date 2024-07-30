namespace CodeTogether.Data.DataAccess;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
	readonly ApplicationDbContext dbContext;

	public UnitOfWorkFactory(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public IUnitOfWork CreateUnitOfWork()
	{
		return new UnitOfWork(dbContext);
	}
}