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
		public IEnumerable<string> Arguments { get; set; } = [];
		public string ExpectedResponse { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public bool HasBeenRun => Run != null;
		public string? ActualResult => Run?.ActualResult ?? string.Empty;
		public bool IsPassed => Run?.IsPassed ?? false;

		public RunDTO ? Run { get; set; } = null;
		public Guid Id { get; set; } = Guid.Empty;

		public class RunDTO
		{
			public Guid Id { get; set; } = Guid.Empty;
			public string? ActualResult { get; set; } = null;
			public bool IsPassed { get; set; } = false;
		}
	}
}
