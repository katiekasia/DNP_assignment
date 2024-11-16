using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CreateCommentDto request,
        [FromServices] ICommentRepository commentRepo)
    {
        Comment comment =
            new Comment(request.Body, request.PostId, request.UserId);
        Comment created = await commentRepo.AddAsync(comment);
        CreateCommentDto dto = new()
        {
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };
        return Results.Created($"comments/{created.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplaceComment([FromRoute] int id,
        [FromBody] CreateCommentDto request,
        [FromServices] ICommentRepository commentRepo)
    {
        Comment existing = await this.commentRepo.GetSingleAsync(id);
        existing.Body = request.Body;
        existing.PostId = request.PostId;
        existing.UserId = request.UserId;
        await this.commentRepo.UpdateAsync(existing);
        return Results.Ok();

    }

    [HttpGet("user/{id}")]
    public async Task<IResult> GetCommentsByUserId([FromRoute] int id)
    {
        var comments = commentRepo.GetMany();
        comments= comments.Where(c => c.UserId == id);
        return Results.Ok();
    }
    [HttpGet("posts/{postId}")]
    public async Task<IResult> GetCommentsByPostId([FromRoute] int postId)
    {
        var comments = commentRepo.GetMany();
        comments = comments.Where(c => c.PostId == postId);
        return Results.Ok(comments);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);
        return Results.NoContent();
    }
     
}