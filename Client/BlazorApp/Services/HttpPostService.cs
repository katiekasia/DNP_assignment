using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CreatePostDto> CreatePostAsync(CreatePostDto request)
    {
       HttpResponseMessage httpResponse =
           await client.PostAsJsonAsync($"posts/", request);
       string response =
           await httpResponse.Content.ReadAsStringAsync();
       if (!httpResponse.IsSuccessStatusCode)
       {
           throw new Exception(response);
       }

       return JsonSerializer.Deserialize<CreatePostDto>(response,
           new JsonSerializerOptions
           {
               PropertyNameCaseInsensitive = true
           });
    }

    public async Task<UpdatePostDto> ReplacePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UpdatePostDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }
    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/id/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
    public async Task<List<PostDto>> GetPostsByTitleAsync(string? title = null)
    {
        string queryString = string.IsNullOrEmpty(title) ? "" : $"?title={title}";
        HttpResponseMessage httpResponse = await client.GetAsync($"posts{queryString}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        { 
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });   }
    

    public async Task<List<PostDto>> GetPostsByUserAsync(int userId)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/user/{userId}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        { 
            throw new Exception(response);
        }

        
        
        return JsonSerializer.Deserialize<List<PostDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
    
    
}