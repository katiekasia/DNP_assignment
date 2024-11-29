using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<CreateCommentDto> AddComment(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync($"comments/", request);
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CreateCommentDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<UpdateCommentDto> ReplaceComment(int id, UpdateCommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"comments/{id}", request);
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UpdateCommentDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task DeleteComment(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"{id}");
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
    }

    public async Task<List<CommentDto>> GetCommentByUserId(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"user/{id}");
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<List<CommentDto>> GetCommentsByPostId(int postId)
    {
        HttpResponseMessage httpResponse =
            //this list is the same as the one on API 
            await client.GetAsync($"comments/posts/{postId}");
        string response =
            await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })?? new List<CommentDto>();
    }
}