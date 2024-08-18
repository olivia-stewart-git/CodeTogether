using CodeTogether.Client.Integration.Authentication;
using System.Text.Json;
using System.Net.Http.Json;
using CodeTogether.Client.Integration;

namespace CodeTogether.Client.Services;

public class UserStateService(HttpClient http)
{
	// Stores the logged in user as a task to allow multiple components to be awaiting the response
	// will then be cached as the task is already completed
	Task<UserInfoDTO?>? userNameTask;
	object gettingUsernameLock = new();

	public async Task<UserInfoDTO?> GetUserName()
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
			userNameTask = http.GetAsync("api/account/user").ContinueWith<Task<UserInfoDTO?>>(responseTask =>
			{
				var response = responseTask.Result;
				if (response.IsSuccessStatusCode)
				{
					var userInfo = response.Content.ReadFromJsonAsync<UserInfoDTO>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					UsernameChanged?.Invoke(this, new());
					return userInfo;
				}
				else
				{
					return Task.FromResult<UserInfoDTO?>(null);
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

	public event EventHandler UsernameChanged;

	/// <summary>
	/// Try perform logon
	/// </summary>
	/// <param name="loginModel"></param>
	/// <returns></returns>
	/// <exception cref="LoginFailedException"></exception>
	public async Task LoginAsUser(LoginRequestDTO loginModel)
	{
		var response = await http.PostAsJsonAsync("/api/account/login", loginModel);
		LoginResponseDTO? dtoResponse;
		try 
		{
			response.EnsureSuccessStatusCode();
			dtoResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
		}
		catch (Exception ex)
		{
			throw new LoginFailedException($"Network error: {ex.Message}{Environment.NewLine}{await response.Content.ReadAsStringAsync()}");
		}

		var loginSuccess = dtoResponse?.IsAuthenticated ?? false;
		if (!loginSuccess)
		{
			throw new LoginFailedException(dtoResponse?.Message ?? "Login failed without message");
		}
		ResetCache();
	}

	public async Task RegisterUser(RegisterAccountDTO registration)
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
