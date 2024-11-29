using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> userEntity = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return userEntity.Entity;
    }

    public async Task UpdateAsync(User user)
    {
        if (!(await ctx.Users.AnyAsync(u => u.Id == user.Id)))
        {
            throw new Exception($"User with id {user.Id} not found");
        }
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existingUser = await ctx.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            throw new Exception($"User with id {id} not found");
        }
        ctx.Users.Remove(existingUser);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await ctx.Users
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"User with id {id} not found");
        }
        return user;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        User? user = await ctx.Users
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
        {
            throw new Exception($"User with username '{username}' not found");
        }
        return user;
    }

    public IQueryable<User> GetMany()
    {
        return ctx.Users.AsQueryable();
    }
}