﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TST_PK))]
public class TestCaseModel
{
	public TestCaseModel(string title, string[] arguments, string expectedResponse, bool isHidden)
	{
		TST_Title = title;
		TST_Arguments = arguments;
		TST_ExpectedResponse = expectedResponse;
		TST_IsHidden = isHidden;
	}

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
	public Guid? TST_QST_FK{ get; set; }
}