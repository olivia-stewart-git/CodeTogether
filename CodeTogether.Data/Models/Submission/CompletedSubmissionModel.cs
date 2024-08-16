using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Runner.Engine;

[PrimaryKey(nameof(CSM_PK))]
public class CompletedSubmissionModel : IDbModel
{
	public Guid CSM_PK { get; set; } = Guid.NewGuid();

	[DeleteBehavior(DeleteBehavior.NoAction)]
	[ForeignKey(nameof(CSM_EXR_FK))]
	public required SubmissionResultModel CSM_Result { get; set; }
	public Guid CSM_EXR_FK { get; set; }

	public DateTime CSM_CompletedAt { get; set; } = DateTime.Now;

	[DeleteBehavior(DeleteBehavior.NoAction)]
	[ForeignKey(nameof(CSM_GM_FK))]
	public GameModel? CSM_Game { get; set; } = null!;

	public Guid? CSM_GM_FK { get; set; } = null!;

	[DeleteBehavior(DeleteBehavior.NoAction)]
	[ForeignKey(nameof(CSM_USR_FK))]
	public UserModel CSM_User { get; set; } = null!;
	public Guid? CSM_USR_FK { get; set; } = null!;
}