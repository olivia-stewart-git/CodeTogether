using CodeTogether.Client.Integration;
using Microsoft.AspNetCore.SignalR;

namespace CodeTogether.Hubs;

// A SignalR 'Hub' to implement live typing and over server push features
public class GameHub : Hub
{
	public async Task SendKeyPresses(Guid userId, List<KeyPressDTO> keyPresses)
	{
		await Clients.Others.SendAsync("ReceiveKeyPresses", userId, keyPresses);
	}
}