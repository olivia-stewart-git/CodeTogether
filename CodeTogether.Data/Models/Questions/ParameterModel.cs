using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TC_PK))]
public class ParameterModel : IDbModel
{
	public Guid TC_PK { get; set; } = Guid.NewGuid();

	public required string TC_Name { get; set; }

	public required TypeModel TC_Type { get; set; }

	public required int TC_Position { get; set; }

	public required ScaffoldModel TC_Scaffold { get; set; }
}
