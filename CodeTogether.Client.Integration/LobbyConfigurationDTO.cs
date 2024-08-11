using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Client.Integration
{
	// Message from the client to the server
	// all fields should be nullable and only those that have changed will be non-null
	// this is to prevent overwriting settings the other players have changed if you havent changed them
	// it will still have a 'last write wins' situation on the changed setting but I think thats fine
	public class SetLobbyConfigurationDTO
	{
		public int? MaxPlayers { get; set; }

		public bool? GoingToStart { get; set; }

		public bool? IsPrivate { get; set; }
	}

	public class LobbyConfigurationDTO
	{
		public int MaxPlayers { get; set; }

		public DateTime? StartingAtUtc { get; set; }
		
		public bool IsPrivate { get; set; }
	}
}
