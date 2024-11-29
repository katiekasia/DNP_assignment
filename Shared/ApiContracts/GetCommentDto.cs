namespace ApiContracts;

public class GetCommentDto
{
    public string Body { get; set; }
    public int PostId { get; set; }
    public int Id { get; set; }
}