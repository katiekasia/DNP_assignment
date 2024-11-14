using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<CreateUserDto> AddUserAsync(CreateUserDto request);
    public Task<UpdateUserDto> UpdateUserAsync(int id, UpdateUserDto request);
    public Task DeleteUserAsync(int id);
    public Task<UserDto> GetUserAsync(int id);
    public Task<List<UserDto>> GetUsersAsync(string? userNameContains = null);
    public Task<List<PostDto>> GetPostsForUserAsync(int userId);
    public Task<List<CommentDto>> GetCommentsForUserAsync(int userId);

}