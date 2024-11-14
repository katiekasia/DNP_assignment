using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{
    private readonly IUserRepository userRepo;


    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]

    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var user = await userRepo.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            return Unauthorized(new { message = "User not found" });
        }
        
        if (user.Password != request.Password)
        {
            return Unauthorized(new { message = "Incorrect password" });
        }
        
        //sending the UserLoginDto WITHOUT THE PASSWORD
        var userLoginDto = new UserLoginDto()
        {
            Id = user.Id,
            Username = user.UserName
        };
        return Ok(userLoginDto);
    }
    
    
}