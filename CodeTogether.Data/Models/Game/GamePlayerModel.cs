﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using Microsoft.EntityFrameworkCore;
// ReSharper disable InconsistentNaming

namespace CodeTogether.Data.Models.Game;
 
[PrimaryKey(nameof(GMP_PK))]
public class GamePlayerModel : IDbModel
{
	public Guid GMP_PK { get; set; }

	[ForeignKey(nameof(GMP_GM_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public GameModel GMP_Game { get; set; } = null!;
	public Guid GMP_GM_FK { get; set; }

	[ForeignKey(nameof(GMP_USR_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public UserModel GMP_User { get; set; } = null!;
	public Guid GMP_USR_FK { get; set; }

	[InverseProperty(nameof(SubmissionModel.SBM_SubmittedBy))]
	public IEnumerable<SubmissionModel> Submissions { get; set; } = new List<SubmissionModel>();
}