using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Data.Models.Questions.Test;

internal class TypeModelTest
{
	[Test]
	public void TestNullTypeWhenInvalidName()
	{
		var typeModel = new TypeModel("something", "something");
		Assert.Null(typeModel.OT_Type);
    }

	[Test]
	public void TestLoadsCorrectType()
	{
		var type = typeof(int);
		Assert.IsNotNull(type);
		var typeModel = new TypeModel(type.Assembly.FullName!, type.FullName!);

		Assert.IsNotNull(typeModel.OT_Type);
		Assert.That(typeModel.OT_Type, Is.EqualTo(type));
	}

	[Test]
	public void TestCreatesFromFactoryMethod()
	{
		var type = GetType();
		var typeModel = TypeModel.FromType(type);
		Assert.IsNotNull(typeModel.OT_Type);
		Assert.That(typeModel.OT_Type, Is.EqualTo(type));
	}
}