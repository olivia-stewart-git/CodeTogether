using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Authentication;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace CodeTogether.Client.Services;

public class UserStateService(HttpClient http)
{
	// Stores the logged in user as a task to allow multiple components to be awaiting the response
	// will then be cached as the task is already completed
	Task<string?>? userNameTask;
	object gettingUsernameLock = new();

	public async Task<string?> GetUserName()
	{
		EnsureRequestHasBeenMade();
		return await userNameTask!;
	}

	void EnsureRequestHasBeenMade()
	{
		if (userNameTask != null)
		{
			return;
		}
		// Ensures that only one request is made
		lock (gettingUsernameLock)
		{
			// Second check is if another thread created the request since we last checked
			if (userNameTask != null)
			{
				return;
			}

			userNameTask = http.GetAsync("api/account/user").ContinueWith<Task<string?>>(responseTask =>
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
				// Unwrap() turns a Task<Task<T>> into a Task<T>
			}).Unwrap();
		}

	}

	public void ResetCache()
	{
		lock (gettingUsernameLock)
		{
			userNameTask = null;
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
		ResetCache();
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
