﻿using CodeTogether.Data.Models;
using CodeTogether.Data.Models.Questions;
using CodeTogether.TestFramework;
using Moq;
using System.Collections.Generic;

namespace CodeTogether.Data.Seeding.Test
{
	internal class SeederTest
	{
		[Test]
		public void TestCallsStepsInOrder()
		{
			var mockDbContext = new Mock<ApplicationDbContext>();
			mockDbContext.SetupMockDbSet(db => db.StmData, new List<StmDataModel> { new StmDataModel { STM_Key = StmDataModel.Constants.HasSeeded, STM_Value = "Y", STM_PK = Guid.NewGuid()} });
			mockDbContext.SetupMockDbSet(db => db.Scaffolds, new List<ScaffoldModel>());
			new Seeder(mockDbContext.Object, log => { }).ExplicitSeed(typeof(TestSeeder1), typeof(TestSeeder2));
			Assert.That(TestSeeder1.CalledAt, Is.LessThan(TestSeeder2.CalledAt));
		}
	}

	class TestSeeder1 : ISeedStep
	{
		public TestSeeder1(ApplicationDbContext _) { }

		public int Order => 1;

		public static DateTime? CalledAt;

		public void Seed(bool initialSeed)
		{
			CalledAt = DateTime.Now;
		}
	}

	class TestSeeder2 : ISeedStep
	{
		public TestSeeder2(ApplicationDbContext _) { }

		public int Order => 2;

		public static DateTime? CalledAt;

		public void Seed(bool initialSeed)
		{
			CalledAt = DateTime.Now;
		}
	}
}
