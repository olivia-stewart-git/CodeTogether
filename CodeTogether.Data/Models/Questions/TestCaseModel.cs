﻿using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TST_PK))]
public class TestCaseModel : IDbModel
{
	public Guid TST_PK { get; set; } = Guid.NewGuid();

	public bool TST_IsHidden { get; set; }

	[MaxLength(30)]
	public string TST_Title { get; set; }

	[MaxLength(100)]
	public string[] TST_Arguments { get; set; }

	[MaxLength(100)]
	public string TST_ExpectedResponse { get; set; }

	[ForeignKey(nameof(TST_QST_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public QuestionModel? TST_Question { get; set; } = null!;
	public Guid? TST_QST_FK { get; set; }
}