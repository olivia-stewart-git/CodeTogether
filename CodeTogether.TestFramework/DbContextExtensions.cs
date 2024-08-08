using System.Linq.Expressions;
using Moq;
using CodeTogether.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.TestFramework;

public static class DbContextExtensions
{
	public static Mock<ApplicationDbContext> SetupMock<T>(this Mock<ApplicationDbContext> dbContext, Expression<Func<ApplicationDbContext, DbSet<T>>> accessExpression, IList<T> set) where T : class
	{
		var data = set.AsQueryable();
		var mockSet = new Mock<DbSet<T>>();
		mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
		mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
		mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
		mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
		dbContext.Setup(x => x.Set<T>()).Returns(mockSet.Object);
		dbContext.Setup(accessExpression).Returns(mockSet.Object);
		return dbContext;
	}
}