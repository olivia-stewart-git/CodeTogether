using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Client.Integration
{
	public class JoinGameResponse
	{
		public required int serverId { get; set; }
		public required Guid playerId { get; set; }
	}
}
