namespace CodeTogether.Data.DataAccess;

/// <summary>
/// A single unitOfWork or transaction in the db.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
	readonly ApplicationDbContext dbContext;

	internal UnitOfWork(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	readonly Dictionary<Type, object> repositoryMap = [];

	//Generic implementation of repository pattern
	public IRepository<T> GetRepository<T>() where T : class, IDbModel
	{
		if (repositoryMap.TryGetValue(typeof(T), out var repository))
		{
			return (IRepository<T>)repository;
		}

		var repositoryInstance = new Repository<T>(dbContext);
		repositoryMap[typeof(T)] = repositoryInstance;
		return repositoryInstance;
	}

	public void Commit()
	{
		dbContext.SaveChanges();
	}
}