﻿namespace CodeTogether.Client.Integration;

// Message from the server to client
[Serializable]
public class LobbyStateDTO
{
	public required string Name { get; set; }
	public required IEnumerable<string> Players { get; set; }
	public required LobbyConfigurationDTO Configuration { get; set; }
}
