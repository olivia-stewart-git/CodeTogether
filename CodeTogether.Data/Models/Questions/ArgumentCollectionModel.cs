using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TC_PK))]
public class ArgumentCollectionModel : IDbModel
{
	public Guid TC_PK { get; set; } = Guid.NewGuid();

	[InverseProperty(nameof(ArgumentModel.OT_Parent))]
	public IList<ArgumentModel> TC_Types { get; set; } = [];
}