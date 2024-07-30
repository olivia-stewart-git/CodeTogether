using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(EX_PK))]
public class ExecutionModel
{
	public Guid EX_PK { get; set; } = Guid.NewGuid();

	public DateTime EC_Date { get; set; } = DateTime.Now;
}