namespace CodeTogether.Client.Services;

public class UserStateService(HttpClient http)
{
	string? userName;
	bool hasTriedGettingUserFromServer = false;

	public async Task<string?> GetUserName()
	{
		if (userName == null && hasTriedGettingUserFromServer)
		{
			return null;
		}

		if (userName != null)
		{
			return userName;
		}

		var response = await http.GetAsync("api/account/user");
		if (response.IsSuccessStatusCode)
		{
			userName = await response.Content.ReadAsStringAsync();
		}
		hasTriedGettingUserFromServer = true;
		return userName;
	}

	public void CreateUser()
	{
	}
}
