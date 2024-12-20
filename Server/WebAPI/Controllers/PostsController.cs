using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

/*
 *· [ApiController] marks this class as a controller, which must be picked by the application when it startes. That’s the ”builder.Services.AddControllers()” call we did above.
 */
[ApiController]
/*
 · [Route("[controller]")] says the route to this controller will be: ”localhost:port/Users”, because the class starts with Users(Controller)
 */
[Route("[controller]")]
/*
 · The class extends ControllerBase, which gives access to helper methods, and other stuff.
 */
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostsController(IPostRepository postRepo)
    {
        /*
         · The class receives a repository through the constructor, which each endpoint can use to manipulate.
         */
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CreatePostDto>> CreatePost(
        [FromBody] CreatePostDto request, IPostRepository postRepo)
    {
        Post post = new Post(request.Title, request.Body,
            request.UserId);
        Post created = await postRepo.AddAsync(post);
        CreateCommentDto dto = new()
        {
            Body = created.Body,
            UserId = created.UserId
        };
        return Created($"posts/{created.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplacePost([FromRoute] int id,
        [FromBody] UpdatePostDto req, [FromServices] IPostRepository postRepo)
    {
        Post existingPost = await postRepo.GetSingleAsync(id);
        existingPost.Title = req.Title;
        existingPost.Body = req.Body;
        existingPost.UserId = req.UserId;

        await postRepo.UpdateAsync(existingPost);
        return Results.Ok();
    }

    [HttpGet("id/{id}")]
    public async Task<IResult> GetPost([FromRoute] int id,
        [FromQuery] bool includeAuthor, 
        [FromQuery] bool includeComments)
    {
        IQueryable<Post> queryForPost =
            postRepo.GetMany()
                .Where(p => p.Id == id).AsQueryable();
        if (includeAuthor)
        {
            queryForPost = queryForPost.Include(p => p.User);
        }

        if (includeComments)
        {
            queryForPost = queryForPost.Include(p => p.Comments);
        }

        PostDto? dto = await queryForPost.Select(post => new PostDto()
        {
            id = post.Id, Title = post.Title, Body = post.Body,
            UserId = post.UserId,
            Author = includeAuthor
                ? new UserDto
                    { Id = post.User.Id, 
                        Username = post.User.UserName }
                : null,
            Comments = includeComments
                ? post.Comments.Select(c => new CommentDto
                        { Id = c.Id, Body = c.Body, UserId = c.UserId })
                    .ToList()
                : new()
        }).FirstOrDefaultAsync();
        return dto == null ? Results.NotFound() : Results.Ok(dto);
    }

    [HttpGet]
    public async Task<IResult> GetPostsByTitle([FromQuery] string? title)
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrEmpty(title))
        {
            posts = posts.Where(
                t => t.Title.ToLower().Contains(title.ToLower()));
        }

        var postsList = await posts.ToListAsync();
        return Results.Ok(postsList);
    }

    [HttpGet("user/{userId}")]
    public async Task<IResult> GetPostsByUser([FromRoute] int? userId)
    {
        var posts = postRepo.GetMany()
            .Where(t => t.UserId == userId);

        var postsList = await posts.ToListAsync();
        return Results.Ok(postsList);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}