using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(QST_PK))]
public class QuestionModel : IDbModel
{
    public Guid QST_PK { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string QST_Name { get; set; }

    [Required]
    [MaxLength(300)]
    public string QST_Description { get; set; }

    [ForeignKey(nameof(QST_EXE_FK))]
    public ExecutionConfigurationModel QST_ExecutionConfigurationModel { get; set; }
    public Guid QST_EXE_FK { get; set; }

    [InverseProperty(nameof(TestCaseModel.TST_Question))]
    public IEnumerable<TestCaseModel> QST_TestCases { get; set; } = [];
}