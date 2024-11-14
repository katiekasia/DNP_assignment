using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;

    private ClaimsPrincipal currentClaimsPrincipal =
        new ClaimsPrincipal(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(currentClaimsPrincipal ?? new());
    }

    public void Logout()
    {
        currentClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }

    public async Task<UserLoginDto> Login(LoginDto loginDto)
    {
        try
        {
            // Send a POST request to login endpoint
            HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync(
                "Auth/login", loginDto);

            // Debug print to track server response
            Console.WriteLine("Response Status Code: " + httpResponse.StatusCode);
            Console.WriteLine("Response Content: " + await httpResponse.Content.ReadAsStringAsync());

            if (!httpResponse.IsSuccessStatusCode)
            {
                // If the response is not successful, throw an exception with the response content
                string response = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"Error during login: {response}");
            }

            // Deserialize the response into UserLoginDto object
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            UserLoginDto userLoginDto =
                JsonSerializer.Deserialize<UserLoginDto>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            // Create claims and set the current authenticated user
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLoginDto.Username),
                new Claim("Id", userLoginDto.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
            currentClaimsPrincipal = new ClaimsPrincipal(identity);

            // Notify Blazor that authentication state has changed
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));

            // Return user login data for further processing if needed
            return userLoginDto;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Login method: {ex.Message}");
            throw;
        }
    }
}
