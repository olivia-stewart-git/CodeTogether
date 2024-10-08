﻿using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(QST_PK))]
public class QuestionModel : IDbModel
{
	public Guid QST_PK { get; set; } = Guid.NewGuid();

	[Required]
	[MaxLength(100)]
	public required string QST_Name { get; set; }

	[Required]
	[MaxLength(int.MaxValue)]
	public required string QST_Description { get; set; }

	public required ScaffoldModel QST_Scaffold { get; set; }

	[InverseProperty(nameof(TestCaseModel.TST_Question))]
	public IEnumerable<TestCaseModel> QST_TestCases { get; set; } = new List<TestCaseModel>();
}