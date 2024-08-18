
namespace CodeTogether.Client.Integration
{
	// Message from the client to the server
	// all fields should be nullable and only those that have changed will be non-null
	// this is to prevent overwriting settings the other players have changed if you havent changed them
	// it will still have a 'last write wins' situation on the changed setting but I think thats fine
	[Serializable]
	public class SetLobbyConfigurationDTO
	{
		public int? MaxPlayers { get; set; }

		public bool? GoingToStart { get; set; }

		public bool? IsPrivate { get; set; }

		public bool? WaitForAllToFinish { get; set; }
	}

	[Serializable]
	public class LobbyConfigurationDTO
	{
		public required int MaxPlayers { get; set; }

		public required DateTime? StartingAtUtc { get; set; }

		public required bool IsPrivate { get; set; }

		public TimeSpan GameLength { get; set; }

		public bool WaitForAllToFinish { get; set; }
	}
}
