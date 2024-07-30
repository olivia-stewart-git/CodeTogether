using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(EX_PK))]
public class ExecutionModel : IDbModel
{
	public Guid EX_PK { get; set; } = Guid.NewGuid();

	public DateTime EC_Date { get; set; } = DateTime.Now;
}