using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Client.Integration.Execution
{
	[Serializable]
	public class TestCaseDto
	{
		public string Name { get; set; } = string.Empty;
		public string[] Arguments { get; set; } = [];
		public string ExpectedResponse { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
