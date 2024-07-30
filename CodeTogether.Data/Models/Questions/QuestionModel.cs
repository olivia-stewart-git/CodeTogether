using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(QST_PK))]
public class QuestionModel
{
	public QuestionModel(string name, string description, ExecutionConfigurationModel executionConfigurationModel)
	{
		QST_Name = name;
        QST_Description = description;
        QstExecutionConfigurationModel = executionConfigurationModel;
	}

    public Guid QST_PK { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string QST_Name { get; set; }

    [Required]
    [MaxLength(300)]
    public string QST_Description { get; set; }

    [ForeignKey(nameof(QST_EXE_FK))]
    public ExecutionConfigurationModel QstExecutionConfigurationModel { get; set; }
    public Guid QST_EXE_FK { get; set; }

    [InverseProperty(nameof(TestCaseModel.TST_Question))]
    public ICollection<TestCaseModel> QST_TestCases { get; set; } = [];
}