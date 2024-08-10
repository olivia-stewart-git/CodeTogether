using Microsoft.AspNetCore.SignalR;

namespace CodeTogether.Hubs;

// A SignalR 'Hub' to implement live typing and over server push features
public class GameHub : Hub
{
	public async Task SendMessage(string message)
	{
		await Clients.Others.SendAsync("SetState", message);
	}

	public override async Task OnConnectedAsync()
	{
		await Clients.Caller.SendAsync("SetState", "Fake Initial Code State");
	}
}