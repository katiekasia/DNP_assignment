using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        EntityEntry<Comment> commentEntity = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return commentEntity.Entity;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(c => c.Id == comment.Id)))
        {
            throw new Exception($"Comment with id {comment.Id} not found");
        }
        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existingComment = await ctx.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (existingComment == null)
        {
            throw new Exception($"Comment with id {id} not found");
        }
        ctx.Comments.Remove(existingComment);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
       Comment? comment = await ctx.Comments
           .Include(c => c.User)
           .Include(c=>c.Post)
           .FirstOrDefaultAsync(c => c.Id == id);
       if (comment == null)
       {
           throw new Exception($"Comment with id {id} not found");
       }

       return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        return ctx.Comments.AsQueryable();
    }
}