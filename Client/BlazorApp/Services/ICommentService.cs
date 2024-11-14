using ApiContracts;
using Entities;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CreateCommentDto> AddComment(CreateCommentDto request);
    public Task<UpdateCommentDto> ReplaceComment(int id, UpdateCommentDto request);
    public Task DeleteComment(int id);
    public Task<List<CommentDto>> GetCommentByUserId(int id);
    public Task<List<CommentDto>> GetCommentsByPostId(int postId);
    
    
    
}