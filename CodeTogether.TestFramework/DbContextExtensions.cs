using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CodeTogether.TestFramework;

public static class DbContextExtensions
{
	public static Mock<DbContext> SetupMock<T>(this Mock<DbContext> dbContext,  IList<T> set) where T : class
	{
		var mockSet = new Mock<DbSet<T>>();
		mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(() => set.AsQueryable().Provider);
		mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(() => set.AsQueryable().Expression);
		mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(() => set.AsQueryable().ElementType);
		mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => set.AsQueryable().GetEnumerator());

		mockSet.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>(val =>
		{
			var index = 0;
			if ((index = set.IndexOf(val)) > 0)
			{
				set.RemoveAt(index);
			}
		});

		mockSet.Setup(x => x.Intersect(It.IsAny<IEnumerable<T>>()))
			.Callback<IEnumerable<T>>(x =>
			{
				foreach (var value in x)
				{
					set.Add(value);
				}
			});
		return dbContext;
	}
}