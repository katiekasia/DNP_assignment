using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService :IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    //Authentication of user

    public async Task<UserLoginDto> LoginAsync(LoginDto loginDto)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("api/login", loginDto);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Login failed");
        }
        return JsonSerializer.Deserialize<UserLoginDto>(response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
    
    
    public async Task<CreateUserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("users", request);
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CreateUserDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<UpdateUserDto> UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UpdateUserDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UserDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<UserDto>> GetUsersAsync(string? userNameContains = null)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users/{userNameContains}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<UserDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<PostDto>> GetPostsForUserAsync(int userId)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users/{userId}/posts");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<CommentDto>> GetCommentsForUserAsync(int userId)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"users/{userId}/comments");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
}