using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TC_PK))]
public class ArgumentCollectionModel : IDbModel
{
	public Guid TC_PK { get; set; } = Guid.NewGuid();

	[InverseProperty(nameof(TypeModel.OT_Parent))]
	public IList<TypeModel> TC_Types { get; set; } = [];
}