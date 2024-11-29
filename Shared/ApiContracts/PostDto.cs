namespace ApiContracts;

public class PostDto
{
    public int id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    
    public UserDto? Author { get; set; }
    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();

}