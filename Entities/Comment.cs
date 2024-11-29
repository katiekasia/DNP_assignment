using System.Text.Json.Serialization;

namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } 
    public int PostId { get; set; } // this property has no setter, because it does not make sense to modify it after creation.
    public int UserId { get; set; }

    [JsonIgnore]
    public Post Post { get; set; } = null!;
    [JsonIgnore]
    public User User { get; set; } = null!;

    private Comment() { } // For EF Core

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}