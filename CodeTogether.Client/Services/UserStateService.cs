using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace CodeTogether.Client.Services;

public class UserStateService(HttpClient http)
{
	// Store the task rather than the result to allow multiple components to hit GetUserName without making multiple requests
	Task<string?>? userName;
	object gettingUsernameLock = new();

	public async Task<string?> GetUserName()
	{
		EnsureRequestHasBeenMade();
		return await userName!;
	}

	void EnsureRequestHasBeenMade()
	{
		if (userName == null)
		{
			lock (gettingUsernameLock)
			{
				// Second check if another thread created the request since we last checked
				if (userName == null)
				{
					userName = http.GetAsync("api/account/user").ContinueWith<string?>(responseTask =>
					{
						var response = responseTask.Result;
						if (response.IsSuccessStatusCode)
						{
							// Second ContinueWith is to cast from Task<string> to Task<string?>
							return response.Content.ReadAsStringAsync().ContinueWith(c => (string?)c.Result);
						}
						else
						{
							return Task.FromResult<string?>(null);
						}
					});
				}
			}
		}
	}

	public async Task LoginAsUser(LoginRequestDTO loginModel)
	{
		var response = await http.PostAsJsonAsync("/api/account/login", loginModel);
		var dtoResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
		var loginSuccess = dtoResponse?.IsAuthenticated ?? false;
		if (!loginSuccess)
		{
			throw new LoginFailedException(dtoResponse?.Message ?? "Login failed without message");
		}
	}

	async public Task RegisterUser(RegisterAccountDTO registration)
	{
		var response = await http.PostAsJsonAsync("/api/account/register", registration);
		if (!response.IsSuccessStatusCode) // Network error
		{
			var content = await response.Content.ReadAsStringAsync();
			throw new RegistrationFailedException(content);
		}

		var registrationResponse = await response.Content.ReadFromJsonAsync<RegistrationRequestResponseDTO>();
		if (registrationResponse.State != RegistrationState.Success) // Application error
		{
			throw new RegistrationFailedException(registrationResponse.Message);
		}
	}
}
