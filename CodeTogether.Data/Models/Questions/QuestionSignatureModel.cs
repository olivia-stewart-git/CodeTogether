using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TC_PK))]
public class QuestionSignatureModel : IDbModel
{
	public Guid TC_PK { get; set; } = Guid.NewGuid();

	[ForeignKey(nameof(TC_TO_FK))]
	public IList<TypeModel> TC_Types { get; set; } = [];
	public Guid TC_TO_FK { get; set; }
}