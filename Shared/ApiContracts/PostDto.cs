namespace ApiContracts;

public class PostDto
{
    public int id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}