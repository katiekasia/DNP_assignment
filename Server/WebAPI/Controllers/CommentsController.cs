using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IResult> AddComment([FromBody] CommentDto request,
        [FromServices] ICommentRepository commentRepo)
    {
        Comment comment =
            new Comment(request.Body, request.PostId, request.UserId);
        Comment created = await commentRepo.AddAsync(comment);
        CommentDto dto = new()
        {
            Id = created.Id,
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };
        return Results.Created($"comments/{created.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplaceComment([FromRoute] int id,
        [FromBody] CommentDto request,
        [FromServices] ICommentRepository commentRepo)
    {
        Comment existing = await this.commentRepo.GetSingleAsync(id);

        existing.Body = request.Body;
        existing.PostId = request.PostId;
        existing.UserId = request.UserId;

        await this.commentRepo.UpdateAsync(existing);

        CommentDto dto = new()
        { 
            Id = existing.Id,
            Body = existing.Body,
            PostId = existing.PostId,
            UserId = existing.UserId
        };

        return Results.Ok(dto);

    }

    [HttpGet("user/{id}")]
    public async Task<IResult> GetCommentsByUserId([FromRoute] int id)
    {
        var comments = await commentRepo.GetMany()
            .Where(c => c.UserId == id)
            .Select(c => new GetCommentDto
            {
                Id = c.Id,
                Body = c.Body,
                PostId = c.PostId,
               
            })
            .ToListAsync();

        return Results.Ok(comments);
    }
   
    [HttpGet("posts/{postId}")]
    public async Task<IResult> GetCommentsByPostId([FromRoute] int postId)
    {
        var comments = await commentRepo.GetMany()
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Body = c.Body,
                PostId = c.PostId,
                UserId = c.UserId
            })
            .ToListAsync();

        return Results.Ok(comments);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);
        return Results.NoContent();
    }
     
}