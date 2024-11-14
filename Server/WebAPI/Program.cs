using FileRepositories;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

//Add support for controllers
builder.Services.AddControllers();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 Now, a Controller can request any of the 
 I*Repository interfaces, as needed, and 
 the Dependency Injection functionality will handle creation for us. 
 This is very convenient.

Similar to the CLI project, the creation of specific 
implementations is isolated to a single place, 
so when we later need to swap out the repository implementations again, it will be very easy. 
We just modify these three lines of code.
 */

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

var app = builder.Build();

//Add support for controllers
app.MapControllers();

//Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Add this if authentication is needed
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();


app.Run();