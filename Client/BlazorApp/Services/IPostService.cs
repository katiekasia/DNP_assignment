using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<CreatePostDto> CreatePostAsync(CreatePostDto request);
    public Task<UpdatePostDto> ReplacePostAsync(int id, UpdatePostDto request);
    public Task DeletePostAsync(int id);
    public Task<PostDto> GetPostAsync(int id);
    public Task<List<PostDto>> GetPostsByTitleAsync(string?title= null);
    public Task<List<PostDto>> GetPostsByUserAsync(int userId);

    
    
}