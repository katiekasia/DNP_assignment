using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreateComment
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public CreateComment(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public Task ShowAsync(int postId)
    {
        Console.WriteLine();
        return CreateCommentAsync(postId);
    }

    private Task CreateCommentAsync(int postId)
    {
            Console.WriteLine(
                "------------Bestieee you are adding a comment----------");
            Console.WriteLine("Comment content:");
            string? body = null;

            while (string.IsNullOrEmpty(body))
            {
                body = Console.ReadLine();
                if (string.IsNullOrEmpty(body))
                {
                    Console.WriteLine("body cannot be empty bestieee.");
                    continue;
                }

                if ("<".Equals(body))
                {
                    Console.WriteLine("Post creation cancelled.");

                    // return a completed task, to indicate that the post creation was cancelled. Normally I would just "return;", but this is a Task-returning method, so I need to return a Task.
                    return Task.CompletedTask;
                }
            }

            Console.WriteLine(
                "Please insert the ID of the user(bestieee) that created the post(that slayying content):");

            int userId = 0;
            while (userId == 0)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("ID cannot be empty.");
                    continue;
                }

                userId = int.Parse(input);
            }
            Console.WriteLine();
           
            return AddCommentAsync(body, postId, userId);
    }

    private async Task AddCommentAsync(string body, int postId, int userId)
    {
        Comment comment = new Comment(body, postId, userId);
        Comment toAdd = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Added comment: {comment.Body}" + "  to the user with ID: "+toAdd.PostId);
        
    }
}