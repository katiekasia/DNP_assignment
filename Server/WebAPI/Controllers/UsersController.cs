using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        User user = new User(request.Username, request.Password);
        User created = await userRepo.AddAsync(user);
        CreateUserDto dto = new()
        {
            Username= created.UserName,
            Password = created.Password,
            Id = created.Id
        };
        return Created($"/Users/{dto.Id}", created);
    }

    [HttpPut("{id}:int")]
    public async Task<ActionResult<User>> UpdateUser([FromRoute] int id,[FromBody] UpdateUserDto request, [FromServices] IUserRepository userRepo)
    {
        User existing = await userRepo.GetSingleAsync(id);
        
        existing.UserName = request.Username;
        existing.Password = request.Password;
        await userRepo.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}:int")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await userRepo.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser([FromRoute] int id)
    {
        User user = await userRepo.GetSingleAsync(id);
        return Ok(user);
    }
    
    // This method takes a nullable string indicated with the question mark,
    // i.e. I explicitly state the value can be null. I also assign the parameter to null as a default value. 
    // This is not strictly necessary, but I find it adds to the readability of the code.
    // The point is that the client can optionally apply this query parameter.

    [HttpGet]
    public ActionResult<List<User>> GetUsers(
        [FromQuery] string? userNameContains = null)
    {
        IQueryable<User> users = userRepo.GetMany()
            .Where(
                u => userNameContains == null ||
                     u.UserName.ToLower()
                         .Contains((userNameContains.ToLower())));
        return Ok(users);
    }
    
 
    /*
     * Below endpoints are not strictly necessary. I can retrieve the same data from other endpoints.
     * But I want to illustrate that you can make multiple endpoints for the same data, if you want to.
     * This approach creates more dedicated, specialized endpoints, which can be easier to understand and use.
     */
    

    // I include this endpoint as an alternate example. My current Get-Many-Posts endpoint can take query parameters, to e.g. filter by user id.
    // But we could also create a dedicated endpoint for returning all posts written by a specific user.

    
    [HttpGet("{userId:int}/posts")]
    public async Task<ActionResult<List<Post>>> GetPostsForUser(
        [FromRoute] int userId,
        [FromServices] IPostRepository postRepo)

    {
        List<Post> posts = postRepo.GetMany()
            .Where(p => p.UserId == userId)
            .ToList();
        return Ok(posts);
    }
    // Here is another example, for getting all comments written by a user.
    [HttpGet("{userId:int}/comments")]
    public async Task<ActionResult<List<Comment>>> GetCommentsForUser(
        [FromRoute] int userId,
        [FromServices] ICommentRepository commentRepo)
    {
        List<Comment> comments = commentRepo.GetMany()
            .Where(c => c.UserId == userId)
            .ToList();
        return Ok(comments);
    }
    
    
}