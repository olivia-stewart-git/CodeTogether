using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTogether.Data.DataAccess;

namespace CodeTogether.Data.Models;

public class StmDataModel : IDbModel
{
	public StmDataModel(string key, string data)
	{
		Key = key;
		Data = data;
	}

    [Key]
    public Guid STM_PK { get; set; } = Guid.NewGuid();

    [MaxLength(20)]
    public string Key { get; set; }

    [MaxLength(100)]
    public string Data { get; set; }
}