using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Client.Integration
{
	public class UserInfoDTO
	{
		public required string Name { get; set; }
		public required Guid Id { get; set; }
	}
}
