using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreateComment
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public CreateComment(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine();
        return CreateCommentAsync();
    }

    private Task CreateCommentAsync()
    {
        Console.WriteLine("------------Bestieee you are adding a comment----------");
        Console.WriteLine("Comment content:");
        string? content = null;
    }

    private async Task AddComment(Comment comment)
    {
        Comment comment = await 
    }
}